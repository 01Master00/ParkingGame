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
				for (int i = 0; i > Gwidth.Value; i++)
				{
					Game.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				}
				for (int i = 0; i > Gheight.Value; i++)
				{
					Game.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
				}
				question.Visibility = Visibility.Collapsed;
			}
			catch(Exception ex)
			{
				Error.Content = "Hiba, számot adjon meg. " + ex.Message;
			}
		}


		private void Gwidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelWidth.Content = "Adja meg a pálya szélességét ami jelenleg " + Math.Floor(Gwidth.Value);

		}

		private void Gheight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			GlabelHeight.Content = "Adja meg a pálya magasságát ami jelenleg " + Math.Floor(Gheight.Value);

		}
	}
}
