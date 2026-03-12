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

		private void StartGame(object sender, RoutedEventArgs e)
		{
			try
			{
				Game.RowDefinitions.Clear();
                Game.ColumnDefinitions.Clear();

                for (int i = 0; i < Gwidth.Value; i++)
				{
					Game.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				}
				for (int i = 0; i < Gheight.Value; i++)
				{
					Game.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
				}
				question.Visibility = Visibility.Collapsed;

				for (int i = 0; i < Gwidth.Value; i++)
                {
                    for (int j = 0; j < Gheight.Value; j++)
                    {
                        Button btn = new Button();
                        btn.Content = "X";
						btn.Width = Game.ActualWidth / Gheight.Value;
						btn.Height = Game.ActualHeight / Gwidth.Value;
                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);
                        Game.Children.Add(btn);
                    }
                }
            }
			catch(Exception ex)
			{
				Error.Content = "Hiba " + ex.Message;
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
