using Serilog;
using Basket.API;
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

    Log.Information("Starting web host ({ApplicationContext})", "Basket.API");
    await host.RunAsync();
    return 1;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", "Basket.API");
    return 0;
}
finally
{
    Log.CloseAndFlush();
}
