using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using XPChallenge.Application.Commom.Contracts;
using XPChallenge.Application.Commom.Models;
using XPChallenge.Application.Commom.Models.ApplicationMailSenderOptions;
using XPChallenge.Application.Contracts;
using XPChallenge.Application.Services;
using XPChallenge.Infrastructure.Services;

namespace XPChallenge.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetConnectionString("mongodb");

        //add hangfire
        var mongoMigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new NoneMongoBackupStrategy()
        };
        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(mongoConnection, "HangFireXPChallenge",
            new MongoStorageOptions
            {
                MigrationOptions = mongoMigrationOptions,
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            }));

        services.AddHangfireServer();

        //add mongo client
        services.AddSingleton<IMongoClient>(s =>
        {
            IMongoClient mongoclient = new MongoClient(mongoConnection);
            return mongoclient;
        });

        //add mongo database
        services.AddSingleton<IMongoDatabase>(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase("xpchallenge");
            return database;
        });

        services.AddRepositories();


        services.Configure<AppOptions>(configuration.GetSection(AppOptions.Section));
        services.Configure<AppMailSenderOptions>(configuration.GetSection(AppMailSenderOptions.Section));

        services.AddScoped<IAppOptionsService, AppOptionsService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IExpiredProductsService,ExpiredProductsService>();

        MongoClassMapers.RegisterMaps();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        var typesToRegister = typeof(RepositoryBase<>).Assembly.GetTypes()
            .Where(x => x.Namespace == "XPChallenge.Infrastructure.Persistance")
            .Where(x => x.IsAbstract == false)
            .ToList();

        foreach (var type in typesToRegister)
        {
            var @interface = type.GetInterfaces().LastOrDefault(i => i.Name.Contains(type.Name));
            if (@interface is not null)
            {
                services.AddScoped(@interface, provider =>
                {
                    var database = provider.GetRequiredService<IMongoDatabase>();
                    var instance = Activator.CreateInstance(type, database);
                    return instance;
                });
            }
        }
    }
}
