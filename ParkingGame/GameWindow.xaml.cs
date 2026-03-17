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

        private double cellWidth; //univerzális változók, ez alapján látom hol vannak a cellák, és hogy mekkora egy cella. Hangsúlyban a HOL
        private double cellHeight;
		private void CanvaFelosztas()
		{
            double width = Game.ActualWidth;
            double height = Game.ActualHeight;
            cellWidth = width / Gwidth.Value;
            cellHeight = height / Gheight.Value;
            // ide jöhet majd az autók generálása
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
                    Canvas.SetLeft(rect, i * cellWidth);  //FONTOS:   LEFT - TOP.  ezeket használjuk mérőezköznek
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

        public bool Check(Cord c) //ha nem szabad helyre néz akkor false, ha szabad akkor true
        {
            if (c.x < 0 || c.y < 0 || c.x >= Gwidth.Value || c.y >= Gheight.Value)
            {
                return false; //pályán kívül van
            }
            foreach (UIElement child in Game.Children)  //ez a megoldás energiaigényes, lehet változtatni kell
            {
                double l = Canvas.GetLeft(child);
                double t = Canvas.GetTop(child);
                int x = (int)(l / cellWidth);
                int y = (int)(t / cellHeight);
                if (x == c.x && y == c.y)
                {
                    return false;
                }
            }
            return true;
        }


        private void Gwidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelWidth.Content = "Magasság, jelenleg: " + Math.Floor(Gwidth.Value);

		}

		private void Gheight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelHeight.Content = "Hosszúság jelenleg: " + Math.Floor(Gheight.Value);

		}

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
