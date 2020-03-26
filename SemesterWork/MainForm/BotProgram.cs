using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MainForm
{
    class BotProgram
    {
        public const int Size = 64;
        public int[] Programs { get; private set; }

        Random random;

        static Dictionary<int, Action<Bot>> commands = new Dictionary<int, Action<Bot>>();

        public BotProgram()
        {
            random = new Random();
            Programs = Programs.Select(x => random.Next(0, Size)).ToArray();
        }

        public static void RegisterCommand(int number, Action<Bot> action)
        {
            commands.Add(number, action);
        }

    }
}
