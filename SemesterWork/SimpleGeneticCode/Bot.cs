using System.Drawing;

namespace SimpleGeneticCode
{
    public class Bot : ICell
    {
        public int Id { get; set; }
        public BotProgram Program { get; private set; }
        public Point Position { get; private set; }
        public World Environment { get; } 
        public int Energy { get; private set; }

        public Bot(World world)
        {
            Energy = Constants.BotBeginningEnergy;
            Environment = world;
            Program = new BotProgram(this);
        }

        public Bot( Point pos, World world) : this(world)
        {
            Position = pos;
        }

        public void Action()
        {
            Program.Execute();
            if (Energy > Constants.MaxBotEnergy)
                Energy = Constants.MaxBotEnergy;
            if (Energy < 1)
                Remove();
        }

        public void Remove()
        {
            Food food = new Food(Position, Energy < Constants.DefaultFoodEnergy ? Constants.DefaultFoodEnergy : Energy, Environment);
            Environment.RemoveCell(this);
            Environment.AddCell(food);
        }
    }
}