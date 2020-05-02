using System.Drawing;
using System.Windows;
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

        public Food(Point position, int energy, World world)
        {
            Color = Color.FromRgb(55,55,55);
            Position = position;
            EnergyReserve = energy;
            Environment = world;
        }

        public void Action()
        {
            Environment.AtmosphereThickness-=Configurations.AtmosphereRegenerationPerFood;
            if (--EnergyReserve < 1)
                Remove();
        }

        public void Remove()
        {
            Environment.RemoveCell(this);
        }
    }
}