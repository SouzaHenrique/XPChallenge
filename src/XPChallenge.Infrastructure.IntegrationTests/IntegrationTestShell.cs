// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using EphemeralMongo;
using MongoDB.Driver;
using XPChallenge.Tests.Shared;

namespace XPChallenge.Infrastructure.IntegrationTests;
public class IntegrationTestShell : IDisposable
{
    public IMongoDatabase mongoDatabase { get; }
    private readonly IMongoRunner mongoRunner;
    private readonly MongoClient client;
    public FixtureContainer FixtureContainer { get; }
    public CancellationTokenSource CTS { get; } = new CancellationTokenSource();
    public IntegrationTestShell()
    {
        FixtureContainer = new FixtureContainer();

        var options = new MongoRunnerOptions
        {
            UseSingleNodeReplicaSet = false,
            StandardOuputLogger = Console.WriteLine,
            StandardErrorLogger = Console.WriteLine,
            MongoPort = 27018
        };

        if (mongoRunner is null)
            mongoRunner = MongoRunner.Run(options);

        client = new MongoClient(mongoRunner.ConnectionString);
        client.DropDatabase("DbIntegrationTest");

        mongoDatabase = client.GetDatabase("DbIntegrationTest");
    }

    public void Dispose()
    {
        mongoRunner.Dispose();
        CTS.Dispose();
    }
}
