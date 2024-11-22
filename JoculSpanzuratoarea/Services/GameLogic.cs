using JoculSpanzuratoarea.DTO;
using JoculSpanzuratoarea.Interfaces;
using JoculSpanzuratoarea.Models;

namespace JoculSpanzuratoarea.Services
{
    public class GameLogic: IGameLogic
    {

        private readonly IJoculSpanzuratoareaRepository _joculSpanzuratoareaRepository;
        private readonly ISessionManager _sessionManager;
        public GameLogic(IJoculSpanzuratoareaRepository joculSpanzuratoareaRepository, ISessionManager sessionManager)
        {
            _joculSpanzuratoareaRepository = joculSpanzuratoareaRepository;
            _sessionManager = sessionManager;
        }

        private void SetGameItems()
        {
            WordWithDefinition wordWithDefinition = _joculSpanzuratoareaRepository.GetRandomWord();
            _sessionManager.SetWord(wordWithDefinition.Word);
            _sessionManager.SetWordDefinition(wordWithDefinition.Definition);

            string maskedWord = "";
            for (int i = 0; i < wordWithDefinition.Word.Length; i++)
            {
                maskedWord += "_";

            }
            _sessionManager.SetMaskedWord(maskedWord);
        }

        public void StartGame()
        {
            _sessionManager.DeleteSessionItems();
            SetGameItems();
        }

        public GameState ComputeGameState(char playLetter, int maxGuesses)
        {
            string word = _sessionManager.GetWord();
            string maskedWord = _sessionManager.GetMaskedWord();
            GameState responseData = new();
            List<int> letterMatches = new();
            if (_sessionManager.GetFailCount() < maxGuesses)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (playLetter == word[i])
                    {
                        letterMatches.Add(i);
                    }
                }
                for (int i = 0; i < letterMatches.Count; i++)
                {
                    int pos = letterMatches[i];
                    maskedWord = maskedWord.Remove(pos, 1).Insert(pos, playLetter.ToString());
                }
                if (letterMatches.Count > 0)
                {
                    responseData.GuessedLetter = true;
                }
                else
                {
                    _sessionManager.IncrementFailCount();
                }
            }
            _sessionManager.SetMaskedWord(maskedWord);

            if (!maskedWord.Contains('_'))
            {
                _sessionManager.SetGuessedFullWord();
                responseData.Word = word;
                responseData.WordDefinition = _sessionManager.GetWordDefinition();
            }
            responseData.MaskedWordToGuess = maskedWord;
            responseData.GuessedFullWord = _sessionManager.GetGuessedFullWord();
            responseData.FailCount = _sessionManager.GetFailCount();
            return responseData;
        }
    }
}
