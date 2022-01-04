using Serilog;
using Catalog.API;
using Microsoft.AspNetCore;

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
    var host = WebHost
        .CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseSerilog()
        .Build();

    Log.Information("Starting web host ({ApplicationContext})", "Catalog.API");
    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", "Catalog.API");
}
finally
{
    Log.CloseAndFlush();
}