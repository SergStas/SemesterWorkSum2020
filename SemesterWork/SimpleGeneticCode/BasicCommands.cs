using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SimpleGeneticCode
{
    public static class BasicCommands
    {
        public static List<Action<Bot>> BasicCommandsList;

        public static void FillList()
        {
            int[] d = new int[] { -1, 0, 1 };
            IEnumerable<Point> pts = d.SelectMany(x => d.Select(a => new Point(x, a)))
                .Where(a => !(a.X == 0 && a.Y == 0));
            BasicCommandsList = pts.SelectMany(x => new Action<Bot>[] { Check(x.X, x.Y), Catch(x.X, x.Y), Move(x.X, x.Y) }).ToList();
        }

        static Action<Bot> Move(int dx, int dy)
        {
            return b => 
            { 
                b.Program.CommandPointer++; 
                b.Environment.MoveCell(b.Position, dx, dy); 
            };
        }

        static Action<Bot> Check(int dx, int dy)
        {
            return b =>
            {
                b.Program.CommandPointer++;
                if (!b.Environment.InBounds(b.Position.X + dx, b.Position.Y + dy))
                    return;
                int increment = 0;
                ICell target = b.Environment.Cells[b.Position.Y + dy, b.Position.X + dx];
                if (target is Food) increment = 1;
                else if (target is Bot) increment = 2;
                b.Program.CommandPointer += increment;
            };
        }

        static Action<Bot> Catch(int dx, int dy)
        {
            return b =>
            {
                b.Eat(dx, dy);
                b.Program.CommandPointer++;
            };
        }
    }
}