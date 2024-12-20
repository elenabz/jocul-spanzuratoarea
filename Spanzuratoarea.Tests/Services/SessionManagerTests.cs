﻿using JoculSpanzuratoarea.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Spanzuratoarea.Tests.Services
{
    public class SessionManagerTests
    {
        private readonly SessionManager _sessionManager;
        private readonly MockHttpSession _httpSession;

        public SessionManagerTests()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            _httpSession = new MockHttpSession();
            httpContextAccessor.Setup(s => s.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(s => s.Session).Returns(_httpSession);
            _sessionManager = new SessionManager(httpContextAccessor.Object);
        }

        [Fact]
        public void DeleteSessionItems()
        {
            _sessionManager.DeleteSessionItems();
            var storage = _httpSession._sessionStorage;
            Assert.Empty(storage);
        }

        [Fact]
        public void SetWord_GivenString()
        {
            string word = "spanzuratoarea";
            _sessionManager.SetWord(word);
            var result = _sessionManager.GetWord();
            Assert.Equal(word, result);
        }

        [Fact]
        public void SetGuessedFullWord()
        {
            int guessed = 1;
            _sessionManager.SetGuessedFullWord();
            var result = _sessionManager.GetGuessedFullWord();
            Assert.Equal(guessed, result);
        }

        [Fact]
        public void SetWordDefinition()
        {
            string definition = "word definition";
            _sessionManager.SetWordDefinition(definition);
            var result = _sessionManager.GetWordDefinition();
            Assert.Equal(definition, result);
        }

        [Fact]
        public void SetMaskedWord()
        {
            string maskedWord = "____a";
            _sessionManager.SetMaskedWord(maskedWord);
            var result = _sessionManager.GetMaskedWord();
            Assert.Equal(maskedWord, result);
        }

        [Fact]
        public void IncrementFailCount()
        {
            int count = 1;
            _sessionManager.IncrementFailCount();
            var result = _sessionManager.GetFailCount();
            Assert.Equal(count, result);
        }
    }
}
