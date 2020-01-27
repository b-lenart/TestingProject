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
    [TestFixture]
    public class HomeControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DoesReturnProperMessageInAbout()
        {
            HomeController homeController = new HomeController();

            ViewResult result = homeController.About() as ViewResult;

            Assert.AreEqual("Your application description page.", result.ViewData["Message"]);
        }

        [Test]
        public void DoesReturnProperMessageInContact()
        {
            HomeController homeController = new HomeController();

            ViewResult result = homeController.Contact() as ViewResult;

            Assert.AreEqual("Your contact page.", result.ViewData["Message"]);
        }

        [Test]
        public void HomeControllerTest()
        {
            HomeController homeController = new HomeController();
            var result = homeController.Index();
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
        }

        [Test]
        public void HomeControllerErrorTest()
        {
            HomeController homeController = new HomeController();
            //var result = homeController.Error();
            var result = homeController.Error() as ViewResult;
            Assert.IsInstanceOf(typeof(ErrorViewModel), result.Model);
        }
    }
}