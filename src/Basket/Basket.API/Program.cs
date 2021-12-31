using Serilog;
using Basket.API;

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
    var host = Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
        })
        .UseSerilog()
        .Build();

    Log.Information("Starting web host ({ApplicationContext})", "Basket.API");
    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", "Basket.API");
}
finally
{
    Log.CloseAndFlush();
}
