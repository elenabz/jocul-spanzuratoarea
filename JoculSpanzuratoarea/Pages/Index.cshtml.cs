using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using JoculSpanzuratoarea.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace JoculSpanzuratoarea.Pages
{

    // session variables
    public enum SessionKeyEnum
    {
        SessionKeyWord,
        SessionKeyWordDefinition,
        SessionKeyMaskedWord,
        SessionKeyFailCount,
        SessionKeyGuessedFullWord
    }
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // proper ties from PageModel will be accessed from page through @model that's why we don't need to return anything from OnGet()
        // the page (.cshtml) has access to properties from PageModel (.cshtml.cs)
        // private can't be accessed from razor page
        private Entry EntryWord { get; set; } = new Entry();
        private Definition DefinitionOfEntry { get; set; } = new Definition();

        public List<char> alphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ".ToList();
        public string maskedWordToGuess = "";

        public readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
            GetRandomWordFromDex();

        }

        public void GetRandomWordFromDex()
        {
            Random random = new Random();
            int id = random.Next(3, 323319);

            // ======== get the Definition using include
            Entry entryWithDefinition = _context.Entries
                .Include(entry => entry.EntryDefinitions)
                .ThenInclude(entryDefinition => entryDefinition.Definition)
                .Where(e => e.Id >= id)
                .Where(e => e.Usable == true)
                .First();

            // joins 2 tables
            var wordFromDexWithDefinition = _context.Entries
                .Join(_context.EntryDefinitions, e => e.Id, ed => ed.EntryId, (e, ed) => new { EntryDefinition = ed, Entry = e })
                .Join(_context.Definitions, ed => ed.EntryDefinition.DefinitionId, d => d.Id, (ed, d) => new { EntryDefinition = ed, Definition = d })
                .Where(e => e.EntryDefinition.Entry.Id >= id).Where(e => e.EntryDefinition.Entry.Usable == true).First();

            EntryWord = wordFromDexWithDefinition.EntryDefinition.Entry;
            DefinitionOfEntry = wordFromDexWithDefinition.Definition;

            bool wordContainsPunctuation = EntryWord.Description.IndexOfAny(new char[] { '(', '/' }) != -1;
            if (wordContainsPunctuation)
            {
                EntryWord.Description = CleanWord(EntryWord.Description);
            }
            return;
        }

        public string CleanWord(string word)
        {
            int indexOfNonLetter = word.IndexOfAny(new char[] { '(', '/' });
            return word.Substring(0, indexOfNonLetter).Trim();
        }
        public void OnGet()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            HttpContext.Session.Remove(SessionKeyEnum.SessionKeyWord.ToString());
            HttpContext.Session.Remove(SessionKeyEnum.SessionKeyWordDefinition.ToString());
            HttpContext.Session.Remove(SessionKeyEnum.SessionKeyMaskedWord.ToString());
            HttpContext.Session.Remove(SessionKeyEnum.SessionKeyFailCount.ToString());
            HttpContext.Session.Remove(SessionKeyEnum.SessionKeyGuessedFullWord.ToString());
            SetWord();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");

        }
        public JsonResult OnPostLetterClick([FromBody] LetterPostData letterData)
        {
            string currentLetter = letterData.Letter.ToLower();
            var responseData = ComputeGameState(currentLetter);
            return new JsonResult(responseData);
        }

        private void SetWord()
        {
            string word = EntryWord.Description.ToLower();
            string wordDefinition = DefinitionOfEntry.InternalRep ?? "";
            HttpContext.Session.SetString(SessionKeyEnum.SessionKeyWord.ToString(), word);
            HttpContext.Session.SetString(SessionKeyEnum.SessionKeyWordDefinition.ToString(), wordDefinition);
            string maskedWord = "";
            for (int i = 0; i < word.Length; i++)
            {
                maskedWord += "_";

            }
            SetMaskedWord(maskedWord);
            maskedWordToGuess = maskedWord;
            return;
        }

        private void SetMaskedWord(string maskedWord)
        {
            HttpContext.Session.SetString(SessionKeyEnum.SessionKeyMaskedWord.ToString(), maskedWord);
            return;
        }
        private void IncrementFailCount()
        {
            int sessionFailCount = (HttpContext.Session.GetInt32(SessionKeyEnum.SessionKeyFailCount.ToString()) ?? 0) + 1;
            HttpContext.Session.SetInt32(SessionKeyEnum.SessionKeyFailCount.ToString(), sessionFailCount);
            return;
        }
        private void SetGuessedFullWord()
        {
            HttpContext.Session.SetInt32(SessionKeyEnum.SessionKeyGuessedFullWord.ToString(), 1);
            return;
        }
        private int GetGuessedFullWord()
        {
            int sessionGuessedFullWord = HttpContext.Session.GetInt32(SessionKeyEnum.SessionKeyGuessedFullWord.ToString()) ?? 0;
            return sessionGuessedFullWord;
        }

        public string GetSessionWord()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyEnum.SessionKeyWord.ToString())))
            {
                SetWord();
            }

            string sessionWord = HttpContext.Session.GetString(SessionKeyEnum.SessionKeyWord.ToString()) ?? "";
            return sessionWord;
        }

        public string GetSessionMaskedWord()
        {
            string sessionMaskedWord = HttpContext.Session.GetString(SessionKeyEnum.SessionKeyMaskedWord.ToString()) ?? "";
            return sessionMaskedWord;
        }

        public int GetSessionFailCount()
        {
            int sessionFailCount = HttpContext.Session.GetInt32(SessionKeyEnum.SessionKeyFailCount.ToString()) ?? 0;
            return sessionFailCount;
        }

        private GameState ComputeGameState(string playLetter)
        {
            string word = GetSessionWord();
            string maskedWord = GetSessionMaskedWord();
            GameState responseData = new GameState();
            List<int> letterMatches = new List<int>();
            if (GetSessionFailCount() < 6)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (playLetter == word[i].ToString())
                    {
                        letterMatches.Add(i);
                    }
                }
                for (int i = 0; i < letterMatches.Count; i++)
                {
                    int pos = letterMatches[i];
                    maskedWord = maskedWord.Remove(pos, 1).Insert(pos, playLetter);
                }
                if (letterMatches.Count > 0)
                {
                    responseData.GuessedLetter = true;
                }
                else
                {
                    IncrementFailCount();
                }
            }
            SetMaskedWord(maskedWord);

            if (!maskedWord.Contains('_'))
            {
                SetGuessedFullWord();
                responseData.Word = word;
                responseData.WordDefinition = HttpContext.Session.GetString(SessionKeyEnum.SessionKeyWordDefinition.ToString()) ?? "";
            }
            responseData.MaskedWordToGuess = maskedWord;
            responseData.GuessedFullWord = GetGuessedFullWord();
            responseData.FailCount = GetSessionFailCount();
            return responseData;
        }
    }
}