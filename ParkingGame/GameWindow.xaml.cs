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
        private GameArea ga;
		public GameWindow()
		{
			InitializeComponent();

		}
		private void CanvaFelosztas()
		{
            // ide jöhet majd az autók generálása
            for (int i = 0; i < ga.width; i++)
            {
                for (int j = 0; j < ga.height; j++)
                {
					
                    Rectangle rect = new Rectangle
                    {
                        Width = ga.widthFeloszt,
                        Height = ga.heightFeloszt,
                        Stroke = Brushes.Black,
                        Fill = Brushes.LightGray
                    };
                    Canvas.SetLeft(rect, i * ga.widthFeloszt);  //FONTOS:   LEFT - TOP.  ezeket használjuk mérőezköznek
                    Canvas.SetTop(rect, j * ga.heightFeloszt);
                    Game.Children.Add(rect);
                }
            }
        }

		private void StartGame()
		{
            CanvaFelosztas();
			//AutoGen();

		}

        public bool Check(Cord c) //ha nem szabad helyre néz akkor false, ha szabad akkor true
        {
            if (c.x < 0 || c.y < 0 || c.x >= ga.width || c.y >= ga.height)
            {
                return false; //pályán kívül van
            }
            foreach (UIElement child in Game.Children)  //ez a megoldás energiaigényes, lehet változtatni kell
            {
                double l = Canvas.GetLeft(child);
                double t = Canvas.GetTop(child);
                int x = (int)(l / ga.widthFeloszt);
                int y = (int)(t / ga.heightFeloszt);
                if (x == c.x && y == c.y)
                {
                    return false;
                }
            }
            return true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

		private void Level_Click(object sender, RoutedEventArgs e)
		{
            Button b = sender as Button;
			ga = new GameArea(Convert.ToInt32(b.Tag));
			LVLselect.Visibility = Visibility.Collapsed;
            StartGame();
		}
	}
}
