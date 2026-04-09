using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingGame
{
    public class GameArea
    {
        int Level;
        int Width;
        int Height;

        double WidthFeloszt;
        double HeightFeloszt;

        public GameArea(int Level)
        {
            this.Level = Level;
            switch (Level)
            {
                case 0:
                    width = 2;
                    height = 2;
                    break;
                case 1:
                    width = 4;
                    height = 3;
                    break;
                case 2:
                    width = 4;
                    height = 4;
                    break;
                case 3:
                    width = 5;
                    height = 4;
                    break;
                case 4:
                    width = 6;
                    height = 5;
                    break;
                case 5:
                    width = 6;
                    height = 6;
                    break;
                case 6:
                    width = 6;
                    height = 7;
                    break;
                case 7:
                    width = 7;
                    height = 8;
                    break;
                case 8:
                    width = 8;
                    height = 8;
                    break;
                case 9:
                    width = 9;
                    height = 8;
                    break;
                case 10:
                    width = 10;
                    height = 10;
                    break;
                case 11:
                    width = 11;
                    height = 10;
                    break;
                case 12:
                    width = 12;
                    height = 11;
                    break;
                case 13:
                    width = 12;
                    height = 12;
                    break;
                case 14:
                    width = 12;
                    height = 13;
                    break;
                case 15:
                    width = 14;
                    height = 14;
                    break;
                default:
                    width = 20;
                    height = 20;
                    break;
            }
            this.WidthFeloszt = 800 / this.width;
            this.HeightFeloszt = 800 / this.height;
        }







        public int width { get => Width; set => Width = value; }
        public int height { get => Height; set => Height = value; }
        public double widthFeloszt { get => WidthFeloszt; }
        public double heightFeloszt { get => HeightFeloszt; }
        public int level { get => Level; }
    }
}
