using JoculSpanzuratoarea.Data;
using JoculSpanzuratoarea.DTO;
using JoculSpanzuratoarea.Interfaces;
using JoculSpanzuratoarea.Pages;
using JoculSpanzuratoarea.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Spanzuratoarea.Tests
{
    public class JoculSpanzuratoareaRepositoryTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            // todo: add data to db + modify random number
            return databaseContext;
        }

        [Fact]
        public void GetRandomWord_ReturnsWordWithDefinition()
        {
            var databaseContext = GetDbContext();
            var joculSpanzuratoareaRepository = new JoculSpanzuratoareaRepository(databaseContext);

            var result = joculSpanzuratoareaRepository.GetRandomWord();

            Assert.IsType<WordWithDefinition>(result);
        }

    }
}
