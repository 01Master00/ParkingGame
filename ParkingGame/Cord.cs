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
		public Cord DeepCheck(GameWindow gw)
		{
			Cord Checkcord;
			//megnézi hogy van-e hely minden kocsinak a környéken
			if (!gw.Check(this))
			{
				return null; //ha foglalt akkor jó
			}


			Checkcord = new Cord(this.x, this.y + 1);
			for (int i = 0; i < 3; i++)
			{
				if (gw.Check(Checkcord)) //ha bármely oldal üres akkor jó
				{
					return null;
				}
				switch (i)
				{
					case 0:
						Checkcord = new Cord(Checkcord.x + 1, Checkcord.y - 1);
						break;
					case 1:
						Checkcord = new Cord(Checkcord.x - 1, Checkcord.y - 1);
						break;
					case 2:
						Checkcord = new Cord(Checkcord.x - 1, Checkcord.y + 1);
						break;
				}
			}
			return null;
		}








	}



}




