using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reactive.Linq;
using System.ComponentModel;

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

        public bool isFinished { get; set; } = false;
        public int Moves
        {
            get { return int.Parse(movesDisplay.Content.ToString()); }
            set { movesDisplay.Content = value; }
        }



        public double WindowWidth { get { return WindowWidth; } }

        public int borderThickness { get { return 1; } set { borderThickness = value; } }
        public int cornerThickness { get { return 1; } set { cornerThickness = value; } }
        public int borderMargin { get { return -1; } set { borderThickness = value; } }
        public int playerMargin { get { return 1; ; } set { playerMargin = value; } }
        public Ellipse start { get; set; }
        public ObservableCollection<Difficulty> Difficulties { get { return new ObservableCollection<Difficulty> { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard, Difficulty.Extreme }; } }


        public int row { get; set; }
        public int col { get; set; }

        public Time timeTaken;
        private MazeFactory factory;

        private Thread thread;

        private delegate void MazeGenerationComplete();
        private MazeGenerationComplete mazeGenerationComplete;

        private DebounceDispatcher debounceTimer = new DebounceDispatcher();

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            DefineGrid();
            Moves = 0;
            MazeGrid.Children.Clear();
            isFinished = false;
            GenRandomMaze();
        }


        private void GenRandomMaze()
        {
            factory = new MazeFactory(GridHeight, GridWidth);
            mazeGenerationComplete = ThreadComplete;
            factory.MazeComplete += MazeComplete;
            ThreadStart threadStart = new ThreadStart(factory.GenerateMaze);
            thread = new Thread(threadStart);
            thread.Start();

        }


        private void MazeComplete(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(mazeGenerationComplete);
            }
            else
            {
                ThreadComplete();
            }
        }

        private void ThreadComplete()
        {
            CarveMaze();
            FinishLine();
            PlayerSprite();



            timeTaken = new Time();
            timeTaken.PropertyChanged += timeDisplayUpdated;
            timeTaken.Start();
        }

        private void timeDisplayUpdated(object sender, PropertyChangedEventArgs e)
        {
            timeDisplay.Content = timeTaken.TimeElapsed;
        }

        private void CarveMaze()
        {
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
                Margin = new Thickness(playerMargin),
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

        private void MazeGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (isFinished)
            {
                return;
            }
            thread.Abort();
            debounceTimer.Debounce(10, (p) =>
            {
                row = Grid.GetRow(start);
                col = Grid.GetColumn(start);
                thread = new Thread(() => MoveSprite(e));
                thread.Start();

            });
        }



        private void MoveSprite(KeyEventArgs e)
        {
            var cell = factory.MazeMap[row, col];
            switch (e.Key)
            {
                case Key.Left:
                    if (cell.West)
                    {
                        if (col - 1 >= 0)
                        {
                            Dispatcher.Invoke(MoveWest);
                        }
                    }
                    break;

                case Key.Up:
                    if (cell.North)
                    {
                        if (row - 1 >= 0)
                        {
                            Dispatcher.Invoke(MoveNorth);
                        }
                    }
                    break;

                case Key.Right:
                    if (cell.East)
                    {
                        if (col + 1 <= GridWidth)
                        {
                            Dispatcher.Invoke(MoveEast);
                        }
                    }
                    break;

                case Key.Down:
                    if (cell.South)
                    {
                        if (row + 1 <= GridHeight)
                        {
                            Dispatcher.Invoke(MoveSouth);
                        }
                    }
                    break;

            }
            Dispatcher.Invoke(IsFinish);
        }

        private void MoveSouth()
        {
            Grid.SetRow(start, ++row);
            Moves++;
        }

        private void MoveEast()
        {
            Grid.SetColumn(start, ++col);
            Moves++;
        }

        private void MoveNorth()
        {
            Grid.SetRow(start, --row);
            Moves++;
        }

        private void MoveWest()
        {
            Grid.SetColumn(start, --col);
            Moves++;
        }

        private void IsFinish()
        {

            if (row == factory.EndPoint.y && col == factory.EndPoint.x)
            {
                timeTaken.Stop();
                MessageBox.Show($"Congratulation's You've completed the maze!\nTime Taken: {timeTaken.TimeElapsed}\nMoves: {Moves}");          
                isFinished = true;

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

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            if (factory != null)
            {
                Grid.SetRow(start, factory.StartPoint.y);
                Grid.SetColumn(start, factory.StartPoint.x);
                Moves = 0;
                timeTaken.Start();
                isFinished = false;
            }
        }

    }
}
