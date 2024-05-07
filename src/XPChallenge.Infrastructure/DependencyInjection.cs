// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



namespace XPChallenge.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetConnectionString("mongodb");

        //add mongo client
        services.AddSingleton<IMongoClient>(s =>
        {
#if DEBUG
            var debuggerClient = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost"),

                ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        System.Diagnostics.Trace.WriteLine($"{e.CommandName} - {e.Command}");
                    });
                }
            });

            return debuggerClient;
#endif

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
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        typeof(RepositoryBase<>).Assembly.GetTypes()
            .Where(x => x.Namespace == "XPChallenge.Infrastructure.Persistance")
            .Where(x => x.IsAbstract == false)
            .ToList()
            .ForEach(x =>
            {
                var @interface = x.GetInterfaces().LastOrDefault(i => i.Name.Contains(x.Name));
                if (@interface is not null)
                {
                    services.TryAddScoped(@interface, provider =>
                    {
                        var database = provider.GetRequiredService<IMongoDatabase>();
                        var instance = Activator.CreateInstance(x, database);
                        return instance;
                    });
                }
            });
    }
}
