using System.Runtime.InteropServices;

namespace SimpleGeneticCode
{
    public static class Constants
    {
        public const int OriginAtmosphereThickness = 100;
        public const int MaxThickness = 1000;
        public const int MaxSunEnergy = 150;
        public const int MaxMineralsEnergy = 25;
        public const int BotBeginningEnergy = 250;
        public const int MaxBotEnergy = 500;
        public const int DefaultFoodEnergy = 20;
        public const int BotEnergyWaste = 10;
        public const int IterationsMaxCount = 10;
        public const int AtmospherePerPhotosynthesis = 1;
        public const int AtmosphereRegenerationPerFood = 2;
        public const int EnergyBorderValueForReproducing = 350;
        public const int ReproducingEnergyWaste = 50;
        public const int BotsStartCount = 100;
        public const int MaxMutationsCount = 20;
        public const int StartCommand = 20;
        public const double MutationChance = 0.5;
        public const double DevouringBonus = 1.25;
        public const bool BeginWithRandomProgram = true;
    }
}