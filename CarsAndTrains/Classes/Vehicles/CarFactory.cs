using System;

namespace CarsAndTrains.Classes.Vehicles
{
    /// <summary>
    /// Factory class that creates a <see cref="Car"></see> class instance
    /// </summary>
    public static class CarFactory
    {
        public const double SPEED_MAX_LIMIT = 2.0f;
        public const double SPEED_MIN_LIMIT = 1.2f;

        public const double DEATH_MAX_LIMIT = 20.0f;
        public const double DEATH_MIN_LIMIT = 10.0f;

        private static Random random;

        /// <summary>
        /// Initializes the Factory
        /// </summary>
        public static void Initialize()
        {
            random = new Random();
        }
        /// <summary>
        /// Creates a new <see cref="Car"/>
        /// </summary>
        /// <param name="nodesNumber">Number of nodes to travel</param>
        /// <param name="nextVehicleIndex">Index in the Pool of the next Car</param>
        /// <returns></returns>
        public static Car Create(int nodesNumber, int nextVehicleIndex)
        {
            Car _car = new Car(RandomSpeedGenerator(), nodesNumber, RandomDeathGenerator(), nextVehicleIndex);
            return _car;
        }


        /// <summary>
        /// Generates Random Speed between SPEED_MAX_LIMIT and SPEED_MIN_LIMIT
        /// </summary>
        /// <returns><see cref="double"/> double </returns>
        public static double RandomSpeedGenerator()
        {
            return (random.NextDouble() * (SPEED_MAX_LIMIT - SPEED_MIN_LIMIT)) + SPEED_MIN_LIMIT;
        }
        /// <summary>
        /// Generates Random DeathTime between DEATH_MAX_LIMIT and DEATH_MIN_LIMIT
        /// </summary>
        /// <returns><see cref="double"/> value</returns>
        public static double RandomDeathGenerator()
        {
            return (random.NextDouble() * (DEATH_MAX_LIMIT - DEATH_MIN_LIMIT)) + DEATH_MIN_LIMIT;
        }
    }
}
