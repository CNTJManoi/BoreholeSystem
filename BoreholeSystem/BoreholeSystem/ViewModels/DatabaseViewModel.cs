using BoreholeSystem.Database;
using BoreholeSystem.Database.Models;
using BoreholeSystem.Services;
using BoreholeSystem.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.ViewModels
{
    public partial class DatabaseViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IWPFService _wpfService;
        private DatabaseController _databaseController;
        public DatabaseViewModel(INavigationService navigationService, IWPFService wpfService)
        {
            _navigationService = navigationService;
            _wpfService = wpfService;
            _databaseController = new DatabaseController();
            InclinometerData = new ObservableCollection<InclinometerModel>(_databaseController.GetInclinometersData());
        }
        
        public DatabaseViewModel() { }

        [ObservableProperty]
        private ObservableCollection<InclinometerModel> inclinometerData;
        [RelayCommand]
        public void Exit()
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
        [RelayCommand]
        public void GetIncData()
        {
            InclinometerData = new ObservableCollection<InclinometerModel>(_databaseController.GetInclinometersData());
        }
    }
}
