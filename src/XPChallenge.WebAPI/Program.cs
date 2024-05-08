using Hangfire;
using XPChallenge.Application.Contracts;
using XPChallenge.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

XPChallenge.Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);
XPChallenge.Application.DependencyInjection.AddApplication(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    //var expiredProductsService = scope.ServiceProvider.GetRequiredService<ExpiredProductsService>();
    //recurringJobManager.AddOrUpdate<ExpiredProductsService>("CheckForExpiredProductsAndNotify", x => expiredProductsService.CheckForExpiredProductsAndNotify(), Cron.Daily);

    IBackgroundJobClient backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
    var expiredProductsService = scope.ServiceProvider.GetRequiredService<IExpiredProductsService>();
    backgroundJobClient.Enqueue<ExpiredProductsService>(x => expiredProductsService.CheckForExpiredProductsAndNotify("hszm2094@gmail.com"));
}

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
