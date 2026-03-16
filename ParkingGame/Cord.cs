using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingGame
{

    internal class Cord
    {
        int X;
        int Y;
            
        public Cord(int x1, int y1)
        {
            X = x1;
            Y = y1;
        }

        public int x { get => X; set => X = value; }
        public int y { get => Y; set => Y = value; }


        //csekkolja a valid autolpacementet True ha jó, False ha rossz
        /*
        public bool CheckSurroundings(Auto auto, bool valid, int LastChecked = 0) //LastChecked: 0-irány és óra járásnak megfelelően halad
        {
            if (LastChecked == 9)
            {
                return true;
            }
            else if (LastChecked == 0)
            {
                //irány alapján megállapítja hogy melyik irányba kell nézni
                
            }


        }

        private bool Check(Cord c) //ha nem szabad helyre néz akkor false, ha szabad akkor true
        {
            if ()
            {

            }
        }
        */
    }



}
