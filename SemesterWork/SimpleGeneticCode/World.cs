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
                if (!(currentCell is null))
                    currentCell.Action();
        }

        public void AddCell(ICell cell)
        {
            if (!CellIsFree(cell.Position))
                return;
            Cells[cell.Position.Y, cell.Position.X] = cell;
            FreeSpace--;
            if (cell is Bot)
                ((Bot)cell).Id = idCounter++;
        }

        public void RemoveCell(ICell cell)
        {
            if (CellIsFree(cell.Position))
                return;
            Cells[cell.Position.Y, cell.Position.X] = null;
            FreeSpace++;
        }

        public bool CellIsFree(Point position)
        {
            return Cells[position.Y, position.X] is null;
        }

        public bool InBounds(Point position)
        {
            return position.X >= 0 && position.Y >= 0 && position.X < Size.Width && position.Y < Size.Height;
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