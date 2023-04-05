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

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        if (!optionsBuilder.IsConfigured)
//        {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//            optionsBuilder.UseSqlServer("Server=DMMHLCHK\\DMMHLCHK;Database=MilkCo;User Id=sa;Password=12345;Trusted_Connection=True; Trust Server Certificate=True");
//        }
//    }

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

            entity.HasOne(d => d.PositionNavigation)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.Position)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Positions");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasIndex(e => new { e.Product, e.Material }, "duplicates")
                .IsUnique();

            entity.HasOne(d => d.MaterialNavigation)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.Material)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingredients_Materials");

            entity.HasOne(d => d.ProductNavigation)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingredients_Products");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.Property(e => e.Cost).HasComputedColumnSql("(case when [Count]>(0) then [Amount]/[Count] else (0) end)", false);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.UnitNavigation)
                .WithMany(p => p.Materials)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Materials_Units");
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
            entity.Property(e => e.Cost).HasComputedColumnSql("(case when [Count]>(0) then [Amount]/[Count] else (0) end)", false);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.UnitNavigation)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Units");
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.Property(e => e.ProductionDate).HasColumnType("date");

            entity.HasOne(d => d.EmployeeNavigation)
                .WithMany(p => p.Productions)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productions_Employees");

            entity.HasOne(d => d.ProductNavigation)
                .WithMany(p => p.Productions)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productions_Products");
        });

        modelBuilder.Entity<PurchaseMaterial>(entity =>
        {
            entity.Property(e => e.PurchaseDate).HasColumnType("date");

            entity.HasOne(d => d.EmployeeNavigation)
                .WithMany(p => p.PurchaseMaterials)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseMaterials_Employees");

            entity.HasOne(d => d.MaterialNavigation)
                .WithMany(p => p.PurchaseMaterials)
                .HasForeignKey(d => d.Material)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseMaterials_Materials");
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

            entity.HasOne(d => d.MonthNavigation)
                .WithMany(p => p.Salaries)
                .HasForeignKey(d => d.Month);

            entity.HasOne(d => d.EmployeeNavigation)
                .WithMany(p => p.Salaries)
                .HasForeignKey(d => d.Employee)
                .HasConstraintName("FK_Salary_Months");
        });

        modelBuilder.Entity<SaleProduct>(entity =>
        {
            entity.Property(e => e.SaleDate).HasColumnType("date");

            entity.HasOne(d => d.EmployeeNavigation)
                .WithMany(p => p.SaleProducts)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleProducts_Employees");

            entity.HasOne(d => d.ProductNavigation)
                .WithMany(p => p.SaleProducts)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleProducts_Products");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}