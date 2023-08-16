using EyeGuard.Application;
using EyeGuard.Core;
using EyeGuard.UI.Views;
using EyeGuard.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EyeGuard.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IHost AppHost { get; private set; }

        protected  override async void OnStartup(StartupEventArgs e)
        {
            AppHost = Host.CreateDefaultBuilder()
                          .ConfigureServices(s =>
                          {
                              s.AddSingleton<IDisplayService,DisplayService>();
                              s.AddTransient<DisplayViewModel>();
                          })
                          .Build();
            await AppHost.StartAsync();
            var mainWindow = new MainWindow();
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await AppHost.StopAsync();
        }
    }
}
