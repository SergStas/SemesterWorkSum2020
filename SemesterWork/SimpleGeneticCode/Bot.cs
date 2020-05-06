using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace SimpleGeneticCode
{
    public class Bot : ICell
    {
        public int Id { get; set; }
        public BotProgram Program { get; }
        public Point Position { get; set; }
        public World Environment { get; }
        public Color Color { get; set; }
        public int EnergyReserve 
        {
            get => energy; 
            set
            {
                energy = value;
                if (value > Configurations.MaxBotEnergy)
                {
                    energy = Configurations.MaxBotEnergy;
                    Autoreproduce();
                }
                else if (value < 1)
                    Remove();
            }
        }

        int energy;
        
        public Bot(World world)
        {
            Environment = world;
            Color = Color.FromRgb(0, 255, 0);
            EnergyReserve = Configurations.BotBeginningEnergy;
            Program = new BotProgram(this);
        }

        public Bot(Point pos, World world) : this(world) { Position = pos; }

        public Bot(Point position, World world, int startCommand) : this(position, world)
        {
            Program.FillWith(startCommand);
        }

        public Bot(BotProgram program, World world, Point pos)
        {
            EnergyReserve = Configurations.BotBeginningEnergy;
            Environment = world;
            Position = pos;
            Program = program;
            program.Owner = this;
            Color = Color.FromRgb(0, 255, 0);
        }

        public void Move(int dx, int dy)
        {
            Environment.MoveCell(Position, dx, dy);
        }

        public void Eat(int dx, int dy)
        {
            if (!Environment.InBounds(Position.X + dx, Position.Y + dy, out bool shift) && !shift)
                return;
            var e = Position.Move(dx, dy);
            Position = e;
            if (shift)
                Environment.Shift(this);
            ICell target = Environment[Position];
            if (target is null) return;
            EnergyReserve +=(int)(Configurations.DevouringBonus * target.EnergyReserve);
            Environment.RemoveCell(target);
            ChangeColor(Color.FromRgb(255, 0, 0));
        }

        public void Reproduce(int dx, int dy, out bool successfully)
        {
            successfully = false;
            if (EnergyReserve < Configurations.EnergyBorderValueForReproducing)
                return;
            Point target = new Point(Position.X + dx, Position.Y + dy);
            if (!Environment.InBounds(target, out bool shift) && !shift)
                return;
            if (shift)
                Environment.Shift(ref target);
            BotProgram newProgram = Program.GetCopy(true);
            Bot newBot = new Bot(newProgram, Environment, target);
            Environment.AddCell(newBot);
            EnergyReserve -= Configurations.ReproducingEnergyWaste;
            successfully = true;
        }

        public void Autoreproduce()
        {
            foreach (Point currentNeighbour in Position.GetNeighbours().ToList().Shuffle())
            {
                Reproduce(currentNeighbour.X - Position.X, currentNeighbour.Y - Position.Y, out bool success);
                if (success)
                    return;
            }
            Remove();
        }

        public void ChangeColor(Color rgbPart)
        {
            Color = rgbPart;
        }

        public void Action()
        {
            EnergyReserve -= Configurations.BotEnergyWaste;
            Program.IterationsCounter = 0;
            Program.Execute();
        }

        public void Remove()
        {
            Food food = new Food(Position, EnergyReserve < Configurations.DefaultFoodEnergy ? Configurations.DefaultFoodEnergy : EnergyReserve, Environment);
            Environment.RemoveCell(this);
            Environment.AddCell(food);
        }

        public override string ToString()
        {
            return $"Command: {Program.Current}; Energy: {EnergyReserve}";
        }
    }
}