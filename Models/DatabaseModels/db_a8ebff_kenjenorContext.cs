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

        public virtual DbSet<ZaloFeedback> ZaloFeedbacks { get; set; } = null!;
        public virtual DbSet<ZaloPicture> ZaloPictures { get; set; } = null!;
        public virtual DbSet<ZaloUser> ZaloUsers { get; set; } = null!;

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
            modelBuilder.Entity<ZaloFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("Zalo_feedback_pk");

                entity.ToTable("Zalo_Feedbacks");

                entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");

                entity.Property(e => e.Feedbacks).HasColumnName("feedbacks");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ZaloFeedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Zalo_feedback_fk");
            });

            modelBuilder.Entity<ZaloPicture>(entity =>
            {
                entity.HasKey(e => e.PictureId)
                    .HasName("Zalo_pic_pk");

                entity.ToTable("Zalo_Pictures");

                entity.Property(e => e.PictureId).HasColumnName("picture_id");

                entity.Property(e => e.PicUrl)
                    .HasMaxLength(100)
                    .HasColumnName("pic_url");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ZaloPictures)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Zalo_pic_fk");
            });

            modelBuilder.Entity<ZaloUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Zalo_user_pk");

                entity.ToTable("Zalo_users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

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
