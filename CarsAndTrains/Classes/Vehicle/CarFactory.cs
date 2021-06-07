using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes.Vehicle
{
    static class CarFactory
    {
        private const double SPEED_MAX_LIMIT = 2.0f;
        private const double SPEED_MIN_LIMIT = .4f;

        private const double DEATH_MAX_LIMIT = 20.0f;
        private const double DEATH_MIN_LIMIT = 10.0f;

        public static Car CreateCar(int nodesNumber, int NextVehicleIndex)
        {
            Car car = new Car(RandomSpeedGenerator(),nodesNumber, RandomDeathGenerator(), NextVehicleIndex);
            return car;
        }

        private static double RandomSpeedGenerator()
        {
            return (new Random().NextDouble() * SPEED_MAX_LIMIT) + SPEED_MIN_LIMIT;
        }
        private static double RandomDeathGenerator()
        {
            return (new Random().NextDouble() * DEATH_MAX_LIMIT) + DEATH_MIN_LIMIT;
        }
    }
}
