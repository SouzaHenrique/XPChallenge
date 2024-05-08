using System.Reflection;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPChallenge.Application.Commom.Models.ApplicationMailSenderOptions;
using XPChallenge.Application.Contracts;
using XPChallenge.Application.Services;

namespace XPChallenge.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.NotificationPublisher = new TaskWhenAllPublisher();
        });

        return services;
    }
}
