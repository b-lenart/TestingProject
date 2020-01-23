using coderush.Controllers;
using coderush.Controllers.Api;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace MgmtSystemTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void About()
        {
            HomeController homeController = new HomeController();

            ViewResult result = homeController.About() as ViewResult;

            Assert.AreEqual("Your application description page.", result.ViewData["Message"]);
        }

        [Test]
        public void HomeControllerTest()
        {
            HomeController homeController = new HomeController();
            IActionResult result = homeController.Index();
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
        }
    }
}