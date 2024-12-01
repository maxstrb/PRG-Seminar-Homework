using System.Collections.Generic;

namespace PisekSimulace{
    public class Program
    {
        public static void Main()
        {
            Solver s = new();
            int time = s.Solve(3000, [new(60, 5, 120, 15, "1"), new(60, 5, 120, 15, "2"), new(60, 5, 120, 15, "3")]);

            Console.WriteLine(time);
        }
    }

    class Solver
    {    
        public int Solve(int sandAmount, Car[] cars)
        {
            PriorityQueue<Car, int> eventCalendar = new();
            foreach (Car car in cars)
            {
                eventCalendar.Enqueue(car, 0);
            }

            Car currentCar = cars[0];
            int currentTime = 0;

            while (eventCalendar.Count > 0 && sandAmount > 0)
            {
                eventCalendar.TryDequeue(out currentCar, out currentTime);

                List<Car> currentCars = [currentCar];

                Car peekedCar;
                int peekedTime = int.MaxValue;

                while (true)
                {
                    if (!eventCalendar.TryPeek(out peekedCar, out peekedTime)) break;
                    if (peekedTime > currentTime) break;

                    if (peekedTime == currentTime)
                    {
                        currentCars.Add(peekedCar);
                        eventCalendar.Dequeue();
                    }
                }

                currentCar = ChooseCurrentCar(currentCars);

                Console.WriteLine(currentCar.name);
                Console.WriteLine(currentTime);

                currentCars.Remove(currentCar);
                currentCars.ForEach(car => { eventCalendar.Enqueue(car, currentTime + currentCar.LoadTime); });

                while (true)
                {
                    if (!eventCalendar.TryPeek(out peekedCar, out peekedTime)) break;
                    if (peekedTime >= currentTime+currentCar.LoadTime) break;

                    if (peekedTime < currentTime + currentCar.LoadTime)
                    {
                        eventCalendar.Dequeue();

                        eventCalendar.Enqueue(peekedCar, currentTime + currentCar.LoadTime);
                    }
                }

                sandAmount -= currentCar.Capacity;

                eventCalendar.Enqueue(currentCar, currentCar.ReturnTime + currentCar.LoadTime + currentTime);
            }

            return currentCar.ReturnTime + currentCar.LoadTime + currentTime;
        }

        Car ChooseCurrentCar(List<Car> cars)
        {
            if (cars.Count == 0) throw new Exception("Co to děláš");
            if (cars.Count == 1) return cars[0];

            Car[] possible_output = FindSmallestElements([.. cars], (Car car) => -car.Capacity);
            if (possible_output.Length == 1) return possible_output[0];
            
            possible_output = FindSmallestElements(possible_output, (Car car) => car.TravelTime);
            if (possible_output.Length == 1) return possible_output[0];

            possible_output = FindSmallestElements(possible_output, (Car car) => car.LoadTime);
            if (possible_output.Length == 1) return possible_output[0];

            possible_output = FindSmallestElements(possible_output, (Car car) => car.UnloadTime);
            return possible_output[0];
        }

        static T[] FindSmallestElements<T>(T[] elements, Func<T, int> property)
        {
            if (elements.Length == 0) throw new Exception("Co to děláš");

            int smallest = property(elements[0]);
            int count = 0;

            foreach (T element in elements){
                int p = property(element);

                if (smallest == p){
                    count++;
                    continue;
                }

                if (smallest < p){
                    smallest = p;
                    count = 1;
                }
            }

            T[] output = new T[count];
            int i = 0;

            foreach (T element in elements){
                if (property(element) == smallest) {
                    output[i] = element;
                    i++;
                }
            }

            return output;
        }
    }

    struct Car(int loadTime, int unloadTime, int travelTime, int capacity, string name)
    {
        public readonly string name = name;
        public readonly int LoadTime { get; } = loadTime;
        public readonly int ReturnTime { get; } = travelTime * 2 + unloadTime;
        public readonly int TravelTime { get; } = travelTime;
        public readonly int UnloadTime { get; } = unloadTime;

        public readonly int Capacity { get; } = capacity;
    }
}