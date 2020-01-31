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
    class CustomerSeleniumTests
    {
        IWebDriver _driver;

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver();

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
                .ElementExists(By.CssSelector(".user-menu span.hidden-xs")));

            _driver.Navigate().GoToUrl("https://localhost:44308/Customer/Index");

        }

        [Test]
        public void AddNewCustomer()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-addnewitem.e-toolbaricons.e-icon.e-addnew")));

            var addButton = _driver.FindElement(By.CssSelector("a.e-addnewitem.e-toolbaricons.e-icon.e-addnew"));
            addButton.Click();
            var nameInput = _driver.FindElement(By.CssSelector("input#GridCustomerName"));
            var saveButton = _driver.FindElement(By.CssSelector("input#EditDialog_Grid_Save"));

            Random random = new Random();
            int randomNumber = random.Next(1000);
            nameInput.SendKeys("Customer" + randomNumber);

            saveButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("table.e-table tbody tr:first-child td:nth-child(2).e-selectionbackground")));
            var customerName = _driver.FindElement(By.CssSelector("table.e-table tbody tr:first-child td:nth-child(2).e-selectionbackground"));
            StringAssert.Contains("Customer" + randomNumber, customerName.Text);
        }

        [Test]
        public void TryToAddCustomerWithNoName()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-addnewitem.e-toolbaricons.e-icon.e-addnew")));

            var addButton = _driver.FindElement(By.CssSelector("a.e-addnewitem.e-toolbaricons.e-icon.e-addnew"));
            addButton.Click();
            var saveButton = _driver.FindElement(By.CssSelector("input#EditDialog_Grid_Save"));

            saveButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("#GridEditForm td .e-error .e-field-validation-error")));
            var validationErrorPopUp = _driver.FindElement(By.CssSelector("#GridEditForm td .e-error .e-field-validation-error"));
            StringAssert.Contains("Customer Name is required.", validationErrorPopUp.Text);
        }

        [Test]
        public void EditCustomer()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit")));

            var firstRow = _driver.FindElement(By.CssSelector("#Grid .e-gridcontent table.e-table tr.e-row"));
            var firstCustomerName = _driver.FindElement(By.CssSelector("#Grid .e-gridcontent table.e-table tr.e-row td:nth-child(2)"));
            var editButton = _driver.FindElement(By.CssSelector("a.e-edititem.e-toolbaricons.e-icon.e-edit"));

            firstRow.Click();
            editButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                .ElementExists(By.CssSelector("input#GridCustomerName")));

            var cutomerNameOnEditForm = _driver.FindElement(By.CssSelector("input#GridCustomerName"));
            StringAssert.Contains(firstCustomerName.Text, cutomerNameOnEditForm.GetAttribute("value"));
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

    }
}
