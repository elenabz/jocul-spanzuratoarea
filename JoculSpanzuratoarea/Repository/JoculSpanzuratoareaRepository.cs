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
            int entriesCount = _context.Entries.Count();
            Random random = new();
            int id = random.Next(3, entriesCount);
            var wordFromDexWithDefinition = _context.Entries
                .Join(_context.EntryDefinitions, e => e.Id, ed => ed.EntryId, (e, ed) => new { EntryDefinition = ed, Entry = e })
                .Join(_context.Definitions, ed => ed.EntryDefinition.DefinitionId, d => d.Id, (ed, d) => new { EntryDefinition = ed, Definition = d })
                .Where(e => e.EntryDefinition.Entry.Id >= id).Where(e => e.EntryDefinition.Entry.Usable).First();

            WordWithDefinition word = new()
            {
                Word = wordFromDexWithDefinition.EntryDefinition.Entry.Description.ToLower(),
                Definition = wordFromDexWithDefinition.Definition.InternalRep ?? ""
            };

            word.Word = CleanWord(word.Word);
            return word;
        }

        private static string CleanWord(string word)
        {
            bool wordContainsPunctuationMarks = word.IndexOfAny(new char[] { '(', '/' }) != -1;
            if (!wordContainsPunctuationMarks)
            {
                return word;
            }
            int indexOfNonLetter = word.IndexOfAny(new char[] { '(', '/' });
            return word.Substring(0, indexOfNonLetter).Trim();
        }
    }
}
