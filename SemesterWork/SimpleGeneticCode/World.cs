using System;
using System.Drawing;
using System.Collections.Generic;

namespace SimpleGeneticCode
{
    public class World
    {
        public ICell[,] Cells { get; private set; }
        public Size Size { get; }
        public List<Bot> Bots { get; private set; }
        public int BotsCount { get { return Bots.Count; } }

        Random random;

        public World(int width, int height, int botsCount)
        {
            Bots = new List<Bot>();
            random = new Random();
            Size = new Size(width, height);
            Cells = new ICell[height, width];
            AddRandomBots(botsCount);
        }

        public World(Size size, int botsCount) : this(size.Width, size.Height, botsCount) { }

        public void Tick()
        {
            foreach (Bot currentBot in Bots)
                currentBot.Action();
        }

        public void AddRandomBots(int count)
        {
            while (Bots.Count != Size.Width * Size.Height && count != 0)
            {
                Point position = new Point();
                do
                {
                    position.X = random.Next(0, Size.Width);
                    position.Y = random.Next(0, Size.Height);
                }
                while (Cells[position.Y, position.X] is Bot);
                Bot bot = new Bot(BotsCount + 1, position, this);
                Bots.Add(bot);
            }
        }
    }
}