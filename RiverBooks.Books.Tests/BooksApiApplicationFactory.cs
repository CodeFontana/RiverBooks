using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RiverBooks.Books.Data;

namespace RiverBooks.Books.Tests;

public class BooksApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing BookDbContext registration
            ServiceDescriptor? descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<BookDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add BookDbContext using an in-memory database for testing
            services.AddDbContext<BookDbContext>(options =>
            {
                options.UseInMemoryDatabase("RiverBooksDb");
            });

            // Ensure the database is created
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            BookDbContext db = scopedServices.GetRequiredService<BookDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
