using Avalonia.Controls;
using BoreholeSystem.ViewModels;

namespace BoreholeSystem.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.TabsControl = TabsControl;
            TabsControl.Content = new MainViewModel();
        }
    }
}