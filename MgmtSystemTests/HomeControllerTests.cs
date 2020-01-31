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
        public HomeController homeController;

        [SetUp]
        public void Setup()
        {
            homeController = new HomeController();
        }

        [Test]
        public void DoesReturnProperMessageInAbout()
        {
            ViewResult result = homeController.About() as ViewResult;

            Assert.AreEqual("Your application description page.", result.ViewData["Message"]);
        }

        [Test]
        public void DoesReturnProperMessageInContact()
        {
            ViewResult result = homeController.Contact() as ViewResult;

            Assert.AreEqual("Your contact page.", result.ViewData["Message"]);
        }

        [Test]
        public void HomeControllerTest()
        {
            var result = homeController.Index();
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
        }

        [Test]
        public void HomeControllerErrorTest()
        {
            ViewResult result = homeController.Error() as ViewResult;
            Assert.IsInstanceOf(typeof(ErrorViewModel), result.Model);
        }
    }
}