﻿using System;
using System.Collections;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using Color = System.Windows.Media.Color;

namespace SimpleGeneticCode
{
    public static class BasicCommands
    {
        private static Action<Bot> checkSun = b =>
        {
            b.Program.CommandPointer += b.Environment.GetSunEnergy(b.Position) < Constants.MaxSunEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        private static Action<Bot> checkEnergy = b =>
        {
            b.Program.CommandPointer += b.EnergyReserve < Constants.MaxBotEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        private static Action<Bot> checkMinerals = b =>
        {
            b.Program.CommandPointer +=
                b.Environment.GetMineralsEnergy(b.Position) < Constants.MaxMineralsEnergy / 2 ? 1 : 2;
            CallNextCommand(b);
        };

        private static Action<Bot> getEnergyFromSun = b =>
        {
            b.EnergyReserve += b.Environment.GetSunEnergy(b.Position);
            b.Environment.AtmosphereThickness += Constants.AtmospherePerPhotosynthesis;
            b.Program.CommandPointer++;
            b.ChangeColor(Color.FromRgb(0, 255, 0));
        };

        private static Action<Bot> getEnergyFromMinerals = b =>
        {
            b.EnergyReserve += b.Environment.GetMineralsEnergy(b.Position);
            b.Program.CommandPointer++;
            b.ChangeColor(Color.FromRgb(0, 0, 255));
        };

        private static Dictionary<string, Action<Bot>> commands = new Dictionary<string, Action<Bot>>
        {
            { "Look Up", CheckCell(0, -1) },
            { "Look Left", CheckCell(1, 0) },
            { "Look Down", CheckCell(0, 1) },
            { "Look Right", CheckCell(-1, 0) },
            { "Charge Up", Catch(0, -1) },
            { "Charge Left", Catch(1, 0) },
            { "Charge Down", Catch(0, 1) },
            { "Charge Right", Catch(-1, 0) },
            { "Go Up", Move(0, -1) },
            { "Go Left", Move(1, 0) },
            { "Go Down", Move(0, 1) },
            { "Go Right", Move(-1, 0) },
            { "Expand Up", Reproduce(0, -1) },
            { "Expand Left", Reproduce(1, 0) },
            { "Expand Down", Reproduce(0, 1) },
            { "Expand Right", Reproduce(-1, 0) },
            { "Check Energy", checkEnergy },
            { "Check minerals", checkMinerals },
            { "Check sun", checkSun },
            { "Photosynthesize", getEnergyFromMinerals },
            { "Absorb minerals", getEnergyFromSun }
        };

        public static Dictionary<string, Action<Bot>> GetBasicCommands()
        {
            //return new Point(0, 0).GetNeighbours().SelectMany(x => new[]
            //        {CheckCell(x.X, x.Y), Catch(x.X, x.Y), Move(x.X, x.Y), Reproduce(x.X, x.Y)})
            //   .Concat(new[] { checkEnergy, checkMinerals, checkSun, getEnergyFromMinerals, getEnergyFromSun});
            return commands;
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