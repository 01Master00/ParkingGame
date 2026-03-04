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

		private void StartGame(object sender, RoutedEventArgs e)
		{
			try
			{
				for (int i = 0; i > Convert.ToInt32(Gwidth); i++)
				{
					Game.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				}
				for (int i = 0; i > Convert.ToInt32(Gheigt); i++)
				{
					Game.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
				}
				question.Visibility = Visibility.Collapsed;
			}
			catch(Exception ex)
			{
				Gwidth.Text = "Hiba, számot adjon meg. " + ex.Message;
				Gheigt.Text = "Hiba, számot adjon meg. " + ex.Message;
			}
		}
	}
}
