using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;


namespace KTR.Models
{
    public partial class KTRContext : DbContext
    {
        public KTRContext()
        {
        }

        public KTRContext(DbContextOptions<KTRContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<RecipeAuditLog> RecipeAuditLog { get; set; }
        public virtual DbSet<Recipes> Recipes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategories { get; }
        public virtual DbSet<MainIngredient> MainIngredients { get; }
        public virtual DbSet<RecipeStatus> RecipeStatuses { get; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Preparation> Preparations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=KTR;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<RecipeAuditLog>(entity =>
            //{
            //    entity.HasKey(e => e.RecipeAuditId);

            //    entity.ToTable("Recipe_Audit_Log");

            //    entity.Property(e => e.RecipeAuditId).HasColumnName("RecipeAuditID");

            //    entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

            //    entity.Property(e => e.CategoryIdOld).HasColumnName("Category_Id_old");

            //    entity.Property(e => e.Description)
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.DescriptionOld)
            //        .HasColumnName("Description_old")
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.MainId).HasColumnName("Main_Id");

            //    entity.Property(e => e.MainIdOld).HasColumnName("Main_Id_old");

            //    entity.Property(e => e.PhotoPath)
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PhotoPathOld)
            //        .HasColumnName("PhotoPath_old")
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.RecipeId).HasColumnName("Recipe_Id");

            //    entity.Property(e => e.Rname)
            //        .HasColumnName("RName")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.RnameOld)
            //        .HasColumnName("RName_old")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ServingsOld).HasColumnName("Servings_old");

            //    entity.Property(e => e.StatusId).HasColumnName("Status_Id");

            //    entity.Property(e => e.StatusIdOld).HasColumnName("Status_Id_old");

            //    entity.Property(e => e.UpdatedAt)
            //        .HasColumnName("updated_at")
            //        .HasColumnType("datetime");

            //    entity.Property(e => e.UpdatedBy)
            //        .HasColumnName("updated_by")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
            //});
            
            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.HasKey(e => e.RecipeId);

                entity.Property(e => e.RecipeId)
                     .HasColumnName("Recipe_Id");

                entity.Property(e => e.CategoryId)
                     .HasColumnName("Category_Id");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("Last_Updated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MainId)
                     .HasColumnName("Main_Id");

                entity.Property(e => e.PhotoPath)
                    .HasColumnName("PhotoPath")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegId)
                     .HasColumnName("RegId")
                     .HasMaxLength(450);

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasColumnName("RName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId)
                     .HasColumnName("Status_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipes__UserId__3C69FB99");

            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__A9D10534D967D1DD")
                    .IsUnique();

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DisplayName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasColumnName("FName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasColumnName("LName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegId)
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.HasKey(e => e.IngredientId);

                entity.Property(e => e.Amt)
                    .HasColumnName("Amt")
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("Unit")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasColumnName("Item")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Prep)
                    .IsRequired()
                    .HasColumnName("Prep")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                   .HasColumnName("Last_Updated")
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RecipeId)
                    .IsRequired();

            });


            modelBuilder.Entity<Preparation>(entity =>
            {
                entity.HasKey(e => e.PrepId);

                entity.Property(e => e.Step)
                    .HasColumnName("Step")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Instr)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Updated)
                   .IsRequired()
                   .HasColumnName("Updated")
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RecipeId)
                    .IsRequired();
            });

        }
    }
}
