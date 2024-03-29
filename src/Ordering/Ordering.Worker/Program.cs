using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ordering.Worker;
using Serilog;

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

    Log.Information("Starting web host ({ApplicationContext})", "Ordering.Worker");
    await host.RunAsync();
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", "Ordering.Worker");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


