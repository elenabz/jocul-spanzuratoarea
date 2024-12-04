using Microsoft.EntityFrameworkCore;
using System.Configuration;

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

                entity.HasIndex(e => e.Status, "Status");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.InternalRep).HasColumnName("internalRep");
                entity.Property(e => e.Lexicon)
                    .HasMaxLength(100)
                    .HasColumnName("lexicon");
                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Entry>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity
                    .ToTable("Entry")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.HasIndex(e => e.Description, "description");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("''")
                    .HasColumnName("description");
                entity.Property(e => e.Usable).HasColumnName("usable");
            });

            modelBuilder.Entity<EntryDefinition>(entity =>
            {
                entity.HasKey(e => new { e.EntryId, e.DefinitionId }).HasName("PRIMARY");

                entity
                    .ToTable("EntryDefinition")
                    .UseCollation("utf8mb4_romanian_ci");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateDate).HasColumnName("createDate");
                entity.Property(e => e.DefinitionId).HasColumnName("definitionId");
                entity.Property(e => e.EntryId).HasColumnName("entryId");
            });

        }
    }
}
