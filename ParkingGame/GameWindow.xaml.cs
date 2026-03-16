using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParkingGame
{
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{
		public GameWindow()
		{
			InitializeComponent();

		}

		private void CanvaFelosztas()
		{
            double width = Game.ActualWidth;
            double height = Game.ActualHeight;
            double cellWidth = width / Gwidth.Value;
            double cellHeight = height / Gheight.Value;
            // ide jöhet a pálya felosztása, a cellák létrehozása és az autók generálása
            for (int i = 0; i < Gwidth.Value; i++)
            {
                for (int j = 0; j < Gheight.Value; j++)
                {
					
                    Rectangle rect = new Rectangle
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Stroke = Brushes.Black,
                        Fill = Brushes.LightGray
                    };
                    Canvas.SetLeft(rect, i * cellWidth);
                    Canvas.SetTop(rect, j * cellHeight);
                    Game.Children.Add(rect);
                }
            }
        }

		private void StartGame(object sender, RoutedEventArgs e)
		{
			CanvaFelosztas();
			question.Visibility = Visibility.Collapsed;

			//AutoGen();

		}


        private void Gwidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelWidth.Content = "Adja meg a pálya magasságát ami jelenleg " + Math.Floor(Gwidth.Value);

		}

		private void Gheight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelHeight.Content = "Adja meg a pálya szélességét ami jelenleg " + Math.Floor(Gheight.Value);

		}

	}
}
