using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using JoculSpanzuratoarea.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace JoculSpanzuratoarea.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Entry EntryWord { get; set; } = new Entry();
        public Definition DefinitionOfEntry { get; set; } = new Definition();

        public const string SessionKeyWord = "_Word";
        public const string SessionKeyMaskedWord = "_MaskedWord";
        public const string SessionKeyFailCount = "_FailCount";
        public const string SessionKeyGuessedFullWord = "_GuessedFullWord";

        public List<char> alphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ".ToList();
        public List<string> words = new List<string>() { "first", "second", "third" };
        public string wordToGuess = "";
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

            Entry entryWithDefinition = _context.Entries.Include(entry => entry.EntryDefinitions).ThenInclude(entryDefinition => entryDefinition.Definition).Where(e => e.Id >= id).Where(e => e.Usable == true).First();
            Console.WriteLine(entryWithDefinition.EntryDefinitions.First().Definition.InternalRep);


            // joins 2 tables 

            var wordFromDexWithDefinition = _context.Entries
                .Join(_context.EntryDefinitions, e => e.Id, ed => ed.EntryId, (e, ed) => new { EntryDefinition = ed, Entry = e })
                .Join(_context.Definitions, ed => ed.EntryDefinition.DefinitionId, d => d.Id, (ed, d) => new { EntryDefinition = ed, Definition = d })
                .Where(e => e.EntryDefinition.Entry.Id >= id).Where(e => e.EntryDefinition.Entry.Usable == true).First();
            EntryWord = wordFromDexWithDefinition.EntryDefinition.Entry;
            DefinitionOfEntry = wordFromDexWithDefinition.Definition;
            DefinitionOfEntry.InternalRep = CleanDefinition(DefinitionOfEntry.InternalRep);
            //Console.WriteLine(wordFromDexWithDefinition.Definition.Lexicon);


            // ========= get only the word

            // Entry wordFromDex = _context.Entries.Where(w => w.Id >= id).Where(w => w.Usable == true).First();
            // bool wordContainsPunctuation = wordFromDex.Description.IndexOfAny(new char[] { '(', '/' }) != -1;
            bool wordContainsPunctuation = EntryWord.Description.IndexOfAny(new char[] { '(', '/' }) != -1;
            if (wordContainsPunctuation)
            {
                //wordFromDex.Description = CleanWord(wordFromDex.Description);
                EntryWord.Description = CleanWord(EntryWord.Description);
            }
            //EntryWord = wordFromDex;
            return;
        }

        public string CleanDefinition(string definition)
        {
            return Regex.Replace(definition, "[$@#[].*[]#$@]", "");
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
            HttpContext.Session.Remove(SessionKeyWord);
            HttpContext.Session.Remove(SessionKeyMaskedWord);
            HttpContext.Session.Remove(SessionKeyFailCount);
            HttpContext.Session.Remove(SessionKeyGuessedFullWord);
            SetWord();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Index");

        }
        public JsonResult OnPostLetterClick([FromBody] LetterPostData letterData)
        {
            string currentLetter = letterData.Letter.ToLower();
            var responseData = ComputeGameState(currentLetter);
            return new JsonResult(responseData);
        }

        private void SetWord()
        {
            int wordIdx = (new Random()).Next(words.Count);
            //string word = words[wordIdx].ToLower();
            string word = EntryWord.Description.ToLower();
            HttpContext.Session.SetString(SessionKeyWord, word);
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
            HttpContext.Session.SetString(SessionKeyMaskedWord, maskedWord);
            return;
        }
        private void IncrementFailCount()
        {
            int sessionFailCount = (HttpContext.Session.GetInt32(SessionKeyFailCount) ?? 0) + 1;
            HttpContext.Session.SetInt32(SessionKeyFailCount, sessionFailCount);
            return;
        }
        private void SetGuessedFullWord()
        {
            HttpContext.Session.SetInt32(SessionKeyGuessedFullWord, 1);
            return;
        }
        private int GetGuessedFullWord()
        {
            int sessionGuessedFullWord = HttpContext.Session.GetInt32(SessionKeyGuessedFullWord) ?? 0;
            return sessionGuessedFullWord;
        }
        public string GetSessionWord()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyWord)))
            {
                SetWord();
            }

            string sessionWord = HttpContext.Session.GetString(SessionKeyWord) ?? "";
            return sessionWord;

        }
        public string GetSessionMaskedWord()
        {
            string sessionMaskedWord = HttpContext.Session.GetString(SessionKeyMaskedWord) ?? "";
            return sessionMaskedWord;

        }
        public int GetSessionFailCount()
        {
            int sessionFailCount = HttpContext.Session.GetInt32(SessionKeyFailCount) ?? 0;
            return sessionFailCount;

        }

        private GameStateData ComputeGameState(string playLetter)
        {
            string word = GetSessionWord();
            string maskedWord = GetSessionMaskedWord();
            GameStateData responseData = new GameStateData();
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
            }
            responseData.MaskedWordToGuess = maskedWord;
            responseData.GuessedFullWord = GetGuessedFullWord();
            responseData.FailCount = GetSessionFailCount();
            return responseData;
        }
    }

    public class PostData
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
    }
    public class LetterPostData
    {
        public string Letter { get; set; } = "";
    }
    public class GameStateData
    {
        public bool GuessedLetter { get; set; } = false;
        public int GuessedFullWord { get; set; } = 0;
        public string MaskedWordToGuess { get; set; } = "";
        public int FailCount { get; set; } = 0;

    }

    public class AppPostData
    {
        public PostData Data { get; set; } = new PostData();
    }
}