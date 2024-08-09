using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BoreholeSystem.Services;
using BoreholeSystem.ViewModels;
using BoreholeSystem.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace BoreholeSystem
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                var mainWindow = new MainWindow();

                var serviceProvider = ConfigureServices(mainWindow);

                var navigationService = new NavigationService(mainWindow, serviceProvider);

                var mainWindowViewModel = new MainWindowViewModel(navigationService);
                var mainViewModel = new MainViewModel(navigationService);
                mainWindow.DataContext = mainWindowViewModel;

                desktop.MainWindow = mainWindow;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                var mainWindow = new MainWindow();
                var serviceProvider = ConfigureServices(mainWindow);
                var mainWindowViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();

            }

            base.OnFrameworkInitializationCompleted();
        }
        private IServiceProvider ConfigureServices(MainWindow mainWindow)
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainWindow>(mainWindow);
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<InclinometerControlViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            return services.BuildServiceProvider();
        }
    }
}