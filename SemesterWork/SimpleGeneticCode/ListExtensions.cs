using System;
using System.Collections.Generic;

namespace SimpleGeneticCode
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random rnd = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int k = rnd.Next(0, i);
                T value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
            return list;
        }
    }
}