using BoreholeSystem.ViewModels;
using BoreholeSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(MainWindow mainWindow, IServiceProvider serviceProvider)
        {
            _mainWindow = mainWindow;
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;
            if (viewModel == null) return;

            var view = ViewLocator.GetView(viewModel);
            if (view != null)
            {
                _mainWindow.TabsControl.Content = view;
            }
        }
    }
}
