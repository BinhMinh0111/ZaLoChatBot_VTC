using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZaloOA_v2.Models.DTO
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

        public virtual DbSet<OaMessage> OaMessages { get; set; } = null!;
        public virtual DbSet<OaNotice> OaNotices { get; set; } = null!;
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
            modelBuilder.Entity<OaMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("OA_Messages_pk");

                entity.ToTable("OA_Messages");

                entity.Property(e => e.MessageId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Message_id");

                entity.Property(e => e.NoticeId).HasColumnName("Notice_id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Notice)
                    .WithMany(p => p.OaMessages)
                    .HasForeignKey(d => d.NoticeId)
                    .HasConstraintName("OA_Messages_NoticeId_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OaMessages)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("OA_Messages_UserId_fk");
            });

            modelBuilder.Entity<OaNotice>(entity =>
            {
                entity.HasKey(e => e.NoticeId)
                    .HasName("OA_Notices_pk");

                entity.ToTable("OA_Notices");

                entity.Property(e => e.NoticeId)
                    .ValueGeneratedNever()
                    .HasColumnName("Notice_id");

                entity.Property(e => e.ContentUrl)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("Content_url");

                entity.Property(e => e.NoticeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Notice_Date");

                entity.Property(e => e.NumNotice).HasColumnName("Num_Notice");
            });

            modelBuilder.Entity<OaPicture>(entity =>
            {
                entity.HasKey(e => e.PictureId)
                    .HasName("OA_Pictures_pk");

                entity.ToTable("OA_Pictures");

                entity.Property(e => e.PictureId).HasColumnName("Picture_id");

                entity.Property(e => e.PicTime)
                    .HasColumnType("datetime")
                    .HasColumnName("pic_time");

                entity.Property(e => e.PicUrl)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("Pic_url");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OaPictures)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("OA_Pictures_UserId_fk");
            });

            modelBuilder.Entity<OaUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("OA_Users_pk");

                entity.ToTable("OA_Users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("User_id");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(50)
                    .HasColumnName("Display_Name");

                entity.Property(e => e.IdByApp).HasColumnName("id_by_app");

                entity.Property(e => e.UserState)
                    .HasColumnName("User_state")
                    .HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
