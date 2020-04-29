using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleGeneticCode
{
    public class BotProgram
    {
        public const int Size = 64;

        public Bot Owner { get; set; }
        public int IterationsCounter { get; set; }
        public int[] Programs { get; private set; }
        public int Current { get => Programs[CommandPointer];  }
        public string CommandName
        {
            get => names.ContainsKey(Programs[CommandPointer]) ? names[Programs[CommandPointer]] : $"Jump {Programs[CommandPointer]}";
        }
        public int CommandPointer
        {
            get => pointer; 
            set => pointer = (value + Size) % Size;
        }

        int pointer;
        Random random;

        static Dictionary<int, Action<Bot>> commands = new Dictionary<int, Action<Bot>>();
        static Dictionary<int, string> names = new Dictionary<int, string>();

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
            int count = random.Next(0, Constants.MaxMutationsCount);
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, Size);
                result.Programs[index] = random.Next(0, Size);
            }
            return result;
        }

        public void FillWith(int command)
        {
            for (int i = 0; i < Size; i++)
                Programs[i] = command;
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

        public string GetCommandsString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Size; i++) 
            {
                builder.Append(Programs[i] + "\t");
                if (i % 8 == 7)
                    builder.Append("\n");
            }
            return builder.ToString();
        }

        public static void UploadCommands(Dictionary<string, Action<Bot>> acts)
        {
            int index = 0;
            foreach (Action<Bot> act in acts.Values)
                RegisterCommand(index++, act);
            index = 0;
            foreach (string name in acts.Keys)
                names[index++] = name;
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