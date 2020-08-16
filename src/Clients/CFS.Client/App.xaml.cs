using CFS.Client.Models;
using CFS.Client.Services;
using CFS.Client.ViewModels;
using CFS.Client.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CFS.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var remoteServiceBaseUrl = Configuration.GetValue<string>("RemoteServiceBaseUrl");

            services
                .AddLogging()
                .AddTransient<MainWindow>()
                .AddTransient<MainViewModel>()
                .AddScoped(x => new CustomerService(remoteServiceBaseUrl, x.GetRequiredService<ILogger<CustomerService>>()))
                .AddScoped(x => new FacilityService(remoteServiceBaseUrl, x.GetRequiredService<ILogger<FacilityService>>()))
                .AddScoped(x => new ServiceService(remoteServiceBaseUrl, x.GetRequiredService<ILogger<ServiceService>>()));
        }
    }
}
