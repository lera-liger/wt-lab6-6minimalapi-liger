using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_liger.Models;

namespace wt_lab7_7postgres_efcore_liger.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FullName = "Лигер Валерия", Specialty = "УИР", Course = 4, AverageGrade = 9.7, IsEnrolled = true, CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc) },
            new Student { Id = 2, FullName = "Ясковец Александра", Specialty = "УИР", Course = 4, AverageGrade = 10, IsEnrolled = true, CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 2), DateTimeKind.Utc) }
        );
    }
}