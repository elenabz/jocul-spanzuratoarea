using JoculSpanzuratoarea.DTO;
using JoculSpanzuratoarea.Interfaces;
using JoculSpanzuratoarea.Pages;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Spanzuratoarea.Tests
{
    public class IndexPageTests
    {
        private readonly Mock<IJoculSpanzuratoareaRepository> _joculSpanzuratoareaRepository;
        public IndexPageTests()
        {
            _joculSpanzuratoareaRepository = new Mock<IJoculSpanzuratoareaRepository>();
        }

        [Fact]
        public void OnPost_RedirectToIndex()
        {
            var model = Mock.Of<WordWithDefinition>(x => x.Word == "software" && x.Definition == "Set de informatii necesare functionarii unui computer...");
            _joculSpanzuratoareaRepository.Setup(x => x.GetRandomWord()).Returns(model);
            var indexPage = new IndexPage(_joculSpanzuratoareaRepository.Object);

            var result = indexPage.OnPost();

            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}