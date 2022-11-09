using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class db_a8ebff_kenjenorContext : DbContext
    {
        public db_a8ebff_kenjenorContext()
        {
        }

        public db_a8ebff_kenjenorContext(DbContextOptions<db_a8ebff_kenjenorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OaCategory> OaCategories { get; set; } = null!;
        public virtual DbSet<OaFeedback> OaFeedbacks { get; set; } = null!;
        public virtual DbSet<OaKeyword> OaKeywords { get; set; } = null!;
        public virtual DbSet<OaPicture> OaPictures { get; set; } = null!;
        public virtual DbSet<OaUser> OaUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SQL8003.site4now.net;Initial Catalog=db_a8ebff_kenjenor;User Id=db_a8ebff_kenjenor_admin;Password=Minh@258369");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OaCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("OA_category_pk");

                entity.ToTable("OA_Categories");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Category)
                    .HasMaxLength(10)
                    .HasColumnName("category");
            });

            modelBuilder.Entity<OaFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("OA_feedback_pk");

                entity.ToTable("OA_Feedbacks");

                entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");

                entity.Property(e => e.Feedbacks).HasColumnName("feedbacks");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OaFeedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("OA_feedback_fk");
            });

            modelBuilder.Entity<OaKeyword>(entity =>
            {
                entity.HasKey(e => e.KeywordId)
                    .HasName("OA_Keywords_pk");

                entity.ToTable("OA_Keywords");

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Keyword)
                    .HasMaxLength(10)
                    .HasColumnName("keyword");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.OaKeywords)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("OA_Keywords_fk");
            });

            modelBuilder.Entity<OaPicture>(entity =>
            {
                entity.HasKey(e => e.PictureId)
                    .HasName("OA_pic_pk");

                entity.ToTable("OA_Pictures");

                entity.Property(e => e.PictureId).HasColumnName("picture_id");

                entity.Property(e => e.PicUrl)
                    .HasMaxLength(100)
                    .HasColumnName("pic_url");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OaPictures)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("OA_pic_fk");
            });

            modelBuilder.Entity<OaUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Zalo_user_pk");

                entity.ToTable("OA_Users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(50)
                    .HasColumnName("display_name");

                entity.Property(e => e.UserGender).HasColumnName("user_gender");

                entity.Property(e => e.UserIdByApp).HasColumnName("user_id_by_app");

                entity.Property(e => e.UserState).HasColumnName("user_state");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
