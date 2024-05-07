using System.Reflection;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace XPChallenge.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.NotificationPublisher = new TaskWhenAllPublisher();
        });

        return services;
    }
}
