using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MainForm
{
    class Bot
    {
        public int Id { get; }
        public Point Position { get; private set; }
        public BotProgram BotProgram { get; private set; }

        public Bot(int id)
        {
            Id = id;
            BotProgram = new BotProgram();
        }

        public Bot(int id, Point pos) : this(id)
        {
            Position = pos;
        }
    }
}
