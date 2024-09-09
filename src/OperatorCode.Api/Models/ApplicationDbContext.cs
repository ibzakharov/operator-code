using Microsoft.EntityFrameworkCore;

namespace OperatorCode.Api.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Operator> Operators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>()
            .HasKey(o => o.Code);
        
        modelBuilder.Entity<Operator>()
            .HasIndex(o => o.Name)
            .IsUnique();
    }
}