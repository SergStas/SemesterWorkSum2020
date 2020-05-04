using System;
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

         Func<ICell, GameWidgetAssembler, Button> visualizer;
         GameWidgetAssembler assembler;

        public GridMap(World world, Func<ICell, GameWidgetAssembler, Button> vis, GameWidgetAssembler wa)
        {
            World = world;
            visualizer = vis;
            assembler = wa;
            Size = world.Size;
            Map = new Grid();
            Map.ShowGridLines = false;
            SetGrid();
            DrawMap();
        }

        public void NextTick()
        {
            World.Tick();
            DrawMap();
        }

        void SetGrid()
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
            Button widget = visualizer(cell, assembler);
            Map.Children.Add(widget);
            Grid.SetColumn(widget, cell.Position.X);
            Grid.SetRow(widget, cell.Position.Y);
        }
    }
}