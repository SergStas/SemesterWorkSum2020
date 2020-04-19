using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGeneticCode
{
    public class BotProgram
    {
        public const int Size = 64;

        public Bot Owner { get; set; }
        public int IterationsCounter { get; set; }
        public int[] Programs { get; private set; }
        public int Current { get { return Programs[CommandPointer]; } }
        public int CommandPointer
        {
            get { return pointer; } 
            set
            {
                pointer = (value + Size) % Size;
            }
        }

        int pointer;
        Random random;

        static Dictionary<int, Action<Bot>> commands = new Dictionary<int, Action<Bot>>();

        public BotProgram()
        {
            random = new Random();
            Programs = new int[Size];
            Programs = Programs.Select(x => random.Next(0, Size)).ToArray();
        }

        public BotProgram(Bot owner) : this() { Owner = owner; }

        public BotProgram GetCopy(bool enableMutation)
        {
            double mutation = random.NextDouble();
            BotProgram result = new BotProgram();
            for (int i = 0; i < Size; i++)
                result.Programs[i] = Programs[i];
            if (!enableMutation || mutation > Constants.MutationChance)
                return result;
            int index = random.Next(0, Size);
            result.Programs[index] = random.Next(0, Size);
            return result;
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

        public static void UploadCommands(IEnumerable<Action<Bot>> collection)
        {
            int index = 0;
            foreach (Action<Bot> act in collection)
                RegisterCommand(index++, act);
        }

        public static void RegisterCommand(int number, Action<Bot> action)
        {
            commands.Add(number, action);
        }

        void Jump(int n)
        {
            CommandPointer += n;
            IterationsCounter++;
            Execute();
        }
    }
}