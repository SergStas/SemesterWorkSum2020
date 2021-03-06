﻿using System.Drawing;
using System.Windows.Controls;
using Color = System.Windows.Media.Color;

namespace SimpleGeneticCode
{
    public interface ICell
    {
        public Point Position { get; set; }
        public int EnergyReserve { get; }
        public World Environment { get; set; }
        public Color Color { get; }
        public void Action();
        public void Remove();
    }
}