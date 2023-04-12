using Microsoft.EntityFrameworkCore;
using Web.Application.Models;

namespace Web.Application.Data;

public partial class DataBaseContext : DbContext
{
    public DataBaseContext() {}

    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options) {}

    public virtual DbSet<Budget> Budgets { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
    public virtual DbSet<Material> Materials { get; set; } = null!;
    public virtual DbSet<Month> Months { get; set; } = null!;
    public virtual DbSet<Position> Positions { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Production> Productions { get; set; } = null!;
    public virtual DbSet<PurchaseMaterial> PurchaseMaterials { get; set; } = null!;
    public virtual DbSet<Salary> Salaries { get; set; } = null!;
    public virtual DbSet<SaleProduct> SaleProducts { get; set; } = null!;
    public virtual DbSet<Unit> Units { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.ToTable("Budget");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(50);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);

            //entity.HasIndex(e => new { e.Product, e.Material }, "duplicates")
            //    .IsUnique();
            
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.Property(e => e.Cost)
                .HasComputedColumnSql("(case when [Count]>(0) then [Amount]/[Count] else (0) end)", false);

            entity.Property(e => e.Name).HasMaxLength(50);
            
        });

        modelBuilder.Entity<Month>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .IsFixedLength();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Cost)
                .HasComputedColumnSql("(case when [Count]>(0) then [Amount]/[Count] else (0) end)", false);

            entity.Property(e => e.Name).HasMaxLength(50);
            
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.Property(e => e.ProductionDate)
                .HasColumnType("date");

        });

        modelBuilder.Entity<PurchaseMaterial>(entity =>
        {
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("date");
            
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.ToTable("Salary");

            entity.HasIndex(e => new { e.Employee, e.Year, e.Month }, "uniqueSalary")
                .IsUnique();

            entity.Property(e => e.ForProduction).HasColumnName("forProduction");

            entity.Property(e => e.ForPurchase).HasColumnName("forPurchase");

            entity.Property(e => e.ForSale).HasColumnName("forSale");

            entity.Property(e => e.Issued).HasMaxLength(50);

        });

        modelBuilder.Entity<SaleProduct>(entity =>
        {
            entity.Property(e => e.SaleDate).HasColumnType("date");
            
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}