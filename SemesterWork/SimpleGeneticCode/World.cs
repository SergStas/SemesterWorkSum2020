using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGeneticCode
{
    public class World
    {
        public ICell[,] Cells { get; }
        public Size Size { get; }
        public int FreeSpace { get; private set; }
        public int AtmosphereThickness
        {
            get => atmosphere;
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
        bool randomPrograms;

        public World(int width, int height, int botsCount, bool startWithRandomProgram)
        {
            FreeSpace = width * height;
            AtmosphereThickness = Constants.OriginAtmosphereThickness;
            random = new Random();
            Size = new Size(width, height);
            Cells = new ICell[height, width];
            randomPrograms = startWithRandomProgram;
            AddRandomBots(botsCount);
        }

        public World(Size size, int botsCount, bool startWithRandomProgram) : this(size.Width, size.Height, botsCount, startWithRandomProgram) { }
        
        public ICell this[Point p]
        {
            get => Cells[p.Y, p.X];
            set => Cells[p.Y, p.X] = value; 
        }

        public void Tick()
        {
            foreach (ICell currentCell in GetOccupiedCells()
                .ToList().Shuffle()
            )
                currentCell.Action();
        }

        public IEnumerable<ICell> GetOccupiedCells()
        {
            foreach (ICell currentCell in Cells)
                if (!CellIsFree(currentCell))
                    yield return currentCell;
        }

        public void AddCell(ICell cell)
        {
            if (cell is null || !InBounds(cell.Position, out bool shift) || !CellIsFree(cell.Position))
                return;
            this[cell.Position] = cell;
            FreeSpace--;
            if (cell is Bot)
                ((Bot)cell).Id = idCounter++;
        }

        public void MoveCell(Point position, int dx, int dy)
        {
            Point newPos = position.Move(dx, dy);
            if (CellIsFree(position) || !InBounds(position.X + dx, position.Y + dy, out bool shift) ||
                !shift && !CellIsFree(newPos))
                return;
            if (shift)
                Shift(ref newPos);
            this[newPos] = this[position];
            this[position] = null;
            this[newPos].Position = newPos;
        }

        public void RemoveCell(ICell cell)
        {
            if (cell is null || !InBounds(cell.Position, out bool shift) || CellIsFree(cell))
                return;
            this[cell.Position] = null;
            FreeSpace++;
        }

        public bool CellIsFree(int x, int y)
        {
            return Cells[y, x] is null;
        }

        public bool CellIsFree(Point position)
        {
            return this[position] is null;
        }

        public bool CellIsFree(ICell cell)
        {
            return cell is null || CellIsFree(cell.Position);
        }

        public bool InBounds(int x, int y, out bool shift)
        {
            shift = (x < 0 || x >= Size.Width) && y >= 0 && y < Size.Height;
            return y >= 0 && y < Size.Height;
        }

        public bool InBounds(Point position, out bool shift)
        {
            return InBounds(position.X, position.Y, out shift);
        }

        public int GetSunEnergy(Point position)
        {
            double heightCoefficient = Size.Height / (Size.Height + 2.0 * position.Y);
            double atmosphereCoefficient = 1 - Math.Pow((double) AtmosphereThickness / Constants.MaxThickness, 2);
            return (int)(Constants.MaxSunEnergy * heightCoefficient * atmosphereCoefficient);
        }

        public int GetMineralsEnergy(Point position)
        {
            double heightCoefficient = (double)position.Y / Size.Height;
            return (int)(Constants.MaxMineralsEnergy * heightCoefficient);
        }

        public void Shift(ICell cell)
        {
            cell.Position = new Point((cell.Position.X + Size.Width) % Size.Width, cell.Position.Y);
        }

        public void Shift(ref Point pos)
        {
            pos = new Point((pos.X + Size.Width) % Size.Width, pos.Y);
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
                while (!CellIsFree(position));
                Bot bot = randomPrograms ? new Bot(position, this) : new Bot(position, this, Constants.StartCommand);
                AddCell(bot);
                count--;
            }
        }
    }
}