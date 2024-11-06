using JoculSpanzuratoarea.Data;
using JoculSpanzuratoarea.DTO;
using JoculSpanzuratoarea.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JoculSpanzuratoarea.Repository
{
    public class JoculSpanzuratoareaRepository : IJoculSpanzuratoareaRepository
    {
        private readonly ApplicationDbContext _context;
        public JoculSpanzuratoareaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public WordWithDefinition GetRandomWord()
        {
            Random random = new();
            int id = random.Next(3, 323319);

            var wordFromDexWithDefinition = _context.Entries
                .Join(_context.EntryDefinitions, e => e.Id, ed => ed.EntryId, (e, ed) => new { EntryDefinition = ed, Entry = e })
                .Join(_context.Definitions, ed => ed.EntryDefinition.DefinitionId, d => d.Id, (ed, d) => new { EntryDefinition = ed, Definition = d })
                .Where(e => e.EntryDefinition.Entry.Id >= id).Where(e => e.EntryDefinition.Entry.Usable).First();

            WordWithDefinition word = new WordWithDefinition();
            word.Word = wordFromDexWithDefinition.EntryDefinition.Entry.Description.ToLower();
            word.Definition = wordFromDexWithDefinition.Definition.InternalRep ?? "";

            bool wordContainsPunctuation = word.Word.IndexOfAny(new char[] { '(', '/' }) != -1;
            if (wordContainsPunctuation)
            {
                word.Word = CleanWord(word.Word);
            }
            return word;
        }

        private static string CleanWord(string word)
        {
            int indexOfNonLetter = word.IndexOfAny(new char[] { '(', '/' });
            return word.Substring(0, indexOfNonLetter).Trim();
        }
    }
}
