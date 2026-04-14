using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParkingGame
{
    public class Auto
    {
        Button Button;
        Cord HeadC;
        Cord TailC;
        int Direction;

        public Auto(Button b, int dir, Cord cords1, Cord cords2)  // Direction: 0-3-ig, 0: felfelé, 1: jobbra, 2: lefelé, 3: balra
        {
            Button = b;
            HeadC = cords1;
            TailC = cords2;
            Direction = dir;
        }



        public int direction { get => Direction; set => Direction = value; }
        public Button button { get => Button; set => Button = value; }
        internal Cord headC { get => HeadC; set => HeadC = value; }
        internal Cord tailC { get => TailC; set => TailC = value; }


        //csekkolja hogy az út kifele szabad-e, ha igen akkor true, ha nem akkor false
        public bool CheckRoute(GameWindow gw, GameArea ga)
        {
            int x = headC.x;
            int y = headC.y;

            switch (this.direction)
            {
                case 0:
                    for (int i = y - 1; i >= 0; i--)
                    {
                        if (!gw.Check(new Cord(x, i), this)) return false;
                    }
                    break;
                case 1:
                    for (int i = x + 1; i < ga.width; i++)
                    {
                        if (!gw.Check(new Cord(i, y), this)) return false;
                    }
                    break;
                case 2:
                    for (int i = y + 1; i < ga.height; i++)
                    {
                        if (!gw.Check(new Cord(x, i), this)) return false;
                    }
                    break;
                case 3:
                    for (int i = x - 1; i >= 0; i--)
                    {
                        if (!gw.Check(new Cord(i, y), this)) return false;
                    }
                    break;
            }
            return true;
        }


        //csekkolja önmagát, ha bármelyik koordináta foglalt akkor false, ha mindkettő szabad akkor true
        public bool CheckSelf(GameWindow gw)
        {
            if (!gw.Check(this.headC) || !gw.Check(this.tailC)) //ha a két koordináta közül bármelyik foglalt akkor nem jó
            {
                return false;
            }
            return true;

        }

        /*
        //csekkolja a valid autolpacementet null ha jó, ha nem akkor visszaadja a problémás koordinátát
        public Cord CheckSurroundings(GameWindow gw)
        {
            int dir = this.direction;
            List<Cord> SurroundingCords = this.headC.GetSurroundingCords(gw);
            foreach(Cord c in this.tailC.GetSurroundingCords(gw))
            {
                SurroundingCords.Add(c);
            }

            if (SurroundingCords == null) return null; //ha nincs szabad hely a környéken akkor jó

            foreach (Cord c in SurroundingCords)
            {
                Cord dc = c.DeepCheck(gw);
                if (dc != null) return dc; //megnézi hogy van-e hely minden kocsinak a környéken
            }
            return null;

        }
        */
    }
}
