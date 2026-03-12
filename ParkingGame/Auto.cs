using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkingGame
{
    internal class Auto
    {
        Button Button;
        int X;
        int Y;
        int Direction;

        public Auto(Button b, int x, int y, int direction)
        {
            Button = b;
            X = x;
            Grid.SetRow(Button, x);
            Y = y;
            Grid.SetColumn(Button, y);
            Direction = direction;
        }

        public void RemoveCar()
        {
            Button.Visibility = System.Windows.Visibility.Collapsed;
        }

        public Button button { get => Button; set => Button = value; }
        public int x1 { get => X; set => X = value; }
        public int y1 { get => Y; set => Y = value; }
        public int Direction1 { get => Direction; }
    }
}
