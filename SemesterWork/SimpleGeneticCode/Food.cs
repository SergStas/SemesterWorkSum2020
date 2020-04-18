﻿using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace SimpleGeneticCode
{
    public class Food : ICell
    {
        public World Environment { get; }
        public Point Position { get; set; }
        public int EnergyReserve { get; private set; }
        public Color Color { get; set; }
        public Button Graphics { get; }

        public Food(Point position, int energy, World world)
        {
            Color = Color.FromRgb(227,125,0);
            Position = position;
            EnergyReserve = energy;
            Environment = world;
            Graphics = new Button() {Background = new SolidColorBrush(Color)};
        }

        public void Action()
        {
            Environment.AtmosphereThickness--;
            if (--EnergyReserve < 1)
                Remove();
        }

        public void Remove()
        {
            Environment.RemoveCell(this);
        }
    }
}