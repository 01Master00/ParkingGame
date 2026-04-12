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


        public void RemoveCar(object sender, RoutedEventArgs e)
        {
            //ShowHeadC();

            Button b = sender as Button;
            Auto toRemove = null;
            foreach (Auto auto in autok)
            {
                if (auto.button == b)
                {
                    toRemove = auto;
                    break;
                }
            }
            if (toRemove == null)
            {
                MessageBox.Show("Hiba történt (null autó), próbáld újra!");
                return;
            }
            int index = autok.IndexOf(toRemove);
            if (autok[index].CheckRoute(this, ga))
            {
                Game.Children.Remove(toRemove.button);
                autok.RemoveAt(index);

                if (autok.Count == 0)
                {
                    MessageBox.Show("NYERTÉL \n:)");
                    HighlightCompletedLevel(ga.level);
                    Game.Children.Clear();
                    autok.Clear();
                    LVLselect.Visibility = Visibility.Visible;
                }
                return;
            }
            switch (autok[index].direction)
            {
                case 0:
                    if (Check(new Cord(autok[index].headC.x, autok[index].headC.y - 1), autok[index]) &&
                        Check(new Cord(autok[index].tailC.x, autok[index].tailC.y - 1), autok[index]))
                    {
                        Game.Children.Remove(autok[index].button);
                        autok[index].headC.y -= 1;
                        autok[index].tailC.y -= 1;
                        PlaceAllCars();
                        return;
                    }
                    break;
                case 1:

                    if (Check(new Cord(autok[index].headC.x+1, autok[index].headC.y ), autok[index]) &&
                        Check(new Cord(autok[index].tailC.x+1, autok[index].tailC.y ), autok[index]))
                    {
                        Game.Children.Remove(autok[index].button);
                        autok[index].headC.x += 1;
                        autok[index].tailC.x += 1;
                        PlaceAllCars();
                        return;
                    }

                    break;
                case 2:

                    if (Check(new Cord(autok[index].headC.x, autok[index].headC.y + 1), autok[index]) &&
                        Check(new Cord(autok[index].tailC.x, autok[index].tailC.y + 1), autok[index]))
                    {
                        Game.Children.Remove(autok[index].button);
                        autok[index].headC.y += 1;
                        autok[index].tailC.y += 1;
                        PlaceAllCars();
                        return;
                    }
                    break;
                case 3:

                    if (Check(new Cord(autok[index].headC.x - 1, autok[index].headC.y), autok[index]) &&
                        Check(new Cord(autok[index].tailC.x - 1, autok[index].tailC.y), autok[index]))
                    {
                        Game.Children.Remove(autok[index].button);
                        autok[index].headC.x -= 1;
                        autok[index].tailC.x -= 1;
                        PlaceAllCars();
                        return;
                    }
                    break;
            }


            MessageBox.Show("Nem lehet ezt az autót megmozdítani, próbálj egy másikat!");


        }

        public void PlaceAllCars()
        {
            Game.Children.Clear();
            foreach (Auto auto in autok)
            {

                if (auto.tailC.y > auto.headC.y || auto.tailC.x > auto.headC.x)
                {
                    Canvas.SetLeft(auto.button, auto.headC.x * ga.widthFeloszt);
                    Canvas.SetTop(auto.button, auto.headC.y * ga.heightFeloszt);
                }
                else if (auto.tailC.y < auto.headC.y || auto.tailC.x < auto.headC.x)
                {
                    Canvas.SetLeft(auto.button, auto.tailC.x * ga.widthFeloszt);
                    Canvas.SetTop(auto.button, auto.tailC.y * ga.heightFeloszt);
                }
                Game.Children.Add(auto.button);
            }
        }

        private void StartGame()
        {
            CanvaFelosztas();
            GenerateLayout();
        }

        // true ha lehetetlen esemény false ha lehetséges true
        private Auto CheckImpossible(Auto c)
        {
            Auto check = null;

            switch (c.direction)
            {
                case 0:
                    for (int i = c.headC.y; i >= 0; i--)
                    {
                        check = new Cord(c.headC.x, i).GetCarByCord(this);
                        if (check != null && c.direction % 2 == check.direction % 2 && c.direction != check.direction)
                        {
                            return check;
                        }
                    }
                    break;
                case 1:
                    for (int i = c.headC.x; i <= ga.width; i++)
                    {
                        check = new Cord(i, c.headC.y).GetCarByCord(this);
                        if (check != null && c.direction % 2 == check.direction % 2 && c.direction != check.direction)
                        {
                            return check;
                        }
                    }
                    break;
                case 2:
                    for (int i = c.headC.y; i <= ga.height; i++)
                    {
                        check = new Cord(c.headC.x, i).GetCarByCord(this);
                        if (check != null && c.direction % 2 == check.direction % 2 && c.direction != check.direction)
                        {
                            return check;
                        }
                    }
                    break;
                case 3:
                    for (int i = c.headC.x; i >= 0; i--)
                    {
                        check = new Cord(i, c.headC.y).GetCarByCord(this);
                        if (check != null && c.direction % 2 == check.direction % 2 && c.direction != check.direction)
                        {
                            return check;
                        }
                    }
                    break;
            }
            return null;
        }

        private void EnableCars()
        {
            foreach (Auto auto in autok)
            {
                auto.button.IsEnabled = true;
            }
        }


        private void ShowHeadC()
        {
            foreach (Auto auto in autok)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Width = ga.widthFeloszt,
                    Height = ga.heightFeloszt,
                    Text = "headc",
                    Background = Brushes.Purple

                };
                Canvas.SetLeft(textBlock, auto.headC.x * ga.width);
                Canvas.SetTop(textBlock, auto.headC.y * ga.height);
            }
        }

        private async Task GenerateLayout()
        {
            //await Task.Delay(50); // Ez biztosítja, hogy a UI frissüljön minden egyes autó elhelyezése után, így láthatod a generálás folyamatát.
            if (ga.width * ga.height / 2 == autok.Count)
            {
                MessageBox.Show("Kezdődjön a játék!");
                EnableCars();
                PlaceAllCars();
                return;
            }
            Random rand = new Random();
            int x, y, ori;
            do
            {
                ErrorCount++;
                if (ErrorCount > ga.level * 50)
                {
                    MessageBox.Show("Kezdődjön a játék (lyukakkal)!");
                    EnableCars();
                    PlaceAllCars();
                    return;
                }
                x = rand.Next(0, ga.width);
                y = rand.Next(0, ga.height);
                ori = rand.Next(0, 4);
            } while (!Check(new Cord(x, y)));
            Auto c = GenerateCar(new Cord(x, y), ori);
            GenerateLayout();
            /*
             if (c == null)
             {
                 GenerateLayout();
             }
            else
             {
                 c = PlaceCar(GenerateCar(c, ori));
                 if (c == null)
                 {
                     GenerateLayout();
                 }
                 else
                 {
                     ErrorCount++;
                     GenerateLayout();
                 }
             }*/
        }


        private Auto GenerateCar(Cord TopLeft, int ori)
        {
            Cord ForceCords = new Cord(-1, -1);
            Button car = new Button();
            List<Cord> surr = TopLeft.GetSurroundingCords(this);
            if (surr == null)
            {
                return null; //ha nincs körül szabad hely akkor nem jó
            }
            else if (surr.Count == 1)
            {
                ForceCords = surr[0];
                if (ForceCords.y > TopLeft.y)
                {
                    ori = 2;
                }
                else if (ForceCords.y < TopLeft.y)
                {
                    ori = 0;
                }
                else if (ForceCords.x > TopLeft.x)
                {
                    ori = 3;
                }
                else if (ForceCords.x < TopLeft.x)
                {
                    ori = 1;
                }
            }
            else
            {
                bool s = false;
                Dictionary<int, Cord> dirToCord = new Dictionary<int, Cord>
                {
                    { 0, new Cord(TopLeft.x, TopLeft.y - 1) },
                    { 1, new Cord(TopLeft.x - 1, TopLeft.y) },
                    { 2, new Cord(TopLeft.x, TopLeft.y + 1) },
                    { 3, new Cord(TopLeft.x + 1, TopLeft.y) }
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
                    if (ForceCords.y < TopLeft.y)
                    {
                        ori = 2;
                    }
                    else if (ForceCords.y > TopLeft.y)
                    {
                        ori = 0;
                    }
                    else if (ForceCords.x > TopLeft.x)
                    {
                        ori = 3;
                    }
                    else if (ForceCords.x < TopLeft.x)
                    {
                        ori = 1;
                    }

                }

            }

            car = new Button()
            {
                Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                Content = (ori == 0) ? "^" : (ori == 1) ? ">" : (ori == 2) ? "v" : "<",
                Background = (ori == 0) ? Brushes.Green : (ori == 1) ? Brushes.Yellow : (ori == 2) ? Brushes.Blue : Brushes.Red,
                IsEnabled = false
            };

            car.Click += new RoutedEventHandler(RemoveCar);
            /*Game.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(RemoveCar));*/
            Auto a = new Auto(car, ori, TopLeft, ForceCords);

            if (a.CheckSelf(this) && CheckImpossible(a) == null)
            {
                autok.Add(a);
                return a;
            }



            return null;
        }

        private Cord PlaceCar(Auto auto)
        {
            if (auto == null)
            {
                MessageBox.Show("Nem sikerült autót generálni, újrapróbálkozás...");
                return null;
            }
            if (!auto.CheckSelf(this))
            {
                MessageBox.Show("Nincs hely az autónak, újrapróbálkozás...");
                return null; //megnézi szabad-e a hely
            }
            Auto impossible = CheckImpossible(auto);
            //Cord finale = auto.CheckSurroundings(this);
            /*
            if (finale != null || impossible != null)
            {
                return finale;
            }
            */
            //finale = auto.CheckSurroundings(this);
            if (impossible == null)
            {
                if (auto.tailC.y > auto.headC.y || auto.tailC.x > auto.headC.x)
                {
                    Canvas.SetLeft(auto.button, auto.headC.x * ga.widthFeloszt);
                    Canvas.SetTop(auto.button, auto.headC.y * ga.heightFeloszt);
                }
                else if (auto.tailC.y < auto.headC.y || auto.tailC.x < auto.headC.x)
                {
                    Canvas.SetLeft(auto.button, auto.tailC.x * ga.widthFeloszt);
                    Canvas.SetTop(auto.button, auto.tailC.y * ga.heightFeloszt);
                }
                Game.Children.Add(auto.button);
                autok.Add(auto);
            }
            return null;
            /*
            else
            {
                return finale;
            }
            */
        }

        // ha üres true ha foglalt false
        public bool Check(Cord c, Auto ignore = null)
        {
            if (c.x < 0 || c.y < 0 || c.x >= ga.width || c.y >= ga.height)
            {
                return false; // Out of bounds
            }
            foreach (Auto child in autok)
            {
                if (ignore != null && child == ignore) continue;
                if ((child.headC.x == c.x && child.headC.y == c.y) ||
                    (child.tailC.x == c.x && child.tailC.y == c.y))
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

        private void HighlightCompletedLevel(int level)
        {
            SaveManager.SaveCompletedLevel(level);
            foreach (var child in LevelCanvas.Children)
            {
                if (child is Button b && b.Tag != null)
                {
                    if (b.Tag.ToString() == level.ToString())
                    {
                        b.Background = Brushes.LightGreen;
                        b.Foreground = Brushes.White;
                    }
                }
            }
        }

        private void LVLselect_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> completed = SaveManager.LoadCompletedLevels();

            foreach (var child in LevelCanvas.Children)
            {
                if (child is Button b && b.Tag != null)
                {
                    int level = Convert.ToInt32(b.Tag);
                    if (completed.Contains(level))
                    {
                        b.Background = Brushes.LightGreen;
                        b.Foreground = Brushes.White;
                    }
                }
            }
        }

        private void DeleteSave_Click(object sender, RoutedEventArgs e)
        {
            SaveManager.DeleteSave();
            foreach (var child in LevelCanvas.Children)
            {
                if (child is Button b && b.Tag != null)
                {
                    b.Background = Brushes.LightGray;
                    b.Foreground = Brushes.Black;
                }
            }
        }

    }
}
