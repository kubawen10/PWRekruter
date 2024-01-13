using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PWRekruter.Controllers;
using PWRekruter.Data;
using PWRekruter.Models;
using PWRekruter.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PWRekruter.Tests.Controller
{
    public class KandydaciControllerTests
    {
        [Fact]
        public async void KandydaciController_Index_ReturnsSuccess()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PWRekruterDbContext>();
            var context = new Mock<PWRekruterDbContext>(optionsBuilder.Options);
            var loginService = new Mock<ILoginService>();
            var controller = new KandydaciController(context.Object, loginService.Object);
            var sampleKandydat = new Kandydat
            {
                Id = 1,
                Email = "Email",
                Haslo = "Haslo",
                Imie = "John",
                DrugieImie = "Doe",
                Nazwisko = "Smith",
                Pesel = "12345678901",
                Plec = Plec.Meżczyzna,
                DataUrodzenia = new DateTime(1990, 1, 1),
                Panstwo = "Poland",
                KodPocztowy = "00-000",
                Miejscowosc = "Warsaw",
                Ulica = "Main Street",
                NumerBudynku = "123",
                NumerMieszkania = "456",
                WynikiEgzaminow = new List<WynikEgzaminu>(),
                Aplikacja = null
            };
            var mockSet = new Mock<DbSet<Kandydat>>();
            context.Setup(m => m.Kandydaci).Returns(mockSet.Object);
            mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).Returns(ValueTask.FromResult(sampleKandydat));


            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Kandydat>(viewResult.Model);
            Assert.Equal(sampleKandydat, model);
        }

        [Fact]
        public async void KandydaciController_Index_ReturnsNotFound()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PWRekruterDbContext>();
            var context = new Mock<PWRekruterDbContext>(optionsBuilder.Options);
            var loginService = new Mock<ILoginService>();
            var controller = new KandydaciController(context.Object, loginService.Object);
            var mockSet = new Mock<DbSet<Kandydat>>();
            context.Setup(m => m.Kandydaci).Returns(mockSet.Object);
            mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).Returns(ValueTask.FromResult<Kandydat>(null));

            var result = await controller.Index();

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
