namespace XPChallenge.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetConnectionString("mongodb");

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
