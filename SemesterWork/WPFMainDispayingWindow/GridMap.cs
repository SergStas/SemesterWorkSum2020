using System.Windows;
using System.Windows.Controls;
using SimpleGeneticCode;
using System.Collections.Generic;
using Size = System.Drawing.Size;

namespace WPFMainDisplayingWindow
{
    public class GridMap
    {
        public Grid Map { get; private set; }
        public Size Size { get; }
        public World World { get; }

        public GridMap(World world)
        {
            World = world;
            Size = world.Size;
            Map = new Grid();
            Map.ShowGridLines = false;
            FillGrid();
            DrawMap();
        }

        public void NextTick()
        {
            World.Tick();
            DrawMap();
        }

        void FillGrid()
        {
            for (int i = 0; i < Size.Height; i++)
                Map.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int j = 0; j < Size.Width; j++)
                Map.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        void DrawMap()
        {
            Map.Children.Clear();
            IEnumerable<ICell> filledCells = World.GetOccupiedCells();
            foreach (ICell currentCell in filledCells)
                DrawCell(currentCell);
        }

        void DrawCell(ICell cell)
        {
            Map.Children.Add(cell.Graphics);
            Grid.SetColumn(cell.Graphics, cell.Position.X);
            Grid.SetRow(cell.Graphics, cell.Position.Y);
        }
    }
}