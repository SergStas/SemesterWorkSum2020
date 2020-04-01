using System.Drawing;

namespace SimpleGeneticCode
{
    public class Bot : ICell
    {
        public int Id { get; set; }
        public BotProgram Program { get; private set; }
        public Point Position { get; set; }
        public World Environment { get; } 
        public int EnergyReserve 
        {
            get { return energy; } 
            set
            {
                if (value > Constants.MaxBotEnergy)
                {
                    energy = Constants.MaxBotEnergy;
                    Autoreproduce();
                }
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

        public Bot(BotProgram program, World world, Point pos)
        {
            EnergyReserve = Constants.BotBeginningEnergy;
            Environment = world;
            Position = pos;
            Program = program;
            program.Owner = this;
        }

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

        public void Reproduce(int dx, int dy, out bool successfully)
        {
            successfully = false;
            if (EnergyReserve < Constants.EnergyBorderValueForReproducing)
                return;
            Point target = new Point(Position.X + dx, Position.Y + dy);
            if (!Environment.InBounds(target))
                return;
            BotProgram newProgram = Program.GetCopy(true);
            Bot newBot = new Bot(newProgram, Environment, target);
            Environment.AddCell(newBot);
            EnergyReserve -= Constants.ReproducingEnergyWaste;
            successfully = true;
        }

        public void Autoreproduce()
        {
            bool success = false;
            foreach (Point currentNeighbour in Position.GetNeighbours())
            {
                Reproduce(currentNeighbour.X, currentNeighbour.Y, out success);
                if (success)
                    return;
            }
            Remove();
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