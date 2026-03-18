using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits.Visibility = Visibility.Visible;

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

		private void PlayClick(object sender, RoutedEventArgs e)
		{
            GameWindow GW = new GameWindow();
            GW.Show();
            
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Credits.Visibility = Visibility.Collapsed;
		}


    }
}