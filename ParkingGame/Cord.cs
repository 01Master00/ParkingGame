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

        //megnézi mely kordináták szabadok a környéken és visszaadja a listájukat
        public List<Cord> GetSurroundingCords(GameWindow gw)
        {
            List<Cord> SurroundingCords = new List<Cord>();
            Cord check = this;
            for (int i = 0; i <= 3; i++)
            {
                switch (i)
                {
                    case 0:
                        check = new Cord(this.x, this.y + 1);
                        break;
                    case 1:
                        check = new Cord(this.x + 1, this.y);
                        break;
                    case 2:
                        check = new Cord(this.x, this.y - 1);
                        break;
                    case 3:
                        check = new Cord(this.x - 1, this.y);
                        break;
                }
                if (gw.Check(check))
                {
                    SurroundingCords.Add(check);
                }

            }
            return SurroundingCords.Count == 0 ? null : SurroundingCords;
        }


        public Auto GetCarByCord(GameWindow gw)
        {
            foreach (Auto a in gw.autok)
            {
                if ((a.headC.x == this.x && a.headC.y == this.y) || (a.tailC.x == this.x && a.tailC.y == this.y))
                {
                    if (a.headC != this || a.tailC != this)
                    {
                        return a;
                    }
                }
            }
            return null;
        }
        /*
        //Ez a függvény majd megkapja a játékostól a koordinátákat, és megnézi hogy szabad-e oda helyezni egy autót. Ha nem akkor false, ha igen akkor true
        //megnézi hogy van-e hely minden kocsinak a környéken
        public Cord DeepCheck(GameWindow gw)
        {
            if (!gw.Check(this))
            {
                return null; //ha foglalt akkor jó
            }

            List<Cord> check = this.GetSurroundingCords(gw);
            if (check == null)
            {
                MessageBox.Show("Nincs hely a környéken, nem helyezhető el autó ide");
                return this;
            }

            return null;
        }
        */

        override public string ToString()
        {
            return "X: " + this.x + " Y: " + this.y;
        }



    }
}




