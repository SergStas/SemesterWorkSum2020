using System;
using System.Drawing;

namespace SimpleGeneticCode
{
    public class World
    {
        public ICell[,] Cells { get; private set; }
        public Size Size { get; }
        public int FreeSpace { get; private set; }
        public int AtmosphereThickness
        {
            get { return atmosphere; } 
            set
            {
                if (value < 0 || value > Constants.MaxThickness)
                    return;
                atmosphere = value;
            }
        }

        Random random;
        int idCounter;
        int atmosphere;

        public World(int width, int height, int botsCount)
        {
            AtmosphereThickness = Constants.OriginAtmosphereThickness;
            random = new Random();
            Size = new Size(width, height);
            Cells = new ICell[height, width];
            AddRandomBots(botsCount);
        }

        public World(Size size, int botsCount) : this(size.Width, size.Height, botsCount) { }

        public void Tick()
        {
            foreach (ICell currentCell in Cells)
                if (!CellIsFree(currentCell))
                    currentCell.Action();
        }

        public void AddCell(ICell cell)
        {
            if (cell is null || !(InBounds(cell.Position) && CellIsFree(cell.Position)))
                return;
            Cells[cell.Position.Y, cell.Position.X] = cell;
            FreeSpace--;
            if (cell is Bot)
                ((Bot)cell).Id = idCounter++;
        }

        public void MoveCell(Point position, int dx, int dy)
        {
            if (CellIsFree(position) || !InBounds(position.X + dx, position.Y + dy) ||
                !CellIsFree(position.X + dx, position.Y + dy))
                return;
            Cells[position.Y + dy, position.X + dx] = Cells[position.Y, position.X];
            Cells[position.Y, position.X] = null;
            Cells[position.Y + dy, position.X + dx].Position = new Point(position.X + dx, position.Y + dy);
        }

        public void RemoveCell(ICell cell)
        {
            if (cell is null || !(InBounds(cell.Position) && !CellIsFree(cell)))
                return;
            Cells[cell.Position.Y, cell.Position.X] = null;
            FreeSpace++;
        }

        public bool CellIsFree(int x, int y)
        {
            return Cells[y, x] is null;
        }

        public bool CellIsFree(Point position)
        {
            return CellIsFree(position.X, position.Y);
        }

        public bool CellIsFree(ICell cell)
        {
            return cell is null || CellIsFree(cell.Position.X, cell.Position.Y);
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Size.Width && y < Size.Height;
        }

        public bool InBounds(Point position)
        {
            return InBounds(position.X, position.Y);
        }

        public int GetSunEnergy(Point position)
        {
            double heightCoeffitient = Size.Height / (Size.Height + 2.0 * position.Y);
            double atmosphereCoeffitient = 1 - (double)AtmosphereThickness / Constants.MaxThickness;
            return (int)(Constants.MaxSunEnergy * heightCoeffitient * atmosphereCoeffitient);
        }

        public int GetMineralsEnergy(Point position)
        {
            double heightCoeffitient = (double)position.Y / Size.Height;
            return (int)(Constants.MaxMineralsEnergy * heightCoeffitient);
        }

        void AddRandomBots(int count)
        {
            while (FreeSpace > 0 && count != 0)
            {
                Point position = new Point();
                do
                {
                    position.X = random.Next(0, Size.Width);
                    position.Y = random.Next(0, Size.Height);
                }
                while (CellIsFree(position));
                Bot bot = new Bot(position, this);
                AddCell(bot);
                count--;
            }
        }
    }
}