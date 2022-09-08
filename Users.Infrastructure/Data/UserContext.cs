using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Data.Models;

namespace Users.Infrastructure.Data;

public class UserContext : DbContext
{
    public DbSet<UserDataModel> Users { get; set; } = null!;

    private readonly string? connectionString;
    private const string migrationsConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True";

    public UserContext() : base()
    {
        // only for migrations
        connectionString = migrationsConnectionString;
    }

    public UserContext(string connectionString)
    {
        this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly(GetType().Assembly.FullName));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDataModel>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}