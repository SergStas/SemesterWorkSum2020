using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SimpleGeneticCode
{
    public static class BasicCommands
    {
        public static List<Action<Bot>> BasicCommandsList { get; private set; }

        static Action<Bot> checkSun = b => { b.Program.CommandPointer += b.Environment.GetSunEnergy(b.Position) < Constants.MaxSunEnergy / 2 ? 1 : 2; CallNextCommand(b); };
        static Action<Bot> checkEnergy = b => { b.Program.CommandPointer += b.EnergyReserve < Constants.MaxBotEnergy / 2 ? 1 : 2; CallNextCommand(b); };
        static Action<Bot> checkMinerals = b => { b.Program.CommandPointer += b.Environment.GetMineralsEnergy(b.Position) < Constants.MaxMineralsEnergy / 2 ? 1 : 2; CallNextCommand(b); };

        public static void FillList()
        {
            int[] d = new int[] { -1, 0, 1 };
            IEnumerable<Point> pts = d.SelectMany(x => d.Select(a => new Point(x, a)))
                .Where(a => !(a.X == 0 && a.Y == 0));
            BasicCommandsList = pts.SelectMany(x => new Action<Bot>[] { CheckCell(x.X, x.Y), Catch(x.X, x.Y), Move(x.X, x.Y) })
                .Concat(new[] { checkEnergy, checkMinerals, checkSun }).ToList();
        }

        static Action<Bot> Move(int dx, int dy)
        {
            return b => 
            { 
                b.Program.CommandPointer++; 
                b.Environment.MoveCell(b.Position, dx, dy); 
            };
        }

        static Action<Bot> CheckCell(int dx, int dy)
        {
            return b =>
            {
                b.Program.CommandPointer++;
                if (!b.Environment.InBounds(b.Position.X + dx, b.Position.Y + dy))
                    return;
                int increment = 1;
                ICell target = b.Environment.Cells[b.Position.Y + dy, b.Position.X + dx];
                if (target is Food) increment = 2;
                else if (target is Bot) increment = 3;
                b.Program.CommandPointer += increment;
                CallNextCommand(b);
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

        static void CallNextCommand(Bot b)
        {
            b.Program.IterationsCounter++;
            b.Program.Execute();
        }
    }
}