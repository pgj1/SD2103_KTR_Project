using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<MainIngredient> MainIngredient { get; set; }
        public virtual DbSet<Preparation> Preparation { get; set; }
        public virtual DbSet<RecipeAuditLog> RecipeAuditLog { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategory { get; set; }
        public virtual DbSet<Recipes> Recipes { get; set; }
        public virtual DbSet<RecipeStatus> RecipeStatus { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=KTR;Trusted_Connection=True");
            }
        }

        // ************* ASPNET USERS *********************


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            // ************* INGREDIENTS ********************* 

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.HasKey(e => e.IngredientId);

                entity.Property(e => e.Amt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IRegId)
                    .HasColumnName("IRegId")
                    .HasMaxLength(450);

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Prep)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IReg)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.IRegId)
                    .HasConstraintName("FK_Ingredients_AspNetUsers");

                entity.HasOne(d => d.Recipe)
                   .WithMany(p => p.Ingredients)
                   .HasForeignKey(d => d.RecipeId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK__Ingredient__R_Id__440B1D61");

            });

            // ************* MAIN INGREDIENT *********************

            modelBuilder.Entity<MainIngredient>(entity =>
            {
                entity.HasKey(e => e.MainId);

                entity.Property(e => e.MainName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });



            // ************* PREPARATION *********************

            modelBuilder.Entity<Preparation>(entity =>
            {
                entity.HasKey(e => e.PrepId);

                entity.Property(e => e.Instr)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PRegId)
                    .HasColumnName("PRegId")
                    .HasMaxLength(450);

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PReg)
                    .WithMany(p => p.Preparation)
                    .HasForeignKey(d => d.PRegId)
                    .HasConstraintName("FK_Preparation_AspNetUsers");

                entity.HasOne(d => d.Recipe)
                  .WithMany(p => p.Preparation)
                  .HasForeignKey(d => d.RecipeId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Preparatio__R_id__46E78A0C");
            });

// ************************RECIPE AUDIT LOG *******************************

            modelBuilder.Entity<RecipeAuditLog>(entity =>
            {
                entity.HasKey(e => e.RecipeAuditId);

                entity.ToTable("Recipe_Audit_Log");

                entity.Property(e => e.RecipeAuditId).HasColumnName("RecipeAuditID");

                entity.Property(e => e.CategoryIdOld).HasColumnName("CategoryId_old");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionOld)
                    .HasColumnName("Description_old")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MainIdOld)
                     .HasColumnName("MainId_old");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoPathOld)
                    .HasColumnName("PhotoPath_old")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeNameOld)
                    .HasColumnName("RecipeName_old")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServingsOld).HasColumnName("Servings_old");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.StatusIdOld).HasColumnName("StatusId_old");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("Updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("Updated_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            // ************************RECIPE CATEGORY *******************************

            modelBuilder.Entity<RecipeCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CatName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });


            // ************************RECIPES *******************************

            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.HasKey(e => e.RecipeId);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegId).HasMaxLength(450);

                entity.HasOne(d => d.CatName)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipes__C_Id__3F466844");

                entity.HasOne(d => d.MainName)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.MainId)
                    .HasConstraintName("FK__Recipes__M_ID__4E88ABD4");

                entity.HasOne(d => d.StatusName)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipes__S_Id__403A8C7D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipes__UserId__3C69FB99");

            });

            // ************************RECIPE STATUS *******************************

            modelBuilder.Entity<RecipeStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });


            // ************************ USERS *******************************

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__A9D10534D967D1DD")
                    .IsUnique();

                entity.Property(e => e.DisplayName)
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







        }
    }
}
