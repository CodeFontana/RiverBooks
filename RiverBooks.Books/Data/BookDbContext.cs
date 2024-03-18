using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.Books.Data.Entities;

namespace RiverBooks.Books.Data;

public class BookDbContext : DbContext
{
    internal DbSet<Book> Books { get; set; }

    public BookDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("books");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
