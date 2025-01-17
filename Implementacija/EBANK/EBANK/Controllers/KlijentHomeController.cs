﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EBANK.Models;
using EBANK.Models.AdministratorRepository;
using EBANK.Data;
using EBANK.Models.KlijentRepository;
using EBANK.Models.NovostRepository;
using EBANK.Utils;
using Microsoft.AspNetCore.Http;
using EBANK.Models.FilijaleBankomatiRepository;

namespace EBANK.Controllers
{
    public class KlijentHomeController : Controller
    {
        private KlijentiProxy _klijenti;
        private OglasnaPlocaProxy _oglasnaPloca;
        private FilijaleBankomatiProxy _filijaleBankomatiProxy;
        private Korisnik korisnik;
        private OOADContext Context;

        public KlijentHomeController(OOADContext context)
        {
            _klijenti = new KlijentiProxy(context);
            _oglasnaPloca = new OglasnaPlocaProxy(context);
            _filijaleBankomatiProxy = new FilijaleBankomatiProxy(context);
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            korisnik = await LoginUtils.Authenticate(Request, Context, this);
            if (korisnik == null) return RedirectToAction("Logout", "Login", new { area = "" });

            _klijenti.Pristupi(korisnik);
            _oglasnaPloca.Pristupi(korisnik);
            _filijaleBankomatiProxy.Pristupi(korisnik);

            ViewData["ime"] = korisnik.Ime;

            ViewData["bankomati"] = await _filijaleBankomatiProxy.DajSveBankomate();
            ViewData["filijale"] = await _filijaleBankomatiProxy.DajSveFilijale();

            return View(await _oglasnaPloca.DajSvePrikazaneNovosti());
        }
    }
}
