using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SimpleGeneticCode
{
    public static class Configurations
    {
        public static int OriginAtmosphereThickness = 100;
        public static int MaxThickness = 1000;
        public static int MaxSunEnergy = 150;
        public static int MaxMineralsEnergy = 25;
        public static int BotBeginningEnergy = 250;
        public static int MaxBotEnergy = 500;
        public static int DefaultFoodEnergy = 20;
        public static int BotEnergyWaste = 10;
        public static int IterationsMaxCount = 10;
        public static int AtmospherePerPhotosynthesis = 1;
        public static int AtmosphereRegenerationPerFood = 2;
        public static int EnergyBorderValueForReproducing = 350;
        public static int ReproducingEnergyWaste = 50;
        public static int BotsStartCount = 100;
        public static int MaxMutationsCount = 20;
        public static int StartCommand = 44;
        public static double MutationChance = 0.5;
        public static double DevouringBonus = 1.25;
        public static int BeginWithRandomProgram = 1;

        public static readonly Dictionary<string, int> MaxValues = new Dictionary<string, int>
        {
            {"OriginAtmosphereThickness", 5000},
            {"MaxThickness", 5000},
            {"MaxSunEnergy", 1000},
            {"MaxMineralsEnergy", 500},
            {"BotBeginningEnergy", 1000},
            {"MaxBotEnergy", 1000},
            {"DefaultFoodEnergy", 1000},
            {"BotEnergyWaste", 1000},
            {"IterationsMaxCount", 64},
            {"AtmospherePerPhotosynthesis", 5000},
            {"AtmosphereRegenerationPerFood", 5000},
            {"EnergyBorderValueForReproducing", 5000},
            {"ReproducingEnergyWaste", 1000},
            {"BotsStartCount", 1000},
            {"MaxMutationsCount", 64},
            {"StartCommand", 63},
            {"MutationChance", 1},
            {"DevouringBonus", 10},
            {"BeginWithRandomProgram", 1}
        };
    }
}