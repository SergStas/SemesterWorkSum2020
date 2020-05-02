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
    }
}