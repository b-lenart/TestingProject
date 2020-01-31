using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MgmtSystemTests
{
    [TestFixture]
    class ChangePasswordSeleniumTest
    {
        IWebDriver _driver;
        string login;

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver();

            login = "super@admin.com";
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
                .ElementExists(By.CssSelector(".user-menu span.hidden-xs")));
        }

        [Test]
        public void ChangePasswordLinkGoesToProperUrl()
        {
            _driver.Navigate().GoToUrl("https://localhost:44308/UserRole/ChangePassword");

            var changePasswordLink = _driver.FindElement(By.CssSelector(".sidebar-menu.tree li:nth-child(34)"));
            Assert.IsTrue(changePasswordLink.GetAttribute("class") == "active");
            StringAssert.StartsWith("https://localhost:44308/UserRole/ChangePassword", _driver.Url);
        }

        [Test]
        public void IsAlertVisibleAfterClickWithNoTabActive()
        {
            _driver.Navigate().GoToUrl("https://localhost:44308/UserRole/ChangePassword");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit")));

            var editButton = _driver.FindElement(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit"));
            editButton.Click();

            System.Threading.Thread.Sleep(2000);
            var alert = _driver.SwitchTo().Alert();
            var alertText = alert.Text;
            alert.Accept();
            Assert.IsTrue(alertText == "No records selected for edit operation");
        }

        [Test]
        public void ChangePasswordProcess()
        {
            _driver.Navigate().GoToUrl("https://localhost:44308/UserRole/ChangePassword");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit")));

            var firstrowemail = _driver.FindElement(By.XPath("//tr[contains(.,'" + login + "')]"));
            firstrowemail.Click();

            var editButton = _driver.FindElement(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit"));
            editButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("input#Email")));

            var userNameInput = _driver.FindElement(By.CssSelector("input#Email"));
            var saveButton = _driver.FindElement(By.CssSelector("input#EditDialog_Grid_Save"));

            saveButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
            .ElementExists(By.CssSelector(".e-field-validation-error")));

            var oldPasswordValidationError = _driver.FindElement(By.CssSelector(".e-field-validation-error[for=\"OldPassword_hidden\"]"));
            var newPasswordValidationError = _driver.FindElement(By.CssSelector(".e-field-validation-error[for=\"Password_hidden\"]"));
            var retypePasswordValidationError = _driver.FindElement(By.CssSelector(".e-field-validation-error[for=\"ConfirmPassword_hidden\"]"));

            StringAssert.Contains(login, userNameInput.GetAttribute("value"));
            StringAssert.Contains("This field is required.", oldPasswordValidationError.Text);
            StringAssert.Contains("Password is required.", newPasswordValidationError.Text);
            StringAssert.Contains("Confirm Password is required.", retypePasswordValidationError.Text);

            string password = "123456";

            var oldPasswordInput = _driver.FindElement(By.CssSelector("input#OldPassword"));
            var newPasswordInput = _driver.FindElement(By.CssSelector("input#Password"));
            var retypePasswordInput = _driver.FindElement(By.CssSelector("input#ConfirmPassword"));

            oldPasswordInput.SendKeys(password);
            newPasswordInput.SendKeys(password);
            retypePasswordInput.SendKeys(password);

            saveButton.Click();

            var topRightMailElement = _driver.FindElement(By.CssSelector("li.dropdown.user.user-menu a.dropdown-toggle"));
            topRightMailElement.Click();
            var signOutButton = _driver.FindElement(By.CssSelector("form#logoutForm div.pull-right button.btn.btn-default.btn-flat"));
            signOutButton.Click();

            StartBrowser();

            var userMailElement = _driver.FindElement(By.CssSelector(".user-menu span.hidden-xs"));
            StringAssert.Contains("super@admin.com", userMailElement.Text);
            Assert.IsTrue("https://localhost:44308/" == _driver.Url);

        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

    }
}
