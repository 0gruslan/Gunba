using System;
using System.Threading;

namespace Lab4
{
    public class DiningPhilosophersWithoutDeadlock
    {
        private const int PhilosopherCount = 5;
        private readonly SemaphoreSlim[] _forks;
        private readonly SemaphoreSlim _tableSemaphore;
        private readonly Thread[] _philosophers;

        public DiningPhilosophersWithoutDeadlock()
        {
            _forks = new SemaphoreSlim[PhilosopherCount];
            _tableSemaphore = new SemaphoreSlim(PhilosopherCount - 1, PhilosopherCount - 1);
            _philosophers = new Thread[PhilosopherCount];

            for (int i = 0; i < PhilosopherCount; i++)
            {
                _forks[i] = new SemaphoreSlim(1, 1);
            }
        }

        public void Start()
        {
            Console.WriteLine("=== Обедающие философы БЕЗ DEADLOCK ===");
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

            Thread.Sleep(15000);

            Console.WriteLine("\nОстановка демонстрации...");
        }

        private void PhilosopherLife(int id)
        {
            int leftFork = id;
            int rightFork = (id + 1) % PhilosopherCount;

            while (true)
            {
                Think(id);

                _tableSemaphore.Wait();

                try
                {
                    _forks[leftFork].Wait();
                    Console.WriteLine($"Философ {id + 1} взял левую вилку {leftFork + 1}");

                    _forks[rightFork].Wait();
                    Console.WriteLine($"Философ {id + 1} взял правую вилку {rightFork + 1}");

                    Eat(id);

                    _forks[rightFork].Release();
                    Console.WriteLine($"Философ {id + 1} положил правую вилку {rightFork + 1}");

                    _forks[leftFork].Release();
                    Console.WriteLine($"Философ {id + 1} положил левую вилку {leftFork + 1}");
                }
                finally
                {
                    _tableSemaphore.Release();
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

