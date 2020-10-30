using MHBuilder.WPF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MHBuilder.Iceborne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost genericHost;

        public App()
        {
            genericHost = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services
                .AddHttpClient()
                ;
        }

        private void SetupGlobals()
        {
            Globals.SetupDownloader(genericHost.Services.GetService<IHttpClientFactory>());
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await genericHost.StartAsync();

            SetupGlobals();

            await WindowManagerExtensions.LoadAndSetup(Path.Join(Globals.ConfigDirectory, "windows.json"));

            WindowManager.Show<MainWindow>();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            WindowManagerExtensions.Save(Path.Join(Globals.ConfigDirectory, "windows.json"));

            using (genericHost)
            {
                await genericHost.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
