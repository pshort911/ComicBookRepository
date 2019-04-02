using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
// ReSharper disable SuggestBaseTypeForParameter

namespace ComicBookRepository.Data
{
    public class ComicBookRepositoryContext : DbContext
    {
        [UsedImplicitly]
        public ComicBookRepositoryContext()
        {
        }

        [UsedImplicitly]
        public ComicBookRepositoryContext(DbContextOptions<ComicBookRepositoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComicBookDetails> ComicBookDetails { get; set; }
        public virtual DbSet<ComicBookTitle> ComicBookTitle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<ComicBookDetails>(entity =>
            {
                entity.HasIndex(e => e.Own)
                    .HasName("ComicBookDetails_IX_ComicBookDetails");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Grade).HasColumnType("nvarchar (20)");

                entity.Property(e => e.IssueName).HasColumnType("nvarchar (100)");

                entity.Property(e => e.IssueNum).HasColumnType("int");

                entity.Property(e => e.Own)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.OwnerId).HasColumnType("nvarchar (450)");

                entity.Property(e => e.Rating).HasColumnType("nvarchar (50)");

                entity.Property(e => e.SpecialIssue)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.Want)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.ComicBookDetails)
                    .HasForeignKey(d => d.TitleId);
            });

            modelBuilder.Entity<ComicBookTitle>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LimitedSeries)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.SortableTitle).HasColumnType("nvarchar (100)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar (100)");
            });
        }
    }
}
