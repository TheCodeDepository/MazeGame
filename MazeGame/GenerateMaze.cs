using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class MazeFactory
    {
        int height;
        int width;
        Random r = new Random();
        Coordinate cellPosition;
        int index = 0;
        int rand = 1;
        int numberOfVistedCells = 0;
        int NUmberOfCells = 0;


        public Coordinate[] dirs { get; set; }
        public Cell[,] MazeMap { get; }
        public Coordinate StartPoint { get; set; }
        public Coordinate EndPoint { get; set; }

        public event EventHandler<EventArgs> MazeComplete;



        public MazeFactory(int height, int width)
        {
            this.height = height;
            this.width = width;
            //Define Maze Size in 2D Arr of cells
            MazeMap = new Cell[height, width];
            NUmberOfCells = MazeMap.GetLength(0) * MazeMap.GetLength(1);
            dirs = new Coordinate[4];
            Directions.AllDirections.CopyTo(dirs);
            EndPoint = GetMazeEndPoint();
            cellPosition = new Coordinate(EndPoint.x, EndPoint.y);

        }

        private Coordinate GetMazeEndPoint()
        {
            int num = r.Next(0, 4);

            switch (num)
            {
                case 0:
                    StartPoint = new Coordinate(0, 0);
                    return new Coordinate(height - 1, width - 1);
                case 1:
                    StartPoint = new Coordinate(0, width - 1);
                    return new Coordinate(height - 1, 0);
                case 2:
                    StartPoint = new Coordinate(0, 0);
                    return new Coordinate(height - 1, width - 1);
                case 3:
                    StartPoint = new Coordinate(height - 1, width - 1);
                    return new Coordinate(0, 0);
                default:
                    return new Coordinate(0, 0);
            }

        }


        public void GenerateMaze()
        {
            bool IsComplete = false;
            bool MoveForward = false;
            numberOfVistedCells++;
            while (!IsComplete)
            {
                MoveForward = false;
                MazeMap[cellPosition.x, cellPosition.y].visited = true;

                //Randomly determine when to chuffle the Direction list
                if (index >= rand)
                {
                    dirs.Shuffle();
                    rand = r.Next(height / 5);
                }
                index++;
                //Checks each direction for a potential Route
                foreach (Coordinate point in dirs)
                {
                    int cx = cellPosition.x + point.x;
                    int cy = cellPosition.y + point.y;
                    //Check that cell is in bounds
                    if (cx >= 0 && cx < width && cy >= 0 && cy < height)
                    {
                        //Check if the tile is already visited
                        if (!MazeMap[cx, cy].visited)
                        {
                            //Carve through walls from this tile to next
                            Carve(cellPosition, point);
                            //Set Currentcell as next cells Prior visited
                            MazeMap[cx, cy].PriorCell = cellPosition;
                            //Update Cell position to newly visited location
                            cellPosition = new Coordinate(cx, cy);
                            numberOfVistedCells++;
                            //Recursively call this method on the next tile
                            MoveForward = true;
                            break;
                        }
                    }
                }
                //check if process has returned to its starting point (the exit point for the maze)
                if (!MoveForward)
                {
                    //If it failed to find a direction and didnt get back to the EndPoint, move the current position back to the prior cell and Recall the method.
                    cellPosition = MazeMap[cellPosition.x, cellPosition.y].PriorCell;     
                }                
                if (NUmberOfCells == numberOfVistedCells)
                {
                    IsComplete = true;
                }
            }         
            MazeComplete(this, EventArgs.Empty);
        }

        private void Carve(Coordinate pos, Coordinate dir)
        {
            if (dir.Equals(Directions.West))
            {
                MazeMap[pos.y, pos.x].West = true;
                MazeMap[pos.y + dir.y, pos.x + dir.x].East = true;
            }
            else if (dir.Equals(Directions.East))
            {
                MazeMap[pos.y, pos.x].East = true;
                MazeMap[pos.y + dir.y, pos.x + dir.x].West = true;
            }
            else if (dir.Equals(Directions.South))
            {
                MazeMap[pos.y, pos.x].South = true;
                MazeMap[pos.y + dir.y, pos.x + dir.x].North = true;
            }
            else if (dir.Equals(Directions.North))
            {
                MazeMap[pos.y, pos.x].North = true;
                MazeMap[pos.y + dir.y, pos.x + dir.x].South = true;
            }
        }
    }








}



