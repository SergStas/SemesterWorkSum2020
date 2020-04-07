using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SimpleGeneticCode;

namespace MainWindow
{
    class Map
    {/*
        #region
        public Size MapSize { get; set; }
        public Bitmap Bitmap { get; set; }
        public Size MatrixSize { get; }
        public World Image { get; }
        
        Size windowSize;
        Size cellSize;

        public Map(World world)
        {
            Image = world;
            windowSize = new Size(Constants.XWindow, Constants.YWindow);
            MatrixSize = new Size(Constants.CellsCountX, Constants.CellsCountY);
            ProcessSizeChanging(windowSize);
            DrawMap();
        }

        public void NextTick()
        {
            Image.Tick();
            DrawMap();
        }

        public void ProcessSizeChanging(Size newSize)
        {
            windowSize = newSize;
            MapSize = new Size((int)(Constants.FieldRatioX * newSize.Width), (int)(Constants.FieldRatioY * newSize.Height));
            Bitmap = new Bitmap((int)(Constants.FieldRatioX * newSize.Width), (int)(Constants.FieldRatioY * newSize.Height));
            cellSize = new Size(MapSize.Width / MatrixSize.Width, MapSize.Height / MatrixSize.Height);
        }

        void DrawMap()
        {
            Bitmap = new Bitmap((int)(Constants.FieldRatioX * windowSize.Width), (int)(Constants.FieldRatioY * windowSize.Height));
            IEnumerable<ICell> filledCells = Image.GetOccupiedCells();
            foreach (ICell currentCell in filledCells)
                DrawCell(currentCell);
        }

        void DrawCell(ICell cell)
        {
            Point upperLeft = new Point(cellSize.Width * cell.Position.X, cellSize.Height * cell.Position.Y);
            for (int i = upperLeft.Y; i < upperLeft.Y + cellSize.Height; i++)
                for (int j = upperLeft.X; j <upperLeft.X + cellSize.Width; j++)
                    Bitmap.SetPixel(j, i, cell.Color);
            if (Math.Min(cellSize.Width,cellSize.Height) < 5)
                return;
            for (int i = upperLeft.Y; i < upperLeft.Y + cellSize.Height; i++)
            {
                Bitmap.SetPixel(upperLeft.X,i, Color.Black);
                Bitmap.SetPixel(upperLeft.X + cellSize.Width - 1,i, Color.Black);
            }
            for (int i = upperLeft.X; i < upperLeft.X + cellSize.Width; i++)
            {
                Bitmap.SetPixel(i, upperLeft.Y, Color.Black);
                Bitmap.SetPixel(i, upperLeft.Y + cellSize.Height - 1, Color.Black);
            }
        }
        #endregion
    */}
}