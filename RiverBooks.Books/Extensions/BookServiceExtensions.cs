using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Books.Data;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.Extensions;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services, ConfigurationManager config)
    {
        string? connectionString = config.GetConnectionString("BooksConnectionString");
        services.AddDbContext<BookDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IBookRepository, EfBookRepository>();
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}