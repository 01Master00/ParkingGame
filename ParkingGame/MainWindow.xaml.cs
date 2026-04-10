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
        DispatcherTimer dvdTimer;

        double dx1 = 3, dy1 = 3;
        double dx2 = -4, dy2 = 2;
        double dx3 = 5, dy3 = -3;

        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits.Visibility = Visibility.Visible;
            StartCreditsScroll();
            StartDvdAnimation();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Credits.Visibility = Visibility.Collapsed;

            if (dvdTimer != null)
                dvdTimer.Stop();
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
                Duration = TimeSpan.FromSeconds(8),
                RepeatBehavior = RepeatBehavior.Forever
            };

            transform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private void StartDvdAnimation()
        {
            Canvas.SetLeft(Logo1, 50);
            Canvas.SetTop(Logo1, 50);

            Canvas.SetLeft(Logo2, 200);
            Canvas.SetTop(Logo2, 150);

            Canvas.SetLeft(Logo3, 400);
            Canvas.SetTop(Logo3, 100);

            dvdTimer = new DispatcherTimer();
            dvdTimer.Interval = TimeSpan.FromMilliseconds(12);
            dvdTimer.Tick += DvdTimer_Tick;
            dvdTimer.Start();
        }

        private void DvdTimer_Tick(object sender, EventArgs e)
        {
            MoveLogo(Logo1, ref dx1, ref dy1);
            MoveLogo(Logo2, ref dx2, ref dy2);
            MoveLogo(Logo3, ref dx3, ref dy3);

            if (IsColliding(Logo1, Logo2))
            {
                dx1 = -dx1; dy1 = -dy1;
                dx2 = -dx2; dy2 = -dy2;
            }
            if (IsColliding(Logo1, Logo3))
            {
                dx1 = -dx1; dy1 = -dy1;
                dx3 = -dx3; dy3 = -dy3;
            }
            if (IsColliding(Logo2, Logo3))
            {
                dx2 = -dx2; dy2 = -dy2;
                dx3 = -dx3; dy3 = -dy3;
            }
        }
        private void MoveLogo(Image img, ref double dx, ref double dy)
        {
            double x = Canvas.GetLeft(img);
            double y = Canvas.GetTop(img);

            x += dx;
            y += dy;

            if (x <= 0 || x + img.ActualWidth >= DvdCanvas.ActualWidth)
                dx = -dx;

            if (y <= 0 || y + img.ActualHeight >= DvdCanvas.ActualHeight)
                dy = -dy;
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
        }
        private bool IsColliding(Image a, Image b)
        {
            double ax = Canvas.GetLeft(a);
            double ay = Canvas.GetTop(a);
            double aw = a.ActualWidth;
            double ah = a.ActualHeight;

            double bx = Canvas.GetLeft(b);
            double by = Canvas.GetTop(b);
            double bw = b.ActualWidth;
            double bh = b.ActualHeight;

            return ax < bx + bw &&
                   ax + aw > bx &&
                   ay < by + bh &&
                   ay + ah > by;
        }

    }
}
