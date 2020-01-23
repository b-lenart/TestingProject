﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MgmtSystemTests
{
    [TestFixture]
    class SeleniumTests
    {
        IWebDriver _driver;

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void GoogleTest()
        {
            var login = "super@admin.com";
            var password = "123456";
            _driver.Navigate().GoToUrl("https://localhost:44308/");
            var loginInput = _driver.FindElement(By.CssSelector(".login-box-body input#Email"));
            var passwordInput = _driver.FindElement(By.CssSelector(".login-box-body input#Password"));
            var loginButton = _driver.FindElement(By.CssSelector(".login-box-body button.btn.btn-primary.btn-block.btn-flat"));

            loginInput.Clear();
            loginInput.SendKeys(login);
            passwordInput.Clear();
            passwordInput.SendKeys(password);
            loginButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists((By.CssSelector(".user-menu span.hidden-xs"))));

            var userMailElement = _driver.FindElement(By.CssSelector(".user-menu span.hidden-xs"));
            StringAssert.Contains(login, userMailElement.Text);
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

    }
}
