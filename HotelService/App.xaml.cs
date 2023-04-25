using HotelService.DataContext;
using HotelService.DataContext.Repositories;
using HotelService.ViewModels;
using HotelService.ViewModels.ApartmentVM;
using HotelService.ViewModels.HotelVM;
using HotelService.ViewModels.ServiceVM;
using HotelService.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotelService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(ManagmentWindow));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
            services.AddDbContext<HotelContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion));

            services.AddScoped<HotelRepository>();
            services.AddScoped<ServiceRepository>();
            services.AddScoped<ApartmentRepository>();

            services.AddTransient(s => new MainWindowVM(s.GetRequiredService<HotelRepository>()));
            services.AddTransient(s => new CreateHotelVM(s.GetRequiredService<HotelRepository>()));
            services.AddTransient(s => new CreateServiceVM(s.GetRequiredService<ServiceRepository>()));
            services.AddTransient(s => new CreateApartmentVM(s.GetRequiredService<ApartmentRepository>(), 
                s.GetRequiredService<HotelRepository>()));
        }
    }
}
