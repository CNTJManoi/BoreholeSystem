using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoreholeSystem.ViewModels
{
    public partial class MainViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Добро пожаловать!";

        [RelayCommand]
        private void GoToInclinometer()
        {

        }
    }
}
