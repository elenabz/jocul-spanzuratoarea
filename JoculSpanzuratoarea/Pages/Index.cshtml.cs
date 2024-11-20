using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using JoculSpanzuratoarea.Services;
using JoculSpanzuratoarea.Interfaces;

namespace JoculSpanzuratoarea.Pages
{
    public class IndexPage : PageModel
    {
        public List<char> alphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ".ToList();
        public string maskedWordToGuess = "";
        public const int MAX_GUESSES = 6;

        private readonly IGameLogic _gameLogic;
        private readonly ISessionManager _sessionManager;

        public IndexPage(IGameLogic gameLogic, ISessionManager sessionManager)
        {
            _gameLogic = gameLogic;
            _sessionManager = sessionManager;
        }

        public void OnGet()
        {
            _gameLogic.StartGame();
            maskedWordToGuess = _sessionManager.GetMaskedWord();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");

        }

        public JsonResult OnPostLetterClick([FromBody] LetterPostData letterData)
        {
            string currentLetter = letterData.Letter.ToLower();
            var responseData = _gameLogic.ComputeGameState(currentLetter, MAX_GUESSES);
            return new JsonResult(responseData);
        }
    }
}