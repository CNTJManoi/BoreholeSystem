using BoreholeSystem.Services;
using BoreholeSystem.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BoreholeSystem.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IWPFService _wpfService;

        public MainWindowViewModel(INavigationService navigationService, IWPFService wpfService)
        {
            _navigationService = navigationService;
            _wpfService = wpfService;
            Navigate();
        }


        private void Navigate()
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
    }
}
