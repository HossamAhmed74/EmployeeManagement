using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique email so we don't store the same employee twice.
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();

        // Deleting a department is blocked while it still has employees.
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed a few departments so the app is usable on first run.
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Information Technology" },
            new Department { Id = 2, Name = "Human Resources" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Marketing" }
        );
    }
}
