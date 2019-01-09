using System;
using ComicBookRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ComicBookRepository
{
    public partial class ComicBookRepositoryContext : DbContext
    {
        public ComicBookRepositoryContext()
        {
        }

        public ComicBookRepositoryContext(DbContextOptions<ComicBookRepositoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComicBookDetails> ComicBookDetails { get; set; }
        public virtual DbSet<ComicBookTitle> ComicBookTitle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Filename=D:\\\\\\\\Personal\\\\ComicBookRepository\\\\ComicBookRepository\\\\ComicBookRepository.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<ComicBookDetails>(entity =>
            {
                entity.HasIndex(e => e.Own)
                    .HasName("ComicBookDetails_IX_ComicBookDetails");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Grade).HasColumnType("nvarchar (20)");

                entity.Property(e => e.IssueName).HasColumnType("nvarchar (100)");

                entity.Property(e => e.IssueNum).HasColumnType("int");

                entity.Property(e => e.Own)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OwnerId).HasColumnType("nvarchar (450)");

                entity.Property(e => e.Rating).HasColumnType("nvarchar (50)");

                entity.Property(e => e.SpecialIssue)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.Want)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.ComicBookDetails)
                    .HasForeignKey(d => d.TitleId);
            });

            modelBuilder.Entity<ComicBookTitle>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FirstIssue).HasColumnType("int");

                entity.Property(e => e.LastIssue).HasColumnType("int");

                entity.Property(e => e.LimitedSeries)
                    .IsRequired()
                    .HasColumnType("BOOLEAN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NumIssues).HasColumnType("int");

                entity.Property(e => e.NumSpIssues).HasColumnType("int");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar (100)");
            });
        }
    }
}
