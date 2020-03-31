using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SimpleGeneticCode
{
    public static class BasicCommands
    {
        static Action<Bot> checkSun = b => { b.Program.CommandPointer += b.Environment.GetSunEnergy(b.Position) < Constants.MaxSunEnergy / 2 ? 1 : 2; CallNextCommand(b); };
        static Action<Bot> checkEnergy = b => { b.Program.CommandPointer += b.EnergyReserve < Constants.MaxBotEnergy / 2 ? 1 : 2; CallNextCommand(b); };
        static Action<Bot> checkMinerals = b => { b.Program.CommandPointer += b.Environment.GetMineralsEnergy(b.Position) < Constants.MaxMineralsEnergy / 2 ? 1 : 2; CallNextCommand(b); };
        static Action<Bot> getEnergyFromSun = b =>
        {
            b.EnergyReserve += b.Environment.GetSunEnergy(b.Position);
            b.Environment.AtmosphereThickness += Constants.AtmospherePerPhotosynthesis;
            b.Program.CommandPointer++;
        };
        static Action<Bot> getEnergyFromMinerals = b =>
        {
            b.EnergyReserve += b.Environment.GetMineralsEnergy(b.Position);
            b.Program.CommandPointer++;
        };

        public static IEnumerable<Action<Bot>> FillList()
        {
            int[] d = new int[] { -1, 0, 1 };
            IEnumerable<Point> pts = d.SelectMany(x => d.Select(a => new Point(x, a)))
                .Where(a => !(a.X == 0 && a.Y == 0));
            return pts.SelectMany(x => new [] { CheckCell(x.X, x.Y), Catch(x.X, x.Y), Move(x.X, x.Y), Reproduce(x.X,x.Y) })
                .Concat(new[] { checkEnergy, checkMinerals, checkSun, getEnergyFromMinerals, getEnergyFromSun });
        }

        static Action<Bot> Move(int dx, int dy)
        {
            return b => 
            {
                b.Program.CommandPointer++; 
                b.Environment.MoveCell(b.Position, dx, dy); 
            };
        }

        static Action<Bot> Reproduce(int dx, int dy)
        {
            return b =>
            {
                if (b.EnergyReserve < Constants.EnergyBorderValueForReproducing)
                    return;
                b.Program.CommandPointer++;
                Point target = new Point(b.Position.X + dx, b.Position.Y + dy);
                if (!b.Environment.InBounds(target))
                    return;
                BotProgram newProgram = b.Program.GetCopy(true);
                Bot newBot = new Bot(newProgram, b.Environment, target);
                b.Environment.AddCell(newBot);
                b.EnergyReserve -= Constants.ReproducingEnergyWaste;
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