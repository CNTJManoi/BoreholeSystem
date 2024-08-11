using BoreholeSystem.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoreholeSystem.ViewModels
{
    public partial class MainViewModel : PageViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void GoToInclinometer()
        {
            _navigationService.NavigateTo<InclinometerControlViewModel>();
        }

        [RelayCommand]
        private void GoToInDatabase()
        {
            _navigationService.NavigateTo<DatabaseViewModel>();
        }
    }
}
