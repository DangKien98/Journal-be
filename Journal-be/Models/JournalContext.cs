using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Journal_be.Models
{
    public partial class JournalContext : DbContext
    {
        public JournalContext()
        {
        }

        public JournalContext(DbContextOptions<JournalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblArticle> TblArticles { get; set; } = null!;
        public virtual DbSet<TblCategory> TblCategories { get; set; } = null!;
        public virtual DbSet<TblPayment> TblPayments { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblTransaction> TblTransactions { get; set; } = null!;
        public virtual DbSet<TblTransactionDetail> TblTransactionDetails { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Journal;User Id=sa;Password=Password123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblArticle>(entity =>
            {
                entity.ToTable("tblArticle");

                entity.Property(e => e.AuthorName).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Titile).HasMaxLength(120);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblArticles)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_tblArticle_tblCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblArticles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticle_tblUser");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.ToTable("tblPayment");

                entity.Property(e => e.Method).HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblUser");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTransaction>(entity =>
            {
                entity.ToTable("tblTransaction");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_tblTransaction_tblPayment");
            });

            modelBuilder.Entity<TblTransactionDetail>(entity =>
            {
                entity.ToTable("tblTransactionDetail");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.TblTransactionDetails)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_tblTransactionDetail_tblArticle");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TblTransactionDetails)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_tblTransactionDetail_tblTransaction");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tblUser");

                entity.Property(e => e.CreateTimed).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_tblUser_tblRole1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
