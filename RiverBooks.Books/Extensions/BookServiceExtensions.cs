using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Data;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.Extensions;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services,
                                                     ConfigurationManager config,
                                                     ILoggerFactory logFactory)
    {
        ILogger logger = logFactory.CreateLogger("BookServiceExtensions");
        string? connectionString = config.GetConnectionString("BooksConnectionString");
        services.AddDbContext<BookDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookService, BookService>();
        logger.LogInformation("Book services added to the service collection.");
        return services;
    }
}