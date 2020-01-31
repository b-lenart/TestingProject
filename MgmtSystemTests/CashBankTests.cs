using coderush.Controllers.Api;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MgmtSystemTests
{
    public class CashBankTests : IDisposable
    {
        protected readonly ApplicationDbContext _context;
        protected readonly CashBankController controller;

        public CashBankTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();

            var cashBankList = new[]
                {
                    new CashBank() { CashBankId = 1, CashBankName = "Cash Bank USA", Description = "abcd" },
                    new CashBank() { CashBankId = 2, CashBankName = "Cash Bank Mexico", Description = "axcd" },
                    new CashBank() { CashBankId = 3, CashBankName = "Cash Bank Poland", Description = "bvcx" },
                    new CashBank() { CashBankId = 4, CashBankName = "Cash Bank UK", Description = "sdfg" },
                    new CashBank() { CashBankId = 5, CashBankName = "Cash Bank Australia", Description = "yxqw" }
                };

            _context.CashBank.AddRange(cashBankList);

            _context.SaveChanges();

            controller = new CashBankController(_context);
        }

        [Fact]
        public async Task GetCashBankReturnOkObjectResult()
        {
            var result = await controller.GetCashBank();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCashBankProperAmount()
        {
            var okResult = controller.GetCashBank().Result as OkObjectResult;

            var items = Assert.IsType<List<CashBank>>(okResult.Value);

            Assert.Equal(5, items.Count);
        }

        [Fact]
        public void PostValidCashBank()
        {
            CashBank newCashBank = new CashBank() { CashBankId = 6, CashBankName = "Cash Bank Switzerland", Description = "swiss" };
            CrudViewModel<CashBank> crudView = new CrudViewModel<CashBank>()
            {
                action = "insert",
                antiForgery = "",
                key = null,
                value = newCashBank
            };

            var result = controller.Insert(crudView);
            Assert.IsType<OkObjectResult>(result);

            var resultValue = result as OkObjectResult;
            var cashBankFromResponse = resultValue.Value as CashBank;

            Assert.Equal(newCashBank.CashBankName, cashBankFromResponse.CashBankName);
        }

        [Fact]
        public void UpdateInvalidCashBank()
        {
            CashBank cashBankToUpdate = new CashBank() { CashBankId = 7, CashBankName = "Cash Bank Greece", Description = "axcd" };
            CrudViewModel<CashBank> crudView = new CrudViewModel<CashBank>()
            {
                action = "update",
                antiForgery = "",
                key = 7,
                value = cashBankToUpdate
            };

            var exception = Assert.Throws<DbUpdateConcurrencyException>(() => controller.Update(crudView));
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }

        [Fact]
        public void RemoveCashBank()
        {
            CashBank cashBankToRemove = new CashBank() { CashBankId = 3, CashBankName = "Cash Bank Poland" };
            CrudViewModel<CashBank> crudView = new CrudViewModel<CashBank>()
            {
                action = "remove",
                antiForgery = "",
                key = 3,
                value = cashBankToRemove
            };

            var result = controller.Remove(crudView);
            Assert.IsType<OkObjectResult>(result);

            var resultValue = result as OkObjectResult;
            var cashBankFromResponse = resultValue.Value as CashBank;

            Assert.Equal(cashBankToRemove.CashBankName, cashBankFromResponse.CashBankName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
