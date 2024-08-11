using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoreholeSystemModelVisualizator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetOrientation(App.QuaternionValue);
        }
        public void SetOrientation(Quaternion quaternion)
        {
            var rotation = new QuaternionRotation3D(quaternion);
            var transform = new RotateTransform3D(rotation);
            inclinometerModel.Transform = transform;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}