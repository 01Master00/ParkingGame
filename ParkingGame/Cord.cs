using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParkingGame
{

    public class Cord
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

        //Ez a függvény majd megkapja a játékostól a koordinátákat, és megnézi hogy szabad-e oda helyezni egy autót. Ha nem akkor false, ha igen akkor true
        public bool DeepCheck()
        {
            //megnézi hogy van-e hely minden kocsinak a környéken
            return true;
        }



 



    }


        
}




