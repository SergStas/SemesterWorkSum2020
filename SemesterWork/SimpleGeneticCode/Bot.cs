using System.Drawing;

namespace SimpleGeneticCode
{
    public class Bot : ICell
    {
        public int Id { get; }
        public Point Position { get; private set; }
        public BotProgram Program { get; private set; }
        public World Environment { get; } 

        public Bot(int id, World world)
        {
            Id = id;
            Environment = world;
            Program = new BotProgram(this);
        }

        public Bot(int id, Point pos, World world) : this(id, world)
        {
            Position = pos;
        }

        public void Action()
        {
            Program.Execute();
        }
    }
}