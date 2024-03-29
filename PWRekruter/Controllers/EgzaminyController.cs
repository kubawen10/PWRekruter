﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Enums;
using PWRekruter.Models;
using PWRekruter.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;

namespace PWRekruter.Controllers
{
    public class EgzaminyController : Controller
    {

        private readonly PWRekruterDbContext _context;
        private readonly ILoginService _loginService;

        public EgzaminyController(PWRekruterDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        // GET: Egzaminy/Edit
        public IActionResult Edit()
        {
            List<dynamic> typWynikuEgzaminuList = new List<dynamic>();

            foreach (var typWyniku in Enum.GetValues(typeof(TypWynikuEgzaminu)).Cast<TypWynikuEgzaminu>())
            {
                dynamic typ = new ExpandoObject();
                typ.val = (Convert.ToInt32(typWyniku)).ToString();
                typ.text = typWyniku.GetEnumLabel();
                typWynikuEgzaminuList.Add(typ);
            }

            ViewBag.TypWynikuEgzaminuList = typWynikuEgzaminuList;

            return View();
        }

        public IActionResult GetEgzaminForm(int id)
        {
            if (!Enum.IsDefined(typeof(TypWynikuEgzaminu), id))
            {
                return RedirectToAction(nameof(Edit));
            }
            else
            {
                switch (id)
                {
                    case (int)TypWynikuEgzaminu.Olimpiada:
                        return RedirectToAction(nameof(OlimpiadaForm));
                    default:
                        return RedirectToAction(nameof(MaturaOkeForm));
                }
            }
        }

        public IActionResult OlimpiadaForm()
        {
			int kandydatId = _loginService.GetUserId();

            List<WynikOlimpiady> olimpiady = _context.WynikiOlimpiady.Where(w => w.KandydatId == kandydatId).ToList();

			foreach (Olimpiada olimpiada in Enum.GetValues(typeof(Olimpiada)))
			{
				if (!olimpiady.Any(o => o.Olimpiada == olimpiada))
				{
					olimpiady.Add(new WynikOlimpiady { Olimpiada = olimpiada });
				}
			}

            olimpiady = olimpiady.OrderBy(o => o.Olimpiada).ToList();

			return PartialView(olimpiady);
        }

        public IActionResult MaturaOkeForm()
        {
			int kandydatId = _loginService.GetUserId();

            List<WynikPrzedmiotowy> wynikiPrzedmiotowe =_context.WynikiPrzedmiotowe.Where(w => w.WynikMaturyOKE.KandydatId == kandydatId).ToList();

            foreach (TypPrzedmiotu typPrzedmiotu in Enum.GetValues(typeof(TypPrzedmiotu)))
            {
                foreach(PoziomPrzedmiotu poziomPrzedmiotu in Enum.GetValues(typeof(PoziomPrzedmiotu)))
                {
					if (!wynikiPrzedmiotowe.Any(w => w.TypPrzedmiotu == typPrzedmiotu && w.PoziomPrzedmiotu==poziomPrzedmiotu))
					{
						wynikiPrzedmiotowe.Add(new WynikPrzedmiotowy { TypPrzedmiotu = typPrzedmiotu, PoziomPrzedmiotu = poziomPrzedmiotu, Wynik = 0 });
                    }
				}
            }

			wynikiPrzedmiotowe = wynikiPrzedmiotowe.OrderBy(w => w.TypPrzedmiotu).ThenBy(w => w.PoziomPrzedmiotu).ToList();

			return PartialView(wynikiPrzedmiotowe);
        }

        [HttpPost]
        public IActionResult EditOlimpiada(IList<WynikOlimpiady> wyniki)
        {
			foreach (var wynik in wyniki)
			{
				var istniejacyWynik = _context.WynikiOlimpiady
						.FirstOrDefault(w => w.Id == wynik.Id);

				if (wynik.TytulOlimpijczyka != null && istniejacyWynik != null)
				{
					istniejacyWynik.TytulOlimpijczyka = wynik.TytulOlimpijczyka;
					_context.WynikiOlimpiady.Update(istniejacyWynik);
				} else if (wynik.TytulOlimpijczyka != null && istniejacyWynik == null)
                {
					wynik.KandydatId = _loginService.GetUserId();
					_context.WynikiOlimpiady.Add(wynik);
                } else if (wynik.TytulOlimpijczyka == null && istniejacyWynik != null)
                {
					_context.WynikiOlimpiady.Remove(istniejacyWynik);
				}
			}

			_context.SaveChanges();
			return RedirectToAction(nameof(KandydaciController.Index), "Kandydaci");
        }

		[HttpPost]
		public IActionResult EditMaturaOke(IList<WynikPrzedmiotowy> wyniki)
		{
            int kandydatId = _loginService.GetUserId();
			WynikMaturyOKE istniejaceWynikiMaturyOke = _context.WynikiMaturyOKE.Where(w => w.KandydatId == kandydatId).Include(w=>w.WynikiPrzedmiotowe).FirstOrDefault();

            if (istniejaceWynikiMaturyOke == null)
            {
				UtworzNowyWynikMaturyOke(kandydatId, wyniki);
			}
            else
            {
				ZaktualizujWynikMaturyOke(istniejaceWynikiMaturyOke.Id, wyniki);
			}

			return RedirectToAction(nameof(KandydaciController.Index), "Kandydaci");
		}

        private void UtworzNowyWynikMaturyOke(int kandydatId, IList<WynikPrzedmiotowy> wyniki)
        {
			var wynikMaturyOKE = new WynikMaturyOKE { KandydatId = kandydatId, WynikiPrzedmiotowe = new Collection<WynikPrzedmiotowy>() };

			foreach (var wynik in wyniki)
			{
				if (wynik.Wynik != 0)
				{
					wynikMaturyOKE.WynikiPrzedmiotowe.Add(wynik);
				}
			}

            if(wynikMaturyOKE.WynikiPrzedmiotowe.Count > 0)
            {
				_context.WynikiMaturyOKE.Add(wynikMaturyOKE);
				_context.SaveChanges();
			}
		}

		private void ZaktualizujWynikMaturyOke(int istniejacyWynikMaturyId, IList<WynikPrzedmiotowy> wyniki)
        {
			foreach (var wynik in wyniki)
			{
				var istniejacyWynikPrzedmiotowy = _context.WynikiPrzedmiotowe.Where(w => w.Id == wynik.Id).FirstOrDefault();

				if (istniejacyWynikPrzedmiotowy != null && wynik.Wynik != 0 && wynik.Wynik != istniejacyWynikPrzedmiotowy.Wynik)
				{
					istniejacyWynikPrzedmiotowy.Wynik = wynik.Wynik;
					_context.WynikiPrzedmiotowe.Update(istniejacyWynikPrzedmiotowy);
				}
				else if (istniejacyWynikPrzedmiotowy != null && wynik.Wynik == 0)
				{
					_context.WynikiPrzedmiotowe.Remove(istniejacyWynikPrzedmiotowy);
				}
				else if (istniejacyWynikPrzedmiotowy == null && wynik.Wynik != 0)
				{
					wynik.WynikMaturyOKEId = istniejacyWynikMaturyId;
					_context.WynikiPrzedmiotowe.Add(wynik);
				}
			}
			_context.SaveChanges();
		}
	}
}
