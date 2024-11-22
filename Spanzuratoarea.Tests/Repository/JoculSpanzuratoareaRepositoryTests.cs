using JoculSpanzuratoarea;
using JoculSpanzuratoarea.Data;
using JoculSpanzuratoarea.DTO;
using JoculSpanzuratoarea.Interfaces;
using JoculSpanzuratoarea.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Spanzuratoarea.Tests.Repository
{
    public class JoculSpanzuratoareaRepositoryTests
    {
        private readonly Mock<IJoculSpanzuratoareaRepository> _joculSpanzuratoareaRepository;
        public JoculSpanzuratoareaRepositoryTests()
        {
            _joculSpanzuratoareaRepository = new Mock<IJoculSpanzuratoareaRepository>();
        }

        private static ApplicationDbContext GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (databaseContext.Entries.Count() <= 0)
            {
                databaseContext.Entries.AddRange(
                    new Entry()
                    {
                        Id = 4,
                        Description = "Test 4",
                        Usable = true,
                        EntryDefinitions = new List<EntryDefinition>()
                        {
                            new EntryDefinition()
                            {
                                Id = 4,
                                EntryId = 4,
                                DefinitionId = 4
                            }
                        },
                        EntryLexemes = new List<EntryLexeme>()
                        {
                            new EntryLexeme() {
                                Id = 4,
                                EntryId = 4,
                                LexemeId = 4
                            }
                        }
                    },
                    new Entry()
                    {
                        Id = 5,
                        Description = "Test 5",
                        Usable = true,
                        EntryDefinitions = new List<EntryDefinition>()
                        {
                            new EntryDefinition()
                            {
                                Id = 5,
                                EntryId = 5,
                                DefinitionId = 5
                            }
                        },
                        EntryLexemes = new List<EntryLexeme>()
                        {
                            new EntryLexeme() {
                                Id = 5,
                                EntryId = 5,
                                LexemeId = 4
                            }
                        }
                    },
                    new Entry()
                    {
                        Id = 6,
                        Description = "Test 6",
                        Usable = true,
                        EntryDefinitions = new List<EntryDefinition>()
                        {
                            new EntryDefinition()
                            {
                                Id = 6,
                                EntryId = 6,
                                DefinitionId = 6
                            }
                        },
                        EntryLexemes = new List<EntryLexeme>()
                        {
                            new EntryLexeme() {
                                Id = 6,
                                EntryId = 6,
                                LexemeId = 4
                            }
                        }
                    },
                    new Entry()
                    {
                        Id = 7,
                        Description = "Test 7",
                        Usable = true,
                        EntryDefinitions = new List<EntryDefinition>()
                        {
                            new EntryDefinition()
                            {
                                Id = 7,
                                EntryId = 7,
                                DefinitionId = 7
                            }
                        },
                        EntryLexemes = new List<EntryLexeme>()
                        {
                            new EntryLexeme() {
                                Id = 7,
                                EntryId = 7,
                                LexemeId = 4
                            }
                        }
                    },
                    new Entry()
                    {
                        Id = 8,
                        Description = "Test 8",
                        Usable = true,
                        EntryDefinitions = new List<EntryDefinition>()
                        {
                            new EntryDefinition()
                            {
                                Id = 8,
                                EntryId = 8,
                                DefinitionId = 8
                            }
                        },
                        EntryLexemes = new List<EntryLexeme>()
                        {
                            new EntryLexeme() {
                                Id = 8,
                                EntryId = 8,
                                LexemeId = 4
                            }
                        }
                    });
            }

            if (databaseContext.Definitions.Count() <= 0)
            {
                databaseContext.Definitions.AddRange(
                    new Definition()
                    {
                        Id = 4,
                        InternalRep = "Definition for Test 4",
                        RareGlyphs = ""
                    },
                    new Definition()
                    {
                        Id = 5,
                        InternalRep = "Definition for Test 5",
                        RareGlyphs = ""
                    },
                    new Definition()
                    {
                        Id = 6,
                        InternalRep = "Definition for Test 6",
                        RareGlyphs = ""
                    },
                    new Definition()
                    {
                        Id = 7,
                        InternalRep = "Definition for Test 7",
                        RareGlyphs = ""
                    },
                    new Definition()
                    {
                        Id = 8,
                        InternalRep = "Definition for Test 8",
                        RareGlyphs = ""
                    }
                );
            }
            databaseContext.SaveChanges();
            return databaseContext;
        }

        [Fact]
        public void GetRandomWord_ReturnsWordWithDefinition()
        {
            var databaseContext = GetDbContextAsync();
            var joculSpanzuratoareaRepository = new JoculSpanzuratoareaRepository(databaseContext);

            var result = joculSpanzuratoareaRepository.GetRandomWord();

            Assert.IsType<WordWithDefinition>(result);
        }
    }
}
