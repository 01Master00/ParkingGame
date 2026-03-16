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




        //csekkolja a valid autolpacementet True ha jó, False ha rossz
        public bool CheckSurroundings(Auto auto, GameWindow gw)
        {
            Cord CheckCord;
            int dir = auto.direction;

            //orientálódás
            if (auto.headC.y != auto.tailC.y && auto.headC.x != auto.tailC.x) // nem valid autó, killswich
            {
                MessageBox.Show("Nem valid autó, a két koordináta nem egy vonalban van!");
                return false;
            }
            switch (dir)
            {
                case 0: 
                    CheckCord = new Cord(auto.headC.x, auto.headC.y + 1);  
                    break;
                case 1: 
                    CheckCord = new Cord(auto.headC.x + 1, auto.headC.y); 
                    break;
                case 2:
                    CheckCord = new Cord(auto.tailC.x, auto.tailC.y + 1);
                    break;
                case 3:
                    CheckCord = new Cord(auto.tailC.x + 1, auto.tailC.y); 
                    break;
                default:  //csak 4 irány van, ha ez nem stimmel akkor valami nagyon rossz van
                    MessageBox.Show("Nem valid irány");
                    return false;
            }

            for (int i = 0; i < 5; i++) //i: (-1)-irány és óra járásnak megfelelően halad
            {
                if (!CheckCord.DeepCheck()) return false; //megnézi hogy van-e hely minden kocsinak a környéken
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
                            return true;
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
                            return true;
                    }
                }
            }
            return true;

        }
    }
}
