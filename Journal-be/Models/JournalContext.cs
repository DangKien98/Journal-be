using System;
using System.Collections.Generic;
using Journal_be.Entities;
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
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TestFile> TestFiles { get; set; } = null!;
        public virtual DbSet<UserEntity> UserEntities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=oggycute.tplinkdns.com;port=31100;user=root;password=password;database=Journal", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TblArticle>(entity =>
            {
                entity.ToTable("tblArticle");

                entity.HasIndex(e => e.CategoryId, "tblArticle_tblCategory_Id_fk");

                entity.HasIndex(e => e.UserId, "tblArticle_tblUser_Id_fk");

                entity.Property(e => e.ArtFile)
                    .HasColumnType("mediumblob")
                    .HasColumnName("artFile");

                entity.Property(e => e.ArtFileName)
                    .HasColumnType("text")
                    .HasColumnName("artFileName");

                entity.Property(e => e.Comment)
                    .HasColumnType("text")
                    .HasColumnName("comment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(120)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblArticles)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("tblArticle_tblCategory_Id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblArticles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tblArticle_tblUser_Id_fk");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.ToTable("tblPayment");

                entity.HasIndex(e => e.UserId, "tblPayment_tblUser_Id_fk");

                entity.Property(e => e.Method)
                    .HasMaxLength(20)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tblPayment_tblUser_Id_fk");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblTransaction>(entity =>
            {
                entity.ToTable("tblTransaction");

                entity.HasIndex(e => e.ArticleId, "tblTransaction_tblArticle_Id_fk");

                entity.HasIndex(e => e.PaymentId, "tblTransaction_tblPayment_Id_fk");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("tblTransaction_tblArticle_Id_fk");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("tblTransaction_tblPayment_Id_fk");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tblUser");

                entity.HasIndex(e => e.RoleId, "tblUser_tblRole_Id_fk");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(320);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Image).HasColumnType("text");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("tblUser_tblRole_Id_fk");
            });

            modelBuilder.Entity<TestFile>(entity =>
            {
                entity.ToTable("testFile");

                entity.Property(e => e.FileTest).HasColumnType("mediumblob");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
