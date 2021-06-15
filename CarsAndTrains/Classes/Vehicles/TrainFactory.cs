using System;

namespace CarsAndTrains.Classes.Vehicles
{
    /// <summary>
    /// Factory class that creates a <see cref="Train"></see>  class instance
    /// </summary>
    public class TrainFactory
    {
        public const double DEATH_TIME = 100.0f;
        private const double SPEED_MAX_LIMIT = 3.5f;
        private const double SPEED_MIN_LIMIT = 3.5f;


        private static Random random;

        /// <summary>
        /// Initializes the Factory
        /// </summary>
        public static void Initialize()
        {
            random = new Random();
        }
        /// <summary>
        /// Creates a new <see cref="Train"/>
        /// </summary>
        /// <param name="nodesNumber">Number of nodes to travel</param>
        /// <param name="nextVehicleIndex">Index in the Pool of the next Car</param>
        /// <returns></returns>
        public static Train Create(int nodesCount, int nextVehicleIndex)
        {
            Train train = new Train(RandomSpeedGenerator(), nodesCount, nextVehicleIndex, DEATH_TIME);
            return train;
        }
        /// <summary>
        /// Generates Random Speed between SPEED_MAX_LIMIT and SPEED_MIN_LIMIT
        /// </summary>
        /// <returns><see cref="double"/> double </returns>
        private static double RandomSpeedGenerator()
        {
            return (random.NextDouble() * (SPEED_MAX_LIMIT - SPEED_MIN_LIMIT)) + SPEED_MIN_LIMIT;
        }

    }
}
