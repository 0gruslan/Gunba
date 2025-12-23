using System;
using System.Threading;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Лабораторная работа 4: Синхронизация потоков\n");
            Console.WriteLine("Выберите задачу для демонстрации:");
            Console.WriteLine("1. Обедающие философы С deadlock");
            Console.WriteLine("2. Обедающие философы БЕЗ deadlock");
            Console.WriteLine("3. Спящий парикмахер");
            Console.WriteLine("4. Producer-Consumer (BlockingCollection)");
            Console.WriteLine("5. Producer-Consumer (SemaphoreSlim + lock)");
            Console.WriteLine("6. Все задачи последовательно");
            Console.Write("\nВведите номер (1-6): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var philosophersWithDeadlock = new DiningPhilosophersWithDeadlock();
                    philosophersWithDeadlock.Start();
                    break;

                case "2":
                    var philosophersWithoutDeadlock = new DiningPhilosophersWithoutDeadlock();
                    philosophersWithoutDeadlock.Start();
                    break;

                case "3":
                    var barber = new SleepingBarber(maxSeats: 5);
                    barber.Start();
                    break;

                case "4":
                    ProducerConsumer.DemonstrateBlockingCollection();
                    break;

                case "5":
                    ProducerConsumer.DemonstrateSemaphoreAndLock();
                    break;

                case "6":
                    RunAllDemonstrations();
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Запуск всех демонстраций...\n");
                    RunAllDemonstrations();
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void RunAllDemonstrations()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ДЕМОНСТРАЦИЯ 1: Обедающие философы С DEADLOCK");
            Console.WriteLine(new string('=', 60));
            var philosophersWithDeadlock = new DiningPhilosophersWithDeadlock();
            philosophersWithDeadlock.Start();
            Thread.Sleep(2000);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ДЕМОНСТРАЦИЯ 2: Обедающие философы БЕЗ DEADLOCK");
            Console.WriteLine(new string('=', 60));
            var philosophersWithoutDeadlock = new DiningPhilosophersWithoutDeadlock();
            philosophersWithoutDeadlock.Start();
            Thread.Sleep(2000);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ДЕМОНСТРАЦИЯ 3: Спящий парикмахер");
            Console.WriteLine(new string('=', 60));
            var barber = new SleepingBarber(maxSeats: 5);
            barber.Start();
            Thread.Sleep(2000);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ДЕМОНСТРАЦИЯ 4: Producer-Consumer (BlockingCollection)");
            Console.WriteLine(new string('=', 60));
            ProducerConsumer.DemonstrateBlockingCollection();
            Thread.Sleep(2000);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ДЕМОНСТРАЦИЯ 5: Producer-Consumer (SemaphoreSlim + lock)");
            Console.WriteLine(new string('=', 60));
            ProducerConsumer.DemonstrateSemaphoreAndLock();
        }
    }
}

