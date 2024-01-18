using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PWRekruter.Controllers;
using PWRekruter.Data;
using PWRekruter.Enums;
using PWRekruter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PWRekruter.Tests.Controller
{
    public class AplikacjeControllerTests 
    {
        private readonly PWRekruterDbContext _context;
        private readonly AplikacjeController _controller;

        public AplikacjeControllerTests()
        {
            var options = new DbContextOptionsBuilder<PWRekruterDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new PWRekruterDbContext(options);
            _controller = new AplikacjeController(_context, null);
            if (!_context.Aplikacje.Any())
            {
                SeedData();
            }
        }

        private void SeedData()
        {
            var wydzialy = new List<Wydzial>
            {
                new Wydzial { Symbol = "W4", Nazwa = "Wydział informatyki i telekomunikacji" },
                new Wydzial { Symbol = "W8", Nazwa = "Wydział zarządzania" }
            };

            var kandydaci = new List<Kandydat>
            {
                 new Kandydat { Id = 1, Email = "kandydat1", Haslo = "haslo",
                        Imie = "Adam", DrugieImie = "Paweł", Nazwisko = "Nowak"},
                    new Kandydat { Id = 2, Email = "kandydat2", Haslo = "haslo",
                        Imie = "Jan", DrugieImie = "Jakub", Nazwisko = "Kowalski"},
                    new Kandydat { Id = 3, Email = "kandydat3", Haslo = "haslo" },
                    new Kandydat { Id = 4, Email = "kandydat4", Haslo = "haslo" },
                    new Kandydat { Id = 5, Email = "kandydat5", Haslo = "haslo" }
            };

            var kierunki = new List<Kierunek>
            {
               new Kierunek
                    {
                        Id=1,
                        Nazwa="Informatyka stosowana",
                        Skrot = "IST",
                        Stopien=StopienStudiow.Istopien,
                        Forma = FormaStudiow.Stacjonarne,
                        Tryb = TrybStudiow.Dzienne,
                        CzasTrwania=7,
                        Czesne=0.0,
                        CzesneDlaCudzoziemcow = 1250.0,
                        JezykWykladowy = Jezyk.Polski,
                        DyscyplinaNaukowa="Informatyka",
                        LiczbaMiejsc=150,
                        OplataRekrutacyjna = 80,
                        Opis = "Studia na tym kierunku umożliwiają doskonalenie umiejętności i zdobywanie wiedzy z szeroko pojmowanej informatyki. Studia pokazują różnorodność zastosowań informatyki, m.in. w rozwiązywaniu problemów biznesowych, technicznych, czy w obszarze gier komputerowych. Informatyka stosowana jest uzupełniana wiedzą z fizyki i matematyki, podstaw zarządzania i komunikacji społecznej.<br><br>Studenci kształcą się w zakresie:<br>- programowania oraz języków i technik programowania;<br>- algorytmów i struktury danych<br>- sieci komputerowych<br>- baz i hurtowni danych<br>- systemów mobilnych, rozproszonych i webowych<br>- multimediów oraz inteligentnych systemów informatycznych<br>- zarządzania projektami informatycznymi",
                        Profil = ProfilKierunku.Ogolnoakademicki,
                        SymbolWydzialu = "W4",
                        IdProgramuStudiow = 1
                    },
                    new Kierunek
                    {
                        Id = 2,
                        Nazwa = "Inżynieria zarządzania",
                        Skrot = "IZ",
                        Stopien = StopienStudiow.IIstopien,
                        Forma = FormaStudiow.Stacjonarne,
                        Tryb = TrybStudiow.Dzienne,
                        CzasTrwania = 7,
                        Czesne = 0.0,
                        CzesneDlaCudzoziemcow = 1500.0,
                        JezykWykladowy = Jezyk.Polski,
                        DyscyplinaNaukowa = "Nauki o zarządzaniu i jakości",
                        LiczbaMiejsc = 120,
                        OplataRekrutacyjna = 80,
                        Opis = "Współczesna branża IT zgłasza ogromne zapotrzebowanie na menadżerów, którzy posiadają zarówno twardą wiedzę techniczną, jak i znają metody oraz narzędzia podejmowania odpowiednich decyzji w zarządzaniu. Inżynieria Zarządzania na naszym Wydziale jest idealną odpowiedzią na taką potrzebę. Nasi Absolwenci są poszukiwanymi na rynku specjalistami, którzy potrafią łączyć wiedzę i miękkie umiejętności menadżerskie z twardymi kompetencjami inżynierskimi.\r\n\r\nKształtowanie kompetencji biznesowych i inżynierskich\r\n\r\nNasz absolwent potrafi formułować i rozwiązywać zadania o charakterze inżynierskim, szczególnie tych dotyczących procesów biznesowych, procesów innowacyjnych, projektów, zastosowania IT w biznesie. Umie dostrzegać ich aspekty systemowe oraz posługiwać się właściwymi normami i standardami, także pozatechnicznymi – ekonomicznymi, prawnymi, ekologicznymi, psychologicznymi, zawodowymi i moralnymi\r\n\r\nKształtowanie kompetencji analitycznych\r\n\r\nNasz absolwent rozumie procesy i zjawiska materialne, finansowe i społeczne zachodzące w organizacjach i ich otoczeniu. Potrafi myśleć analitycznie i wykorzystuje w tym celu podstawowy aparat matematyczny i statystyczny oraz umiejętności logicznego myślenia i wnioskowania.\r\n\r\nKształtowanie kompetencji społecznych\r\n\r\nNasz absolwent potrafi w współdziałać i pracować w grupowych i zespołowych formach organizacji pracy (przyjmując w nich różne role). Potrafi organizować pracę małych zespołów i nimi kierować. Jest przygotowany do odpowiedzialnego pełnienia ról zawodowych z uwzględnieniem zmieniających się potrzeb społecznych\r\n\r\nKształtowanie kompetencji informatyczno-technologicznych\r\n\r\nNasz absolwent ma uporządkowaną, podbudowaną teoretycznie wiedzę ogólną dotyczącą narzędzi i technologii implementacji SIZ, modelowania procesów biznesowych, inżynierii zarządzania projektami, a także obejmującą kluczowe zagadnienia w zakresie zastosowania IT w biznesie. Zna obowiązujące trendy w IT i potrafi niektóre z nich zastosować.",
                        Profil = ProfilKierunku.Ogolnoakademicki,
                        SymbolWydzialu = "W8",
                    }
        };

            var aplikacje = new List<Aplikacja>
            {
                new Aplikacja
                {
                    Id = 1,
                    DataZlozenia = DateTime.Now,
                    IdKandydata = 1,
                    Oplacona = true,
                    Status = StatusAplikacji.Zlozona,
                    IdTuryRekrutacji = 1
                },
                new Aplikacja
                {
                    Id = 2,
                    DataZlozenia = DateTime.Now,
                    IdKandydata = 2,
                    Oplacona = true,
                    Status = StatusAplikacji.Zlozona,
                    IdTuryRekrutacji = 1
                }
            };

            var preferencje = new List<Preferencja>
            {
                new Preferencja
                {
                    Id = 1,
                    IdAplikacji = 1,
                    IdKierunku = 1,
                    Priorytet = 1,
                    WartoscWskaznika = 477.7
                },
                new Preferencja
                {
                    Id = 2,
                    IdAplikacji = 1,
                    IdKierunku = 2,
                    Priorytet = 2,
                    WartoscWskaznika = 480.1,
                    IdWybranejSpecjalizacji = 1
                },
                new Preferencja
                {
                    Id = 3,
                    IdAplikacji = 2,
                    IdKierunku = 2,
                    Priorytet = 1,
                    WartoscWskaznika = 477.7
                }
            };

            _context.Kandydaci.AddRange(kandydaci);
            _context.Wydzialy.AddRange(wydzialy);
            _context.Kierunki.AddRange(kierunki);
            _context.Aplikacje.AddRange(aplikacje);
            _context.Preferencje.AddRange(preferencje);

            _context.SaveChanges();
        }



        [Fact]
        public async Task Index_ReturnsViewResultAplikacjeFilteredByName()
        {
            var result = await _controller.Index(null, "Nowak", "", null, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Aplikacja>>(viewResult.ViewData.Model);
            Assert.Single(model);
            Assert.Equal(1, model.First().Id);

            result = await _controller.Index(null, "", "", null, null);
            viewResult = Assert.IsType<ViewResult>(result);
            model = Assert.IsAssignableFrom<IEnumerable<Aplikacja>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            var uniqueIds = new HashSet<long>(model.Select(aplikacja => aplikacja.Id));
            Assert.True(uniqueIds.SetEquals(new HashSet<long> { 1, 2 }));

            result = await _controller.Index(null, "Jakub", "", null, null);
            viewResult = Assert.IsType<ViewResult>(result);
            model = Assert.IsAssignableFrom<IEnumerable<Aplikacja>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }

        [Fact]
        public async Task ChangeAppResult_UpdatesResultAndReturnsContentForValidInput()
        {   
            var result = await _controller.ChangeAppResult(1, "akceptuj");
            Assert.IsType<ContentResult>(result);
            var contentResult = (ContentResult)result;
            Assert.Equal("Zapisano zmiany", contentResult.Content);
            var updatedPref = _context.Preferencje.FirstOrDefault(p => p.Id == 1);
            Assert.Equal(WynikAplikacji.Zakwalifikowano,updatedPref.Wynik);

            result = await _controller.ChangeAppResult(2, "odrzuc");
            Assert.IsType<ContentResult>(result);
            contentResult = (ContentResult)result;
            Assert.Equal("Zapisano zmiany", contentResult.Content);
            updatedPref = _context.Preferencje.FirstOrDefault(p => p.Id == 2);
            Assert.Equal(WynikAplikacji.Odrzucono,updatedPref.Wynik);

            result = await _controller.ChangeAppResult(1, "usun");
            Assert.IsType<ContentResult>(result);
            contentResult = (ContentResult)result;
            Assert.Equal("Zapisano zmiany", contentResult.Content);
            updatedPref = _context.Preferencje.FirstOrDefault(p => p.Id == 1);
            Assert.Null(updatedPref.Wynik);
        }
    }
}
