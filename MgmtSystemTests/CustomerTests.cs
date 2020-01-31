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
    public class CustomerTests : IDisposable
    {
        protected readonly ApplicationDbContext _context;

        public CustomerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();

            var customers = new[]
                {
                    new Customer() { CustomerId = 1, CustomerName = "John One" },
                    new Customer() { CustomerId = 2, CustomerName = "John Two" },
                    new Customer() { CustomerId = 3, CustomerName = "John Three" }
                };

            _context.Customer.AddRange(customers);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCustomersReturnOkObjectResult()
        {
            var controller = new CustomerController(_context);

            var result = await controller.GetCustomer();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomersProperAmount()
        {
            var controller = new CustomerController(_context);
            var okResult = controller.GetCustomer().Result as OkObjectResult;

            var items = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void PostValidCustomer()
        {
            var controller = new CustomerController(_context);

            Customer newCustomer = new Customer() { CustomerId = 4, CustomerName = "John New" };
            CrudViewModel<Customer> crudView = new CrudViewModel<Customer>()
            {
                action = "insert",
                antiForgery = "",
                key = null,
                value = newCustomer
            };

            var result = controller.Insert(crudView);
            Assert.IsType<OkObjectResult>(result);

            var resultValue = result as OkObjectResult;
            var customerFromResponse = resultValue.Value as Customer;

            Assert.Equal(newCustomer.CustomerName, customerFromResponse.CustomerName);
        }

        [Fact]
        public void PostInvalidCustomerReturnsArgumentNullException()
        {
            var controller = new CustomerController(_context);

            Customer newCustomer = new Customer() { CustomerId = 4, CustomerName = null };
            CrudViewModel<Customer> crudView = new CrudViewModel<Customer>()
            {
                action = "insert",
                antiForgery = "",
                key = null,
                value = newCustomer
            };

            var exception = Assert.Throws<ArgumentNullException>(() => controller.Insert(crudView));
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Customer name cannot be null", exception.Message);
        }

        [Fact]
        public void UpdateInvalidCustomer()
        {
            var controller = new CustomerController(_context);

            Customer newCustomer = new Customer() { CustomerId = 50, CustomerName = "John Invalid" };
            CrudViewModel<Customer> crudView = new CrudViewModel<Customer>()
            {
                action = "update",
                antiForgery = "",
                key = 50,
                value = newCustomer
            };

            var exception = Assert.Throws<DbUpdateConcurrencyException>(() => controller.Update(crudView));
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }

        [Fact]
        public void UpdateCustomer()
        {
            var controller = new CustomerController(_context);

            Customer customerToUpdate = new Customer() { CustomerId = 2, CustomerName = "John Updated" };
            CrudViewModel<Customer> crudView = new CrudViewModel<Customer>()
            {
                action = "update",
                antiForgery = "",
                key = 2,
                value = customerToUpdate
            };

            var result = controller.Update(crudView);
            Assert.IsType<OkObjectResult>(result);

            var resultValue = result as OkObjectResult;
            var customerFromResponse = resultValue.Value as Customer;

            Assert.Equal(customerToUpdate.CustomerName, customerFromResponse.CustomerName);
        }

        [Fact]
        public void RemoveCustomer()
        {
            var controller = new CustomerController(_context);

            Customer customerToRemove = new Customer() { CustomerId = 2, CustomerName = "John Two" };
            CrudViewModel<Customer> crudView = new CrudViewModel<Customer>()
            {
                action = "remove",
                antiForgery = "",
                key = 2,
                value = customerToRemove
            };

            var result = controller.Remove(crudView);
            Assert.IsType<OkObjectResult>(result);

            var resultValue = result as OkObjectResult;
            var customerFromResponse = resultValue.Value as Customer;

            Assert.Equal(customerToRemove.CustomerName, customerFromResponse.CustomerName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
