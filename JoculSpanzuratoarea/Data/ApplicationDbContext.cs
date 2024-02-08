using Microsoft.EntityFrameworkCore;

namespace JoculSpanzuratoarea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Definition>? Definitions { get; set; }

        public virtual DbSet<Entry>? Entries { get; set; }

        public virtual DbSet<EntryDefinition>? EntryDefinitions { get; set; }

        public virtual DbSet<EntryLexeme>? EntryLexemes { get; set; }

        public virtual DbSet<Lexeme>? Lexemes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=33060;user=root;password=root;database=spanzuratoarea", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Definition>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity
                    .ToTable("Definition")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => e.CreateDate, "CreateDate");

                entity.HasIndex(e => e.Lexicon, "Lexicon");

                entity.HasIndex(e => e.ModDate, "ModDate");

                entity.HasIndex(e => e.Status, "Status");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.HasIndex(e => e.HasAmbiguousAbbreviations, "abbrevReview");

                entity.HasIndex(e => e.RareGlyphs, "rareGlyphs");

                entity.HasIndex(e => e.SourceId, "sourceId");

                entity.HasIndex(e => e.Structured, "structured");

                entity.HasIndex(e => new { e.Volume, e.Page }, "volume");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.HasAmbiguousAbbreviations).HasColumnName("hasAmbiguousAbbreviations");
                entity.Property(e => e.InternalRep).HasColumnName("internalRep");
                entity.Property(e => e.Lexicon)
                    .HasMaxLength(100)
                    .HasColumnName("lexicon");
                entity.Property(e => e.ModDate).HasColumnName("modDate");
                entity.Property(e => e.ModUserId).HasColumnName("modUserId");
                entity.Property(e => e.Page).HasColumnName("page");
                entity.Property(e => e.RareGlyphs)
                    .HasMaxLength(150)
                    .HasDefaultValueSql("''")
                    .HasColumnName("rareGlyphs");
                entity.Property(e => e.SourceId).HasColumnName("sourceId");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Structured).HasColumnName("structured");
                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Entry>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity
                    .ToTable("Entry")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => e.Description, "description");

                entity.HasIndex(e => e.StructStatus, "structStatus");

                entity.HasIndex(e => e.StructuristId, "structuristId");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Adult).HasColumnName("adult");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("''")
                    .HasColumnName("description");
                entity.Property(e => e.ModDate).HasColumnName("modDate");
                entity.Property(e => e.ModUserId).HasColumnName("modUserId");
                entity.Property(e => e.Usable).HasColumnName("usable");
                entity.Property(e => e.MultipleMains).HasColumnName("multipleMains");
                entity.Property(e => e.StructStatus)
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("structStatus");
                entity.Property(e => e.StructuristId).HasColumnName("structuristId");
            });

            modelBuilder.Entity<EntryDefinition>(entity =>
            {
                entity.HasKey(e => new { e.EntryId, e.DefinitionId }).HasName("PRIMARY");

                entity
                    .ToTable("EntryDefinition")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => new { e.DefinitionId, e.EntryRank }, "definitionId_2");

                entity.HasIndex(e => new { e.EntryId, e.DefinitionRank }, "entryId_2");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.DefinitionId).HasColumnName("definitionId");
                entity.Property(e => e.DefinitionRank).HasColumnName("definitionRank");
                entity.Property(e => e.EntryId).HasColumnName("entryId");
                entity.Property(e => e.EntryRank).HasColumnName("entryRank");
                entity.Property(e => e.ModDate).HasColumnName("modDate");
            });

            modelBuilder.Entity<EntryLexeme>(entity =>
            {
                entity.HasKey(e => new { e.EntryId, e.LexemeId }).HasName("PRIMARY");

                entity
                    .ToTable("EntryLexeme")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => new { e.EntryId, e.LexemeRank }, "entryId_2");

                entity.HasIndex(e => new { e.EntryId, e.Main }, "entryId_3");

                entity.HasIndex(e => new { e.LexemeId, e.EntryRank }, "lexemeId");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.EntryId).HasColumnName("entryId");
                entity.Property(e => e.EntryRank).HasColumnName("entryRank");
                entity.Property(e => e.LexemeId).HasColumnName("lexemeId");
                entity.Property(e => e.LexemeRank).HasColumnName("lexemeRank");
                entity.Property(e => e.Main)
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("main");
                entity.Property(e => e.ModDate).HasColumnName("modDate");
            });

            modelBuilder.Entity<Lexeme>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity
                    .ToTable("Lexeme")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => e.ModDate, "ModDate");

                entity.HasIndex(e => e.Form, "lexem_forma");

                entity.HasIndex(e => e.Reverse, "lexem_invers");

                entity.HasIndex(e => new { e.FormNoAccent, e.Description }, "lexem_neaccentuat_2");

                entity.HasIndex(e => e.FormUtf8General, "lexem_utf8_general");

                entity.HasIndex(e => e.ModelType, "modelType");

                entity.HasIndex(e => e.StaleParadigm, "staleParadigm");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Compound).HasColumnName("compound");
                entity.Property(e => e.ConsistentAccent).HasColumnName("consistentAccent");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("''")
                    .HasColumnName("description");
                entity.Property(e => e.Form)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''")
                    .HasColumnName("form");
                entity.Property(e => e.FormNoAccent)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''")
                    .HasColumnName("formNoAccent");
                entity.Property(e => e.FormUtf8General)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''")
                    .HasColumnName("formUtf8General")
                    .UseCollation("utf8mb4_general_ci");
                entity.Property(e => e.Frequency)
                    .HasDefaultValueSql("'0'")
                    .HasColumnName("frequency");
                entity.Property(e => e.HasApheresis).HasColumnName("hasApheresis");
                entity.Property(e => e.HasApocope).HasColumnName("hasApocope");
                entity.Property(e => e.Hyphenations)
                    .HasMaxLength(255)
                    .HasColumnName("hyphenations");
                entity.Property(e => e.ModDate).HasColumnName("modDate");
                entity.Property(e => e.ModelNumber)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("''")
                    .HasColumnName("modelNumber");
                entity.Property(e => e.ModelType)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("''")
                    .HasColumnName("modelType");
                entity.Property(e => e.NoAccent).HasColumnName("noAccent");
                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''")
                    .HasColumnName("notes");
                entity.Property(e => e.Number).HasColumnName("number");
                entity.Property(e => e.Pronunciations)
                    .HasMaxLength(255)
                    .HasColumnName("pronunciations");
                entity.Property(e => e.Restriction)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("''")
                    .HasColumnName("restriction");
                entity.Property(e => e.Reverse)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''")
                    .HasColumnName("reverse");
                entity.Property(e => e.StaleParadigm).HasColumnName("staleParadigm");
                entity.Property(e => e.StopWord).HasColumnName("stopWord");
            });

        }
    }
}
