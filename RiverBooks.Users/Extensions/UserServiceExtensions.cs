using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Data;
using RiverBooks.Users.Data.Entities;
using RiverBooks.Users.Interfaces;
using RiverBooks.Users.Services;

namespace RiverBooks.Users.Extensions;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services,
                                                     ConfigurationManager config,
                                                     ILoggerFactory logFactory)
    {
        ILogger logger = logFactory.CreateLogger("UserServiceExtensions");
        string? connectionString = config.GetConnectionString("UsersConnectionString");
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UserDbContext>();
        services.AddSingleton<ITokenService, TokenService>();
        logger.LogInformation("User services added to the service collection.");
        return services;
    }
}
