using System;

namespace CarsAndTrains.Classes.Vehicles
{
    public class TrainFactory
    {
        public const double DEATH_TIME = 10.0f;
        private const double SPEED_MAX_LIMIT = 3.5f;
        private const double SPEED_MIN_LIMIT = 3.5f;


        private static Random random;

        public static void Initialize()
        {
            random = new Random();
        }

        public static Train Create(int nodesCount, int nextVehicleIndex)
        {
            Train train = new Train(RandomSpeedGenerator(), nodesCount, nextVehicleIndex);
            return train;
        }

        private static double RandomSpeedGenerator()
        {
            return (random.NextDouble() * (SPEED_MAX_LIMIT- SPEED_MIN_LIMIT)) + SPEED_MIN_LIMIT;
        }

    }
}
