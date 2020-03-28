using System.Drawing;

namespace SimpleGeneticCode
{
    public class Food : ICell
    {
        public World Environment { get; }
        public Point Position { get; }
        public int EnergyReserve { get; private set; }

        public Food(Point position, int energy, World world)
        {
            Position = position;
            EnergyReserve = energy;
            Environment = world;
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