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
        Cord Cords;
        int Direction;

        public Auto(Button b, int dir, Cord cords1)  // dir: 0-3-ig, 0: felfelé, 1: jobbra, 2: lefelé, 3: balra
        {
            Button = b;
            Cords = cords1;
            Grid.SetRow(b, cords.x);
            Grid.SetColumn(b, cords.y);
            Direction = dir;
        }



        public Button button { get => Button; set => Button = value; }

        public int direction { get => Direction; }
        internal Cord cords { get => Cords; set => Cords = value; }
    }
}
