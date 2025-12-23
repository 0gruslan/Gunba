using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab4
{
    public class SleepingBarber
    {
        private readonly SemaphoreSlim _barberSemaphore;
        private readonly SemaphoreSlim _customerSemaphore;
        private readonly Mutex _seatMutex;
        private readonly ConcurrentQueue<int> _waitingRoom;
        private readonly int _maxSeats;
        private bool _barberSleeping;
        private readonly Thread _barberThread;
        private bool _isRunning;

        public SleepingBarber(int maxSeats = 5)
        {
            _maxSeats = maxSeats;
            _barberSemaphore = new SemaphoreSlim(0, 1);
            _customerSemaphore = new SemaphoreSlim(0);
            _seatMutex = new Mutex();
            _waitingRoom = new ConcurrentQueue<int>();
            _barberSleeping = true;
            _isRunning = true;

            _barberThread = new Thread(BarberWork)
            {
                Name = "Парикмахер"
            };
        }

        public void Start()
        {
            Console.WriteLine("=== Задача о спящем парикмахере ===");
            Console.WriteLine($"Количество мест в очереди: {_maxSeats}\n");

            _barberThread.Start();

            Random random = new Random();
            for (int i = 1; i <= 15; i++)
            {
                Thread.Sleep(random.Next(200, 800));
                CustomerArrives(i);
            }

            Thread.Sleep(5000);
            _isRunning = false;
            _barberSemaphore.Release();
            _barberThread.Join();
        }

        private void BarberWork()
        {
            while (_isRunning)
            {
                if (_waitingRoom.IsEmpty && _barberSleeping)
                {
                    Console.WriteLine("Парикмахер спит...");
                    _barberSleeping = true;
                }

                _barberSemaphore.Wait();

                if (!_isRunning) break;

                _barberSleeping = false;

                while (_waitingRoom.TryDequeue(out int customerId))
                {
                    Console.WriteLine($"Парикмахер стрижет клиента {customerId}");
                    Thread.Sleep(new Random().Next(1000, 2000));
                    Console.WriteLine($"Клиент {customerId} обслужен");

                    _customerSemaphore.Release();
                }

                if (_waitingRoom.IsEmpty)
                {
                    _barberSleeping = true;
                }
            }

            Console.WriteLine("Парикмахер закончил работу");
        }

        private void CustomerArrives(int customerId)
        {
            _seatMutex.WaitOne();
            try
            {
                if (_waitingRoom.Count < _maxSeats)
                {
                    _waitingRoom.Enqueue(customerId);
                    Console.WriteLine($"Клиент {customerId} пришел и сел в очередь (мест занято: {_waitingRoom.Count}/{_maxSeats})");

                    if (_barberSleeping)
                    {
                        Console.WriteLine($"Клиент {customerId} будит парикмахера");
                        _barberSemaphore.Release();
                    }
                }
                else
                {
                    Console.WriteLine($"Клиент {customerId} ушел - нет свободных мест");
                }
            }
            finally
            {
                _seatMutex.ReleaseMutex();
            }
        }
    }
}

