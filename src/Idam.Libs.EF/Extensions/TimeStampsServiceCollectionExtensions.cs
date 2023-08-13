using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Models;

namespace Microsoft.Extensions.DependencyInjection;
public static class TimeStampsServiceCollectionExtensions
{
    /// <summary>
    /// Registers an TimeStampsOptions action to configure a particular type of options.
    /// Also Inject IServiceProvider to DbContextExtension class.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureTimeStamps(this IServiceCollection services, Action<TimeStampsOptions> setupAction)
    {
        services.Configure(setupAction);

        DbContextExtensions.Configure(services.BuildServiceProvider());

        return services;
    }
}
