using coderush.Data;
using coderush.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using coderush.Services;

namespace MgmtSystemTests
{
    public class NumberSequenceTests : IDisposable
    {
        protected readonly ApplicationDbContext _context;

        public NumberSequenceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();

            var sequences = new[]
                {
                    new coderush.Models.NumberSequence() { NumberSequenceId = 10, NumberSequenceName = "BILL", Module = "BILL", Prefix = "BILL", LastNumber = 3 },
                    new coderush.Models.NumberSequence() { NumberSequenceId = 11, NumberSequenceName = "RANDOM", Module = "RANDOM", Prefix = "RANDOM", LastNumber = 4 },
                    new coderush.Models.NumberSequence() { NumberSequenceId = 12, NumberSequenceName = "BILL", Module = "BILL", Prefix = "BILL", LastNumber = 5 }
                };

            _context.NumberSequence.AddRange(sequences);

            _context.SaveChanges();
        }

        [Fact]
        public void CreateNumberSequenceWithExistingModule()
        {
            var sequence = new coderush.Services.NumberSequence(_context);

            var result = sequence.GetNumberSequence("BILL");

            Assert.Equal("00004#BILL", result);
        }

        [Fact]
        public void TestNumberSequenceReturnType()
        {
            var sequence = new coderush.Services.NumberSequence(_context);

            var result = sequence.GetNumberSequence("BILL");

            Assert.IsType<string>(result);
        }

        [Fact]
        public void CreateNumberSequenceWithNotExistingdModule()
        {
            var sequence = new coderush.Services.NumberSequence(_context);

            var result = sequence.GetNumberSequence("app");

            Assert.Equal("00001#app", result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
