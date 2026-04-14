using ParkingGame;
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
using System.Windows.Threading;

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

                    if (Check(new Cord(autok[index].headC.x + 1, autok[index].headC.y), autok[index]) &&
                        Check(new Cord(autok[index].tailC.x + 1, autok[index].tailC.y), autok[index]))
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

            Auto car = CheckImpossible2(autok[index]);
            if (car != null)
            {
                MessageBox.Show("Lehetetlent találtál, Sorry");
                Game.Children.Remove(autok[index].button);
                autok.RemoveAt(index);
                PlaceAllCars();
                return;
            }

            MessageBox.Show("Nem lehet ezt az autót megmozdítani, próbálj egy másikat!");

        }

        public void PlaceAllCars()
        {
            foreach (Auto auto in autok)
            {
                Game.Children.Remove(auto.button);

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

        private Auto CheckImpossible2(Auto original)
        {
            Auto check = original;
            Auto newCar = null;
            for (int i = 0; i < 10; i++)
            {
                if (check == null)
                {
                    return null;
                }
                switch (check.direction)
                {
                    case 0:

                        newCar = new Cord(check.headC.x, check.headC.y - 1).GetCarByCord(this);
                        if (newCar != null)
                        {
                            if (newCar == original)
                            {
                                return newCar;
                            }
                        }
                        break;
                    case 1:
                        newCar = new Cord(check.headC.x + 1, check.headC.y).GetCarByCord(this);
                        if (newCar != null)
                        {
                            if (newCar == original)
                            {
                                return newCar;
                            }
                        }
                        break;
                    case 2:
                        newCar = new Cord(check.headC.x, check.headC.y + 1).GetCarByCord(this);
                        if (newCar != null)
                        {
                            if (newCar == original)
                            {
                                return newCar;
                            }
                        }
                        break;
                    case 3:
                        newCar = new Cord(check.headC.x - 1, check.headC.y).GetCarByCord(this);
                        if (newCar != null)
                        {
                            if (newCar == original)
                            {
                                return newCar;
                            }
                        }
                        break;
                }
                check = newCar;
                newCar = null;
            }
            return null;
        }





        // true ha lehetetlen esemény false ha lehetséges true
        private Auto CheckImpossible(Auto c)
        {
            Auto check = null, hold = null;

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
                        if (hold == null)
                        {
                            hold = check;
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
                        if (check != null && hold == null)
                        {
                            hold = check;
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
                        if (check != null && hold == null)
                        {
                            hold = check;
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
                        if (check != null && hold == null)
                        {
                            hold = check;
                        }
                    }
                    break;
            }
            hold = CheckImpossible2(hold);
            if (hold != null)
            {
                hold.button.Content = "X";
                hold.button.Background = Brushes.Red;
                MessageBox.Show("lehetetlen (loop) \norientáció: " + hold.direction + " " + hold.headC + " " + hold.tailC);
                return hold;
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

        /*
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
        */
        private int NoCarPlaced = 0;
        private async Task GenerateLayout()
        {
            if (NoCarPlaced < 5)
            {
                await Task.Delay(100);
                PlaceAllCars();
            }
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
                if (ErrorCount > ga.level * 100)
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


        private int GetMostCommonOrientation()
        {
            Dictionary<int, int> orientationCount = new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 }
            };

            foreach (Auto auto in autok)
            {
                orientationCount[auto.direction]++;
            }
            return orientationCount.OrderByDescending(o => o.Value).First().Key;
        }
        

        private Auto GenerateCar(Cord TopLeft, int ori)
        {
            Dictionary<int, int> orientationWeight = new Dictionary<int, int>
            {
                { 0, 7 },
                { 1, 7 },
                { 2, 7 },
                { 3, 7 }
            };
            Cord ForceCords = null;
            Button car = new Button();
            List<Cord> surr = TopLeft.GetSurroundingCords(this);
            if (TopLeft.x == 0)
            {
                orientationWeight[1] -= 2;
            }
            if (TopLeft.x == ga.width - 1)
            {
                orientationWeight[3] -= 2;
            }
            if (TopLeft.y == 0)
            {
                orientationWeight[0] -= 2;
            }
            if (TopLeft.y == ga.height)
            {
                orientationWeight[2] -= 2;
            }


            if (surr == null || surr.Count == 0)
            {
                return null; //ha nincs körül szabad hely akkor nem jó
            }
            else if (surr.Count == 1)
            {
                ForceCords = surr[0];
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
            else
            {
                Dictionary<int, Cord> ForceOri = new Dictionary<int, Cord>();
                foreach (Cord c in surr)
                {
                    if (c.y < TopLeft.y)
                    {
                        ForceOri[2] = c;
                    }
                    else if (c.y > TopLeft.y)
                    {
                        ForceOri[0] = c;
                    }
                    else if (c.x > TopLeft.x)
                    {
                        ForceOri[3] = c;
                    }
                    else if (c.x < TopLeft.x)
                    {
                        ForceOri[1] = c;
                    }


                    if (c.x == 0)
                    {
                        orientationWeight[3] -= 2;
                    }
                    else if(c.x == 1 && TopLeft.x != 0)
                    {
                        orientationWeight[3] -= 1;
                    }
                    if (c.x == ga.width-1)
                    {
                        orientationWeight[1] -= 2;
                    }
                    else if (c.x == ga.width - 2 && TopLeft.x != ga.width - 1)
                    {
                        orientationWeight[1] -= 1;
                    }
                    if (c.y == 0)
                    {
                        orientationWeight[2] -= 2;
                    }
                    else if (c.y == 1 && TopLeft.y != 0)
                    {
                        orientationWeight[2] -= 1;
                    }
                    if (c.y == ga.height)
                    {
                        orientationWeight[0] -= 2;
                    }
                    else if (c.y == ga.height - 1 && TopLeft.y != ga.height - 1)
                    {
                        orientationWeight[0] -= 1;
                    }
                }
                orientationWeight[GetMostCommonOrientation()] -= 3;


                int totalWeight = orientationWeight.Values.Sum();
                Random rand = new Random();
                int randomValue;
                do
                {
                    randomValue = rand.Next(0, totalWeight);
                    if (randomValue < orientationWeight[0])
                    {
                        ForceCords = ForceOri.ContainsKey(0) ? ForceOri[0] : null;
                        ori = 0;
                    }
                    else if (randomValue < orientationWeight[0] + orientationWeight[1])
                    {
                        ForceCords = ForceOri.ContainsKey(1) ? ForceOri[1] : null;
                        ori = 1;
                    }
                    else if (randomValue < orientationWeight[0] + orientationWeight[1] + orientationWeight[2])
                    {
                        ForceCords = ForceOri.ContainsKey(2) ? ForceOri[2] : null;
                        ori = 2;
                    }
                    else if (randomValue < orientationWeight[0] + orientationWeight[1] + orientationWeight[2] + orientationWeight[3])
                    {
                        ForceCords = ForceOri.ContainsKey(3) ? ForceOri[3] : null;
                        ori = 3;
                    }
                } while (ForceCords == null);

            }





            string color = (ori == 0 ? "red" : ori == 1 ? "blue" : ori == 2 ? "yellow" : "green");
            Image carImage = new Image()
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/{color}.png", UriKind.Absolute)),
                Stretch = Stretch.Fill,
                RenderTransformOrigin = new Point(0.5, 0.5),

                LayoutTransform = new RotateTransform(ori == 0 ? 180 : ori == 1 ? 270 : ori == 2 ? 0 : 90),
            };

            car = new Button()
            {
                Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                Content = carImage,
                IsEnabled = false,
                Padding = new Thickness(0)
            };


            car.Click += new RoutedEventHandler(RemoveCar);
            /*Game.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(RemoveCar));*/
            Auto a = new Auto(car, ori, TopLeft, ForceCords);

            Auto im = CheckImpossible(a);
            do
            {
                if (im != a && im != null)
                {
                    autok.Remove(im);
                    Game.Children.Remove(im.button);
                    NoCarPlaced = 0;
                }
                if (im == a && a != null)
                {
                    NoCarPlaced++;
                    return a;
                }
                im = CheckImpossible(a);
            } while (im != null);

            if (a.CheckSelf(this) && CheckImpossible(a) == null)
            {
                NoCarPlaced = 0;
                autok.Add(a);
                return a;
            }


            NoCarPlaced++;
            return null;
        }
        /*
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
            
            if (finale != null || impossible != null)
            {
                return finale;
            }
            
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
            
            else
            {
                return finale;
            }
            
        }
        */
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
            myPoints.Text = $"Pontjaid: {SaveManager.LoadPoints()}";

            foreach (var child in LevelCanvas.Children)
            {
                if (child is Button b && b.Tag != null && b.Tag.ToString() == level.ToString())
                {
                    b.Background = Brushes.LightGreen;
                    b.Foreground = Brushes.White;
                }
            }
        }

        private void LVLselect_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> completed = SaveManager.LoadCompletedLevels();
            myPoints.Text = $"Pontjaid: {SaveManager.LoadPoints()}";

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
            myPoints.Text = "Pontjaid: 0";

            foreach (var child in LevelCanvas.Children)
            {
                if (child is Button b && b.Tag != null)
                {
                    b.Background = Brushes.LightGray;
                    b.Foreground = Brushes.Black;
                }
            }
        }
        private void Mystery_Click(object sender, RoutedEventArgs e)
        {
            int points = SaveManager.LoadPoints();

            if (points < 5000)
            {
                MessageBox.Show($"A pálya feloldásához 5000 pont kell, de neked nincs ennyid");
                return;
            }
            SaveManager.SpendPoints(5000);
            myPoints.Text = $"Pontjaid: {SaveManager.LoadPoints()}";
            MessageBox.Show("Üsd az autót, ahol látod");
            StartCarHitter();
        }

        DispatcherTimer carTimer;
        Random rnd = new Random();
        Button activeCar = null;
        int carScore = 0;
        int carTimeLeft = 20;

        private void StartCarHitter()
        {
            CarHitter.Visibility = Visibility.Visible;
            CarHitterGrid.Children.Clear();
            carScore = 0;
            carTimeLeft = 20;
            for (int i = 0; i < 9; i++)
            {
                Button b = new Button()
                {
                    Background = Brushes.DarkGray,
                    FontSize = 30,
                    Tag = "empty"
                };
                b.Click += CarHitterClick;
                CarHitterGrid.Children.Add(b);
            }
            carTimer = new DispatcherTimer();
            carTimer.Interval = TimeSpan.FromMilliseconds(700);
            carTimer.Tick += CarTick;
            carTimer.Start();
        }
        private void CarTick(object sender, EventArgs e)
        {
            carTimeLeft--;
            if (carTimeLeft <= 0)
            {
                carTimer.Stop();
                MessageBox.Show($"Vége, ennyi autot találtál el: {carScore}");
                CarHitter.Visibility = Visibility.Collapsed;
                LVLselect.Visibility = Visibility.Visible;
                return;
            }
            if (activeCar != null)
            {
                activeCar.Content = "";
                activeCar.Background = Brushes.DarkGray;
                activeCar.Tag = "empty";
            }
            int index = rnd.Next(0, 9);
            activeCar = CarHitterGrid.Children[index] as Button;
            activeCar.Content = "🚗";
            activeCar.Background = Brushes.Yellow;
            activeCar.Tag = "car";
        }
        private void CarHitterClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b.Tag.ToString() == "car")
            {
                carScore++;
                b.Content = "";
                b.Background = Brushes.DarkGray;
                b.Tag = "empty";
            }
        }

    }
}
