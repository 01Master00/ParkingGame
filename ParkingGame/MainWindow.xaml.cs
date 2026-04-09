using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        private DispatcherTimer gameTimer;
        private void CreditsCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StartCreditsScroll();
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

        private void CreditsCanvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits.Visibility = Visibility.Visible;
            StartCreditsScroll();
            StartDvdAnimation();
        }

        private void StartCreditsScroll()
        {
            CreditsList.UpdateLayout();
            var transform = new TranslateTransform();
            CreditsList.RenderTransform = transform;

            double canvasHeight = CreditsCanvas.ActualHeight;
            double listHeight = CreditsList.ActualHeight;

            var animation = new DoubleAnimation
            {
                From = -listHeight,
                To = canvasHeight,
                Duration = TimeSpan.FromSeconds(4),
                RepeatBehavior = RepeatBehavior.Forever
            };

            transform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Credits.Visibility = Visibility.Collapsed;
            if (gameTimer != null)
                gameTimer.Stop();
            if (dvdTimer != null)
                dvdTimer.Stop();
        }
        DispatcherTimer dvdTimer;
        double dx = 3;
        double dy = 3;

        private void StartDvdAnimation()
        {
            Canvas.SetLeft(DvdText, 50);
            Canvas.SetTop(DvdText, 50);

            dvdTimer = new DispatcherTimer();
            dvdTimer.Interval = TimeSpan.FromMilliseconds(16);
            dvdTimer.Tick += DvdTimer_Tick;
            dvdTimer.Start();
        }
        private void DvdTimer_Tick(object sender, EventArgs e)
        {
            double x = Canvas.GetLeft(DvdText);
            double y = Canvas.GetTop(DvdText);
            x += dx;
            y += dy;
            if (x <= 0 || x + DvdText.ActualWidth >= DvdCanvas.ActualWidth)
                dx = -dx;

            if (y <= 0 || y + DvdText.ActualHeight >= DvdCanvas.ActualHeight)
                dy = -dy;

            Canvas.SetLeft(DvdText, x);
            Canvas.SetTop(DvdText, y);
        }
    }
}