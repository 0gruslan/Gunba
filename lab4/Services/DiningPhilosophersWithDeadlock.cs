using System;
using System.Threading;

namespace Lab4
{
    public class DiningPhilosophersWithDeadlock
    {
        private const int PhilosopherCount = 5;
        private readonly object[] _forks;
        private readonly Thread[] _philosophers;

        public DiningPhilosophersWithDeadlock()
        {
            _forks = new object[PhilosopherCount];
            _philosophers = new Thread[PhilosopherCount];

            for (int i = 0; i < PhilosopherCount; i++)
            {
                _forks[i] = new object();
            }
        }

        public void Start()
        {
            Console.WriteLine("=== Обедающие философы С DEADLOCK ===");
            Console.WriteLine();

            for (int i = 0; i < PhilosopherCount; i++)
            {
                int philosopherId = i;
                _philosophers[i] = new Thread(() => PhilosopherLife(philosopherId))
                {
                    Name = $"Философ {philosopherId + 1}"
                };
                _philosophers[i].Start();
            }

            Thread.Sleep(10000);

            Console.WriteLine("\nОстановка демонстрации...");
        }

        private void PhilosopherLife(int id)
        {
            int leftFork = id;
            int rightFork = (id + 1) % PhilosopherCount;

            while (true)
            {
                Think(id);

                lock (_forks[leftFork])
                {
                    Console.WriteLine($"Философ {id + 1} взял левую вилку {leftFork + 1}");
                    Thread.Sleep(100);

                    lock (_forks[rightFork])
                    {
                        Console.WriteLine($"Философ {id + 1} взял правую вилку {rightFork + 1}");
                        Eat(id);
                        Console.WriteLine($"Философ {id + 1} положил обе вилки");
                    }
                }
            }
        }

        private void Think(int id)
        {
            Console.WriteLine($"Философ {id + 1} думает...");
            Thread.Sleep(new Random().Next(500, 1500));
        }

        private void Eat(int id)
        {
            Console.WriteLine($"Философ {id + 1} ЕСТ (использует вилки {id + 1} и {((id + 1) % 5) + 1})");
            Thread.Sleep(new Random().Next(500, 1000));
        }
    }
}

