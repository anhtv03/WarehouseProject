using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WarehouseProject.Models {
    public partial class WarehouseDBContext : DbContext {
        public WarehouseDBContext() {
        }

        public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options)
            : base(options) {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<InventoryHistory> InventoryHistories { get; set; } = null!;
        public virtual DbSet<InventoryQuotum> InventoryQuota { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>(entity => {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<InventoryHistory>(entity => {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__Inventor__4D7B4ADD1E1C5D1F");

                entity.ToTable("InventoryHistory");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryHistories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Produ__60A75C0F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InventoryHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__UserI__619B8048");
            });

            modelBuilder.Entity<InventoryQuotum>(entity => {
                entity.HasKey(e => e.QuotaId)
                    .HasName("PK__Inventor__AE96C9E2FD96F7ED");

                entity.Property(e => e.QuotaId).HasColumnName("QuotaID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Height).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Length).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Width).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryQuota)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Produ__628FA481");
            });

            modelBuilder.Entity<Product>(entity => {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.AvailableQuantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CostPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Images).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Unit).HasMaxLength(20);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__Catego__6383C8BA");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Products__Suppli__6477ECF3");
            });

            modelBuilder.Entity<Role>(entity => {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Supplier>(entity => {
                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.Username, "UQ__Users__Username")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__Users__Role__6754599E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
