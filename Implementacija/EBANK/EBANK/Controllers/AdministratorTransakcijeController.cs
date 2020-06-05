﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBANK.Data;
using EBANK.Models;
using EBANK.Models.TransakcijaRepository;
using EBANK.Models.AdministratorRepository;
using EBANK.Utils;

namespace EBANK.Controllers
{
    public class AdministratorTransakcijeController : Controller
    {
        private readonly TransakcijeProxy _transakcije;
        private OOADContext Context;
        private Korisnik korisnik;

        public AdministratorTransakcijeController(OOADContext context)
        {
            Context = context;
            _transakcije = new TransakcijeProxy(context);
        }

        // GET: AdministratorTransakcije
        public async Task<IActionResult> Index()
        {
            korisnik = await LoginUtils.Authenticate(Request, Context, this);
            if (korisnik == null) return RedirectToAction("Logout", "Login", new { area = "" });

            _transakcije.Pristupi(korisnik);

            ViewData["Ime"] = korisnik.Ime;

            return View(await _transakcije.DajSveTransakcije());
        }

        private bool TransakcijaExists(int id)
        {
            return _transakcije.DaLiPostojiTransakcija(id);
        }
    }
}
