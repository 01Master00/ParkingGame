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
		

		private void StartGame(object sender, RoutedEventArgs e)
		{
			question.Visibility = Visibility.Collapsed;

			//AutoGen();

		}

        public bool Check(Cord c) //ha nem szabad helyre néz akkor false, ha szabad akkor true
        {
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


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
