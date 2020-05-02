using System;
using System.Collections;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Controls;
using Color = System.Windows.Media.Color;

namespace SimpleGeneticCode
{
    public static class BasicCommands
    {
        static Action<Bot> checkSun = b =>
        {
            b.Program.CommandPointer += b.Environment.GetSunEnergy(b.Position) < Configurations.MaxSunEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        static Action<Bot> checkEnergy = b =>
        {
            b.Program.CommandPointer += b.EnergyReserve < Configurations.MaxBotEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        static Action<Bot> checkMinerals = b =>
        {
            b.Program.CommandPointer +=
                b.Environment.GetMineralsEnergy(b.Position) < Configurations.MaxMineralsEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        private static Action<Bot> getEnergyFromSun = b =>
        {
            b.EnergyReserve += b.Environment.GetSunEnergy(b.Position);
            b.Environment.AtmosphereThickness += Configurations.AtmospherePerPhotosynthesis;
            b.Program.CommandPointer++;
            b.ChangeColor(Color.FromRgb(0, 255, 0));
        };

        private static Action<Bot> getEnergyFromMinerals = b =>
        {
            b.EnergyReserve += b.Environment.GetMineralsEnergy(b.Position);
            b.Program.CommandPointer++;
            b.ChangeColor(Color.FromRgb(0, 0, 255));
        };

        private static Dictionary<string, Action<Bot>> commands = new Dictionary<string, Action<Bot>>();
        /*{
            { "Look Up", CheckCell(0, -1) },
            { "Look Left", CheckCell(-1, 0) },
            { "Look Down", CheckCell(0, 1) },
            { "Look Right", CheckCell(1, 0) },
            { "Look Up-Right", CheckCell(1, -1) },
            { "Look Up-Left", CheckCell(-1, -1) },
            { "Look Down-Left", CheckCell(-1, 1) },
            { "Look Down-Right", CheckCell(1, 1) },
            { "Charge Up", Catch(0, -1) },
            { "Charge Left", Catch(-1, 0) },
            { "Charge Down", Catch(0, 1) },
            { "Charge Right", Catch(1, 0) },
            { "Charge Up-Right", Catch(1, -1) },
            { "Charge Up-Left", Catch(-1, -1) },
            { "Charge Down-Left", Catch(-1, 1) },
            { "Charge Down-Right", Catch(1, 1) },
            { "Go Up", Move(0, -1) },
            { "Go Left", Move(1, 0) },
            { "Go Down", Move(0, 1) },
            { "Go Right", Move(-1, 0) },
            { "Go Up-Right", Move(1, -1) },
            { "Go Up-Left", Move(-1, 0-1) },
            { "Go Down-Left", Move(-1, 1) },
            { "Go Down-Right", Move(1, 1) },
            { "Expand Up", Reproduce(0, -1) },
            { "Expand Left", Reproduce(-1, 0) },
            { "Expand Down", Reproduce(0, 1) },
            { "Expand Right", Reproduce(1, 0) },
            { "Expand Up-Right", Reproduce(1, -1) },
            { "Expand Up-Left", Reproduce(-1, -1) },
            { "Expand Down-Left", Reproduce(-1, 1) },
            { "Expand Down-Right", Reproduce(1, 1) },
            { "Check Energy", checkEnergy },
            { "Check minerals", checkMinerals },
            { "Check sun", checkSun },
            { "Absorb minerals", getEnergyFromMinerals },
            { "Photosynthesize", getEnergyFromSun }
        };*/

        public static Dictionary<string, Action<Bot>> GetBasicCommands()
        {
            //return new Point(0, 0).GetNeighbours().SelectMany(x => new[]
            //        {CheckCell(x.X, x.Y), Catch(x.X, x.Y), Move(x.X, x.Y), Reproduce(x.X, x.Y)})
            //   .Concat(new[] { checkEnergy, checkMinerals, checkSun, getEnergyFromMinerals, getEnergyFromSun});
            FillDictionary();
            return commands;
        }

        static void FillDictionary()
        {
            string[] orientedNames = new[] {"Look", "Charge", "Go", "Expand", "Check relativity"};
                for (int i=0;i<orientedNames.Length;i++)
                    foreach (Point p in Point.Empty.GetRound())
                    {
                        Action<Bot> act = i == 0 ? CheckCell(p.X, p.Y) :
                            i == 1 ? Catch(p.X, p.Y) :
                            i == 2 ? Move(p.X, p.Y) :
                            i == 3 ? Reproduce(p.X, p.Y) :
                            CheckRelativity(p.X, p.Y);
                        commands.Add(orientedNames[i] + " " 
                            + (p.Y == -1 ? "U" : p.Y == 0 ? "" : "D")
                            + (p.X == -1 ? "L" : p.X == 0 ? "" : "R"), act);
                    }
            commands.Add("Check Energy", checkEnergy);
            commands.Add("Check minerals", checkMinerals);
            commands.Add("Check sun", checkSun);
            commands.Add("Absorb minerals", getEnergyFromMinerals);
            commands.Add("Photosynthesize", getEnergyFromSun);
        }

        static Action<Bot> CheckRelativity(int dx, int dy)
        {
            return b =>
            {
                b.Program.CommandPointer++;
                Point target = b.Position.Move(dx, dy);
                if (!b.Environment.InBounds(target, out bool shift) && !shift)
                    return;
                if (shift)
                    b.Environment.Shift(ref target);
                if (b.Environment.CellIsFree(target) || !(b.Environment[target] is Bot))
                    return;
                b.Program.CommandPointer += ((Bot)b.Environment[target]).Program.GetComparisonDelta(b.Program) / 4;
                CallNextCommand(b);
            };
        }

        static Action<Bot> Move(int dx, int dy)
        {
            return b => 
            {
                b.Program.CommandPointer++; 
                b.Move(dx, dy); 
            };
        }

        static Action<Bot> Reproduce(int dx, int dy)
        {
            return b =>
            {
                b.Reproduce(dx, dy, out bool successfully);
                b.Program.CommandPointer++;
            };
        }

        static Action<Bot> CheckCell(int dx, int dy)
        {
            return b =>
            {
                Point newPos = b.Position.Move(dx, dy);
                b.Program.CommandPointer++;
                if (!b.Environment.InBounds(newPos, out bool shift) && !shift)
                    return;
                if (shift)
                    b.Environment.Shift(ref newPos);
                int increment = 1;
                ICell target = b.Environment[newPos];
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