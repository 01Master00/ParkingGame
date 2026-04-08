using System;
using System.Collections.Generic;
using System.Linq;
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



        public Button button { get => Button; set => Button = value; }

        public int direction { get => Direction; }
        internal Cord headC { get => HeadC; set => HeadC = value; }
        internal Cord tailC { get => TailC; set => TailC = value; }




        //csekkolja a valid autolpacementet null ha jó, ha nem akkor visszaadja a problémás koordinátát
        public Cord CheckSurroundings(GameWindow gw)
        {
            Cord CheckCord;
            int dir = this.direction;

            //orientálódás
            if (this.headC.y != this.tailC.y && this.headC.x != this.tailC.x) // nem valid autó, killswich
            {
                MessageBox.Show("Nem valid autó, a két koordináta nem egy vonalban van!");
                return null;
            }
            switch (dir)
            {
                case 0: 
                    CheckCord = new Cord(this.headC.x, this.headC.y + 1);  
                    break;
                case 1: 
                    CheckCord = new Cord(this.headC.x + 1, this.headC.y); 
                    break;
                case 2:
                    CheckCord = new Cord(this.tailC.x, this.tailC.y + 1);
                    break;
                case 3:
                    CheckCord = new Cord(this.tailC.x + 1, this.tailC.y); 
                    break;
                default:  //csak 4 irány van, ha ez nem stimmel akkor valami nagyon rossz van
                    MessageBox.Show("Nem valid irány");
                    return null;
            }

            for (int i = 0; i < 5; i++) //i: (-1)-irány és óra járásnak megfelelően halad
            {
                if (CheckCord.DeepCheck(gw) != null) return CheckCord.DeepCheck(gw); //megnézi hogy van-e hely minden kocsinak a környéken
                if (dir % 2 == 0) // vertikális autó
                {
                    switch (i) // Checkcords-on relatív kordináták segítségével végiglépkedek, és megnézem hogy van-e ott autó, vagy pályán kívül van-e
                    {
                        case 0:
                            CheckCord.x++;
                            CheckCord.y--;
                            break;
                        case 1:
                            CheckCord.y--;
                            break;
                        case 2:
                            CheckCord.x--;
                            CheckCord.y--;
                            break;
                        case 3:
                            CheckCord.x--;
                            CheckCord.y++;
                            break;
                        case 4:
                            CheckCord.y++;
                            break;
                        default:
                            return null;
                    }
                }
                else // horizontális autó
                {
                    switch (i) // Checkcords-on végiglépkedek, és megnézem hogy van-e ott autó, vagy pályán kívül van-e
                    {
                        case 0:
                            CheckCord.x--;
                            CheckCord.y--;
                            break;
                        case 1:
                            CheckCord.x--;
                            break;
                        case 2:
                            CheckCord.x--;
                            CheckCord.y++;
                            break;
                        case 3:
                            CheckCord.x++;
                            CheckCord.y++;
                            break;
                        case 4:
                            CheckCord.x++;
                            break;
                        default:
                            return null;
                    }
                }
            }
            return null;

        }
    }
}
