using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParkingGame
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameArea ga;

        public List<Auto> autok;
        int ErrorCount;
        public GameWindow()
        {
            InitializeComponent();

        }
        private void CanvaFelosztas()
        {
            ErrorCount = 0;
            autok = new List<Auto>();
            for (int i = 0; i < ga.width; i++)
            {
                for (int j = 0; j < ga.height; j++)
                {

                    Rectangle rect = new Rectangle
                    {
                        Width = ga.widthFeloszt,
                        Height = ga.heightFeloszt,
                        Stroke = Brushes.Black,
                        Fill = Brushes.LightGray
                    };
                    Canvas.SetLeft(rect, i * ga.widthFeloszt);  //FONTOS:   LEFT - TOP.  ezeket használjuk mérőezköznek
                    Canvas.SetTop(rect, j * ga.heightFeloszt);
                    Game.Children.Add(rect);
                }
            }
        }

        public void RemoveCar(object sender, RoutedEventArgs e) // Fix: Change parameter type to match RoutedEventHandler
        {
            Button b = sender as Button;
            Game.Children.Remove(b);
            foreach (Auto auto in autok)
            {
                if (auto.button == b)
                {
                    autok.Remove(auto);
                    return;
                }
            }
        }

        private void StartGame()
        {
            CanvaFelosztas();
            GenerateLayout();

        }

        // true ha lehetetlen esemény false ha lehetséges
        private Auto CheckImpossible(Auto c)
        {
            Cord check = c.headC;

            for (int i = 0; i <= 3; i++)
            {
                switch (i)
                {
                    case 0:
                        check = new Cord(c.headC.x, c.headC.y + 1);
                        break;
                    case 1:
                        check = new Cord(c.headC.x + 1, c.headC.y);
                        break;
                    case 2:
                        check = new Cord(c.headC.x, c.headC.y - 1);
                        break;
                    case 3:
                        check = new Cord(c.headC.x - 1, c.headC.y);
                        break;
                }
                Auto GetCar = check.GetCarByCord(this);
                if (GetCar != null && GetCar != c)
                {
                    if (GetCar.direction % 2 == c.direction % 2 && GetCar.direction != c.direction)
                    {
                        if(GetCar.direction%2 == 1)
                        {
                            if(GetCar.headC.y == c.headC.y)
                            {
                        return GetCar;
                            }
                        }
                        else if (GetCar.direction%2 == 0)
                        {
                            if (GetCar.headC.x == c.headC.x)
                            {
                                return GetCar;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private async Task GenerateLayout()
        {
            await Task.Delay(200); // Ez biztosítja, hogy a UI frissüljön minden egyes autó elhelyezése után, így láthatod a generálás folyamatát.
            if (ga.width * ga.height / 2 == autok.Count)
            {
                MessageBox.Show("Sikeres pálya generálás!");
                return;
            }
            Random rand = new Random();
            int x, y, ori;
            do
            {
                ErrorCount++;
                if (ErrorCount > ga.level * 100)
                {
                    MessageBox.Show("'Sikertelen' pálya generálás, túl sok hibás próbálkozás.");
                    return;
                }
                x = rand.Next(0, ga.width);
                y = rand.Next(0, ga.height);
                ori = rand.Next(0, 4);
            } while (!Check(new Cord(x, y)));

            Cord c = PlaceCar(x, y, ori);
            if (c == null)
            {
                GenerateLayout();
            }
            else
            {
                c = PlaceCar(x, y, ori, c);
                if (c == null)
                {
                    GenerateLayout();
                }
                else
                {
                    ErrorCount++;
                    GenerateLayout();
                }
            }
        }

        private Cord PlaceCar(int x, int y, int ori, Cord ForceCords = null)
        {
            double width = Game.ActualWidth;
            double height = Game.ActualHeight;

            Cord TopLeft = new Cord(x, y);
            Button car = new Button();
            List<Cord> surr = TopLeft.GetSurroundingCords(this);
            if (surr.Count == 0)
            {
                return null; //ha nincs körül szabad hely akkor nem jó
            }
            else if (surr.Count == 1)
            {
                ForceCords = surr[0];
                if (ForceCords.y > y)
                {
                    ori = 2;
                }
                else if (ForceCords.y < y)
                {
                    ori = 0;
                }
                else if (ForceCords.x > x)
                {
                    ori = 3;
                }
                else if (ForceCords.x < x)
                {
                    ori = 1;
                }
                car = new Button()
                {
                    Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                    Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                    Background = Brushes.Blue, // egyetlen pozíció lehetséges akkor kék
                    Content = (ori == 0) ? "^" : (ori == 1) ? ">" : (ori == 2) ? "v" : "<"
                };
            }
            else
            {
                bool s = false;
                Dictionary<int, Cord> dirToCord = new Dictionary<int, Cord>
                {
                    { 0, new Cord(x, y - 1) },
                    { 1, new Cord(x - 1, y) },
                    { 2, new Cord(x, y + 1) },
                    { 3, new Cord(x + 1, y) }
                };
                foreach (var kvp in dirToCord)
                {
                    foreach (Cord cord in surr)
                    {
                        if (cord == kvp.Value)
                        {
                            ori = kvp.Key;
                            s = true;
                        }
                    }
                }
                if (!s)
                {
                    Random rand = new Random();
                    ForceCords = surr[rand.Next(0, surr.Count)];
                    if (ForceCords.y > y)
                    {
                        ori = 2;
                    }
                    else if (ForceCords.y < y)
                    {
                        ori = 0;
                    }
                    else if (ForceCords.x > x)
                    {
                        ori = 3;
                    }
                    else if (ForceCords.x < x)
                    {
                        ori = 1;
                    }
                    car = new Button()
                    {
                        Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                        Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                        Background = Brushes.Purple, // több pozíció lehetséges akkor lila
                        Content = (ori == 0) ? "^" : (ori == 1) ? ">" : (ori == 2) ? "v" : "<"
                    };
                }

            }

            car.Click += new RoutedEventHandler(RemoveCar);
            Game.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(RemoveCar));
            Auto auto = new Auto(car, ori, TopLeft, ForceCords);
            if (!auto.CheckSelf(this))
            {
                return null; //megnézi szabad-e a hely
            }
            Cord finale = auto.CheckSurroundings(this);
            Auto impossible = CheckImpossible(auto);

            if (finale == null && impossible != null)
            {
                Cord csere = auto.headC;
                auto.headC = auto.tailC;
                auto.tailC = csere;
                auto.direction = (auto.direction + 2) % 4;
                auto.button.Content = (auto.direction == 0) ? "^" : (auto.direction == 1) ? ">" : (auto.direction == 2) ? "v" : "<";
                auto.button.Background = Brushes.Red;
            }

            impossible = CheckImpossible(auto);
            finale = auto.CheckSurroundings(this);
            if (finale == null && impossible == null)
            {
                if (ForceCords.y > y || ForceCords.x > x)
                {
                    Canvas.SetLeft(car, x * ga.widthFeloszt);
                    Canvas.SetTop(car, y * ga.heightFeloszt);
                }
                else if (ForceCords.y < y || ForceCords.x < x)
                {
                    Canvas.SetLeft(car, ForceCords.x * ga.widthFeloszt);
                    Canvas.SetTop(car, ForceCords.y * ga.heightFeloszt);
                }
                Game.Children.Add(car);
                autok.Add(auto);
                return null;
            }
            else
            {
                return finale;
            }
        }

        // ha üres true ha foglalt false
        public bool Check(Cord c)
        {
            if (c.x < 0 || c.y < 0 || c.x >= ga.width || c.y >= ga.height)
            {
                return false; // Out of bounds
            }
            foreach (Auto child in autok)
            {
                if (child.headC.x == c.x && child.headC.y == c.y || child.tailC.x == c.x && child.tailC.y == c.y)
                {
                    return false;
                }
            }
            return true;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Level_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ga = new GameArea(Convert.ToInt32(b.Tag));
            LVLselect.Visibility = Visibility.Collapsed;
            StartGame();
        }
    }
}
