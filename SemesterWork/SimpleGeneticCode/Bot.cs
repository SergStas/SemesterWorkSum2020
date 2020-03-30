using System.Drawing;

namespace SimpleGeneticCode
{
    public class Bot : ICell
    {
        public int Id { get; set; }
        public BotProgram Program { get; private set; }
        public Point Position { get; private set; }
        public World Environment { get; } 
        public int EnergyReserve {
            get { return energy; } 
            private set
            {
                if (value > Constants.MaxBotEnergy)
                    energy = Constants.MaxBotEnergy;
                if (value < 1)
                    Remove();
            }
        }

        int energy;

        public Bot(World world)
        {
            EnergyReserve = Constants.BotBeginningEnergy;
            Environment = world;
            Program = new BotProgram(this);
        }

        public Bot(Point pos, World world) : this(world) { Position = pos; }

        public void Move(int dx, int dy)
        {
            Environment.MoveCell(Position, dx, dy);
        }

        public void Eat(int dx, int dy)
        {
            if (!Environment.InBounds(Position.X + dx, Position.Y + dy))
                return;
            ICell target = Environment.Cells[Position.Y + dy, Position.X + dx];
            if (target is null) return;
            EnergyReserve += target.EnergyReserve;
            Environment.RemoveCell(target);
        }

        public void Action()
        {
            EnergyReserve -= Constants.BotEnergyWaste;
            Program.IterationsCounter = 0;
            Program.Execute();
        }

        public void Remove()
        {
            Food food = new Food(Position, EnergyReserve < Constants.DefaultFoodEnergy ? Constants.DefaultFoodEnergy : EnergyReserve, Environment);
            Environment.RemoveCell(this);
            Environment.AddCell(food);
        }
    }
}