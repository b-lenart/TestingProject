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

            _driver.Navigate().GoToUrl("https://localhost:44308/UserRole/ChangePassword");

            //var changePasswordLink = _driver.FindElement(By.XPath("//a[contains(@href, '/UserRole/ChangePassword')]"));
            //new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
            //    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
            //    .ElementExists(By.CssSelector(".sidebar-menu.tree li:nth-child(34) a")));
            //var changePasswordLink = _driver.FindElement(By.CssSelector(".sidebar-menu.tree li:nth-child(34) a"));

            //var changePasswordLink = element.FindElement(By.XPath("//a[contains(@href, '/UserRole/ChangePassword')]"));
            //changePasswordLink.Click();

        }

        [Test]
        public void ChangePasswordLinkGoesToProperUrl()
        {
            var changePasswordLink = _driver.FindElement(By.CssSelector(".sidebar-menu.tree li:nth-child(34)"));
            Assert.IsTrue(changePasswordLink.GetAttribute("class") == "active");
            StringAssert.StartsWith("https://localhost:44308/UserRole/ChangePassword", _driver.Url);
        }

        [Test]
        public void IsAlertVisibleAfterClickWithNoTabActive()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit")));

            var editButton = _driver.FindElement(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit"));
            editButton.Click();

            //IAlert alert = driver.SwitchTo().Alert();
            System.Threading.Thread.Sleep(2000);
            var alert = _driver.SwitchTo().Alert();
            var alertText = alert.Text;
            alert.Accept();
            Assert.IsTrue(alertText == "No records selected for edit operation");
        }

        [Test]
        public void ChangePasswordProcess()
        {
            //_driver.switchTo().alert()
            //var login = "super@admin.com";

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit")));

            //var firstRow = _driver.FindElement(By.CssSelector("#Grid .e-gridcontent table.e-table tr.e-row"));
            //var firstrowemail = _driver.FindElement(By.XPath("//tr[text() = 'super@admin.com']"));
            var firstrowemail = _driver.FindElement(By.XPath("//tr[contains(.,'" + login + "')]"));
            firstrowemail.Click();

            var editButton = _driver.FindElement(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit"));
            editButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("input#Email")));

            var userNameInput = _driver.FindElement(By.CssSelector("input#Email"));
            var oldPassword = _driver.FindElement(By.CssSelector("input#OldPassword"));
            var saveButton = _driver.FindElement(By.CssSelector("input#EditDialog_Grid_Save"));
            //StringAssert.Contains(firstCustomerName.Text, cutomerNameOnEditForm.GetAttribute("value"));

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

            //var addButton = _driver.FindElement(By.CssSelector("a.e-addnewitem.e-toolbaricons.e-icon.e-addnew"));
            //var nameInput = _driver.FindElement(By.CssSelector("input#GridCustomerName"));
            //var saveButton = _driver.FindElement(By.CssSelector("input#EditDialog_Grid_Save"));

            //Random random = new Random();
            //int randomNumber = random.Next(1000);
            //nameInput.SendKeys("Customer" + randomNumber);

            //saveButton.Click();

            //new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
            //    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
            //    .ElementExists(By.CssSelector("table.e-table tbody tr:first-child td:nth-child(2).e-selectionbackground")));
            //var customerName = _driver.FindElement(By.CssSelector("table.e-table tbody tr:first-child td:nth-child(2).e-selectionbackground"));
            //StringAssert.Contains("Customer" + randomNumber, customerName.Text);
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

    }
}
