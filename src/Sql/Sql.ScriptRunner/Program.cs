using Microsoft.AspNetCore.Hosting;
using Serilog;
using Sql.ScriptRunner;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService(services =>
        {
            var logger = services.GetRequiredService<ILogger<Worker>>();
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            return new Worker(logger, connectionString);
        });
    })
    .UseSerilog()
    .Build();

    await host.RunAsync();

    Log.Information("Starting web host ({ApplicationContext})", "Sql.ScriptRunner");
    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", "Sql.ScriptRunner");
}
finally
{
    Log.CloseAndFlush();
}


