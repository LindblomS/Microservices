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

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<WorkerOptions>(options =>
        {
            options.ConnectionString = configuration["ConnectionString"];
            options.EventBusConnection = configuration["EventBus:Connection"];
            options.SubscriptionClientName = configuration["EventBus:SubscriptionClientName"];
            options.CheckUpdateTime = configuration.GetValue<int>("CheckUpdateTime");
        });

        services.AddEventBus(configuration);
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();


