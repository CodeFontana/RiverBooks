using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users.Extensions;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services,
                                                     ConfigurationManager config,
                                                     ILoggerFactory logFactory)
    {
        ILogger logger = logFactory.CreateLogger("UserServiceExtensions");
        logger.LogInformation("User services added to the service collection.");
        return services;
    }
}
