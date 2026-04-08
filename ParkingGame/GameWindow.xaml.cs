using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
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

        List<Auto> autok;
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

        private void StartGame()
        {
            CanvaFelosztas();
            GenerateLayout();

        }

        private void GenerateLayout()
        {
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
                if (ErrorCount > 500)
                {
                    MessageBox.Show("'Sikertelen' pálya generálás, túl sok hibás próbálkozás.");
                    return;
                }
                ori = rand.Next(0, 4);
                x = rand.Next(0, ga.width);
                y = rand.Next(0, ga.height);
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

            Cord HeadC = new Cord(x, y);
            Button car;
            if (ForceCords == null)
            {
                switch (ori)
                {
                    case 0: ForceCords = new Cord(x, y + 1); break;
                    case 1: ForceCords = new Cord(x + 1, y); break;
                    case 2: ForceCords = new Cord(x, y + 1); break;
                    case 3: ForceCords = new Cord(x + 1, y); break;
                    default: ForceCords = new Cord(x, y); break;
                }
                int tries = 0;
                do
                {
                    switch (tries)
                    {
                        case 0: ForceCords = new Cord(x, y + 1); ori = 0; break;
                        case 1: ForceCords = new Cord(x + 1, y); ori = 1; break;
                        case 2: ForceCords = new Cord(x, y + 1); ori = 2; break;
                        case 3: ForceCords = new Cord(x + 1, y); ori = 4; break;
                    }
                    if (tries >= 4)
                    {
                        return null; // túl sok próbálkozás, valószínűleg zsákutca
                    }
                    tries++;
                } while (!Check(ForceCords));
                car = new Button
                {
                    Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                    Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                    Background = Brushes.Pink,
                    Content = (ori == 0) ? "^" : (ori == 1) ? ">" : (ori == 2) ? "v" : "<"
                };
            }
            else
            {
                bool found = false;
                for (int i = 0; i < 4; i++)
                {
                    int nx = ForceCords.x, ny = ForceCords.y;
                    int nori = i;
                    switch (i)
                    {
                        case 0: ny += 1; break;
                        case 1: nx += 1; break;
                        case 2: ny -= 1; break;
                        case 3: nx -= 1; break;
                    }
                    if (Check(new Cord(nx, ny)))
                    {
                        HeadC = new Cord(nx, ny);
                        ori = nori;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return ForceCords; 

                car = new Button
                {
                    Width = (ori % 2 == 0) ? ga.widthFeloszt : ga.widthFeloszt * 2,
                    Height = (ori % 2 == 0) ? ga.heightFeloszt * 2 : ga.heightFeloszt,
                    Background = Brushes.Pink,
                    Content = (ori == 0) ? "^" : (ori == 1) ? ">" : (ori == 2) ? "v" : "<"
                };
            }

            Auto auto = new Auto(car, ori, HeadC, ForceCords);
            Cord finale = auto.CheckSurroundings(this);

            if (finale == null)
            {
                Canvas.SetLeft(car, x * ga.widthFeloszt);
                Canvas.SetTop(car, y * ga.heightFeloszt);
                Game.Children.Add(car);
                autok.Add(auto); 
                return null;
            }
            else
            {
                return finale;
            }
        }


        public bool Check(Cord c)
        {
            if (c.x < 0 || c.y < 0 || c.x >= ga.width || c.y >= ga.height)
            {
                return false; // Out of bounds
            }
            foreach (UIElement child in Game.Children)
            {
                // Only check Buttons (cars), not Rectangles (background)
                if (child is not Button) continue;

                double l = Canvas.GetLeft(child);
                double t = Canvas.GetTop(child);
                int x = (int)(l / ga.widthFeloszt);
                int y = (int)(t / ga.heightFeloszt);
                if (x == c.x && y == c.y)
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
