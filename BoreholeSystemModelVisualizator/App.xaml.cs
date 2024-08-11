using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media.Media3D;

namespace BoreholeSystemModelVisualizator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Quaternion QuaternionValue { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string[] args = e.Args;

            if (args.Length > 0)
            {
                
                QuaternionValue = new Quaternion(double.Parse(args[0].Replace('.', ','))
                    , double.Parse(args[1].Replace('.', ','))
                    , double.Parse(args[2].Replace('.', ','))
                    , double.Parse(args[3].Replace('.', ',')));
            }
        }
    }

}
