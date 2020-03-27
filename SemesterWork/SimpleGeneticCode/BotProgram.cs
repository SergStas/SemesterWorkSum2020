using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGeneticCode
{
    public class BotProgram
    {
        public const int Size = 64;
        public Bot Owner { get; }
        public int[] Programs { get; private set; }
        public int Current { get { return Programs[commandPointer]; } }

        Random random;
        int commandPointer;

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
            if (commands.ContainsKey(Current))
                commands[Current](Owner);
            else
                Jump(Current);
        }

        void Jump(int n)
        {
            commandPointer = (commandPointer + n) % Size;
        }
    }
}