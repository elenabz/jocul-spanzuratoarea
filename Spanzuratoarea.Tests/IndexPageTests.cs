using JoculSpanzuratoarea.Interfaces;
using JoculSpanzuratoarea.Models;
using JoculSpanzuratoarea.Pages;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Spanzuratoarea.Tests
{
    public class IndexPageTests
    {
        private readonly Mock<ISessionManager> _sessionManager;
        private readonly Mock<IGameLogic> _gameLogic;
        public IndexPageTests()
        {
            _sessionManager = new Mock<ISessionManager>();
            _gameLogic = new Mock<IGameLogic>();
        }

        [Fact]
        public void OnPost_RedirectToIndex()
        {
            var indexPage = new IndexPage(_gameLogic.Object, _sessionManager.Object);

            var result = indexPage.OnPost();

            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public void OnPostLetterClick_GivenALetter_ReturnsJsonResult()
        {
            var indexPage = new IndexPage(_gameLogic.Object, _sessionManager.Object);

            var letterData = Mock.Of<LetterPostData>(x => x.Letter == 'a');

            var result = indexPage.OnPostLetterClick(letterData);

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnPostLetterClick_GivenNoLetter_ReturnsJsonResult()
        {
            var indexPage = new IndexPage(_gameLogic.Object, _sessionManager.Object);

            var letterData = Mock.Of<LetterPostData>();

            var result = indexPage.OnPostLetterClick(letterData);

            Assert.IsType<JsonResult>(result);
        }
    }
}