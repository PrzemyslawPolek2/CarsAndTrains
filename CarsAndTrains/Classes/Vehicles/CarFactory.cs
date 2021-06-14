using System;

namespace CarsAndTrains.Classes.Vehicles
{
    public static class CarFactory
    {
        private const double SPEED_MAX_LIMIT = 3.0f;
        private const double SPEED_MIN_LIMIT = 1.5f;

        private const double DEATH_MAX_LIMIT = 20.0f;
        private const double DEATH_MIN_LIMIT = 10.0f;

        private static Random random;

        public static void Initialize()
        {
            random = new Random();
        }

        public static Car Create(int nodesNumber, int NextVehicleIndex)
        {
            Car car = new Car(RandomSpeedGenerator(), nodesNumber, RandomDeathGenerator(), NextVehicleIndex);
            return car;
        }

        private static double RandomSpeedGenerator()
        {
            return (random.NextDouble() * SPEED_MAX_LIMIT) + SPEED_MIN_LIMIT;
        }
        private static double RandomDeathGenerator()
        {
            return (random.NextDouble() * DEATH_MAX_LIMIT) + DEATH_MIN_LIMIT;
        }
    }
}
