using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace MazeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }


        public int GridHeight { get; set; }
        public int GridWidth { get; set; }

        public double WindowWidth { get {return WindowWidth; } }

        public int borderThickness { get { return 3; } set { borderThickness = value; } }
        public int cornerThickness { get { return 1; } set { cornerThickness = value; } }
        public int borderMargin { get { return -3; } set { borderThickness = value; } }

        public ObservableCollection<Difficulty> Difficulties { get { return new ObservableCollection<Difficulty> { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard, Difficulty.Extreme }; } }
        MazeFactory factory;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DefineGrid();
            MazeGrid.Children.Clear();
            factory = new MazeFactory(GridHeight, GridWidth);
            factory.GenerateMaze();
            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridWidth; j++)
                {

                    int north = 0;
                    int south = 0;
                    int west = 0;
                    int east = 0;

                    var t = factory.MazeMap[i, j];
                    if (t.North != true)
                    {
                        north = borderThickness;
                    }
                    if (t.South != true)
                    {
                        south = borderThickness;
                    }
                    if (t.East != true)
                    {
                        east = borderThickness;
                    }
                    if (t.West != true)
                    {
                        west = borderThickness;
                    }

                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        Margin = new Thickness(borderMargin),
                        BorderThickness = new Thickness { Top = north, Bottom = south, Right = east, Left = west },
                        CornerRadius = new CornerRadius(cornerThickness)
                    };

                    MazeGrid.Children.Add(border);
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                }
            }

            FinishLine();
            PlayerSprite();

        }

        private void FinishLine()
        {
            Rectangle ellipse = new Rectangle
            {
                Fill = Brushes.Green
            };

            MazeGrid.Children.Add(ellipse);
            Grid.SetRow(ellipse, factory.EndPoint.y);
            Grid.SetColumn(ellipse, factory.EndPoint.x);
            Grid.SetZIndex(ellipse, -1);
        }

        private void PlayerSprite()
        {
            start = new Ellipse
            {
                Margin = new Thickness(6),
                Fill = Brushes.Red,               
            };
            MazeGrid.Children.Add(start);
            Grid.SetRow(start, factory.StartPoint.y);
            Grid.SetColumn(start, factory.StartPoint.x);
        }

        private void DefineGrid()
        {
            MazeGrid.ColumnDefinitions.Clear();
            MazeGrid.RowDefinitions.Clear();
            for (int i = 0; i < GridHeight; i++)
            {
                MazeGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < GridWidth; j++)
            {
                MazeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        Ellipse start;

        private void MazeGrid_KeyDown(object sender, KeyEventArgs e)
        {
            var tmp = factory.MazeMap[Grid.GetRow(start), Grid.GetColumn(start)];
            switch (e.Key)
            {
                case Key.Left:
                    if (Grid.GetColumn(start) - 1 >= 0)
                    {
                        if (tmp.West)
                        {
                            Grid.SetColumn(start, Grid.GetColumn(start) - 1);
                            IsFinish();
                        }
                    }
                    break;

                case Key.Up:
                    if (Grid.GetRow(start) - 1 >= 0)
                    {
                        if (tmp.North)
                        {
                            Grid.SetRow(start, Grid.GetRow(start) - 1);
                            IsFinish();
                        }
                    }
                    break;

                case Key.Right:
                    if (Grid.GetColumn(start) + 1 <= GridWidth)
                    {
                        if (tmp.East)
                        {
                            Grid.SetColumn(start, Grid.GetColumn(start) + 1);
                            IsFinish();
                        }
                    }
                    break;

                case Key.Down:
                    if (Grid.GetRow(start) + 1 <= GridHeight)
                    {
                        if (tmp.South)
                        {
                            Grid.SetRow(start, Grid.GetRow(start) + 1);
                            IsFinish();
                        }
                    }
                    break;

            }
        }

        private void IsFinish()
        {
            var t = Grid.GetRow(start);
            var e = Grid.GetColumn(start);
            if (Grid.GetRow(start) == factory.EndPoint.y && Grid.GetColumn(start) == factory.EndPoint.x)
            {
                MessageBox.Show("Congrats!");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Difficulties[diffCbo.SelectedIndex])
            {
                case Difficulty.Easy:
                    GridWidth = (int)Difficulty.Easy;
                    GridHeight = (int)Difficulty.Easy;
                    break;
                case Difficulty.Medium:
                    GridWidth = (int)Difficulty.Medium;
                    GridHeight = (int)Difficulty.Medium;
                    break;
                case Difficulty.Hard:
                    GridWidth = (int)Difficulty.Hard;
                    GridHeight = (int)Difficulty.Hard;
                    break;
                case Difficulty.Extreme:
                    GridWidth = (int)Difficulty.Extreme;
                    GridHeight = (int)Difficulty.Extreme;
                    break;
            }
        }

        private void MazeGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
         
        }
    }
}
