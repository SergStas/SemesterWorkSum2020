using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGeneticCode
{
    public class BotProgram
    {
        public const int Size = 64;
        public Bot Owner { get; }
        public int IterationsCounter { get; set; }
        public int[] Programs { get; private set; }
        public int Current { get { return Programs[CommandPointer]; } }
        public int CommandPointer
        {
            get { return pointer; } set
            {
                pointer = (value + Size) % Size;
            }
        }

        int pointer;
        Random random;

        static Dictionary<int, Action<Bot>> commands = new Dictionary<int, Action<Bot>>();

        public BotProgram(Bot owner)
        {
            Owner = owner;
            random = new Random();
            Programs = Programs.Select(x => random.Next(0, Size)).ToArray();
        }

        public static void RegisterCommand(int number, Action<Bot> action)
        {
            commands.Add(number, action);
        }

        public void Execute()
        {
            if (IterationsCounter >= Constants.IterationsMaxCount)
                return;
            if (commands.ContainsKey(Current))
                commands[Current](Owner);
            else
                Jump(Current);
        }

        void Jump(int n)
        {
            CommandPointer += n;
            IterationsCounter++;
            Execute();
        }
    }
}