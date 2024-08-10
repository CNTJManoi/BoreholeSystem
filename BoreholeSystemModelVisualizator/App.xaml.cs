using System.Configuration;
using System.Data;
using System.Windows;

namespace BoreholeSystemModelVisualizator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string[] args = e.Args;

            if (args.Length > 0)
            {
                string firstParameter = args[0];
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }

}
