using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4
{
    public class ProducerConsumer
    {
        public static void DemonstrateBlockingCollection()
        {
            Console.WriteLine("=== Producer-Consumer с BlockingCollection ===");
            Console.WriteLine();

            var buffer = new BlockingCollection<int>(boundedCapacity: 5);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var producers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int producerId = i + 1;
                producers[i] = Task.Run(() => ProducerWork(buffer, producerId, cancellationToken));
            }

            var consumers = new Task[2];
            for (int i = 0; i < 2; i++)
            {
                int consumerId = i + 1;
                consumers[i] = Task.Run(() => ConsumerWork(buffer, consumerId, cancellationToken));
            }

            Thread.Sleep(10000);
            cancellationTokenSource.Cancel();

            buffer.CompleteAdding();

            Task.WaitAll(producers);
            Task.WaitAll(consumers);

            Console.WriteLine("\nДемонстрация завершена\n");
        }

        public static void DemonstrateSemaphoreAndLock()
        {
            Console.WriteLine("=== Producer-Consumer с SemaphoreSlim + lock ===");
            Console.WriteLine();

            const int bufferSize = 5;
            var buffer = new Queue<int>();
            var lockObject = new object();
            var emptySlots = new SemaphoreSlim(bufferSize, bufferSize);
            var filledSlots = new SemaphoreSlim(0, bufferSize);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var producers = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int producerId = i + 1;
                producers[i] = Task.Run(() => ProducerWorkWithSemaphore(
                    buffer, lockObject, emptySlots, filledSlots, producerId, cancellationToken));
            }

            var consumers = new Task[2];
            for (int i = 0; i < 2; i++)
            {
                int consumerId = i + 1;
                consumers[i] = Task.Run(() => ConsumerWorkWithSemaphore(
                    buffer, lockObject, emptySlots, filledSlots, consumerId, cancellationToken));
            }

            Thread.Sleep(10000);
            cancellationTokenSource.Cancel();

            for (int i = 0; i < bufferSize; i++)
            {
                emptySlots.Release();
                filledSlots.Release();
            }

            Task.WaitAll(producers);
            Task.WaitAll(consumers);

            Console.WriteLine("\nДемонстрация завершена\n");
        }

        private static void ProducerWork(BlockingCollection<int> buffer, int producerId, CancellationToken cancellationToken)
        {
            int item = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    item++;
                    buffer.Add(item, cancellationToken);
                    Console.WriteLine($"Производитель {producerId} добавил товар {item} (буфер: {buffer.Count})");
                    Thread.Sleep(new Random().Next(300, 800));
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            Console.WriteLine($"Производитель {producerId} завершил работу");
        }

        private static void ConsumerWork(BlockingCollection<int> buffer, int consumerId, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested || !buffer.IsCompleted)
            {
                try
                {
                    if (buffer.TryTake(out int item, 1000, cancellationToken))
                    {
                        Console.WriteLine($"Потребитель {consumerId} забрал товар {item} (буфер: {buffer.Count})");
                        Thread.Sleep(new Random().Next(500, 1200));
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            Console.WriteLine($"Потребитель {consumerId} завершил работу");
        }

        private static void ProducerWorkWithSemaphore(
            Queue<int> buffer,
            object lockObject,
            SemaphoreSlim emptySlots,
            SemaphoreSlim filledSlots,
            int producerId,
            CancellationToken cancellationToken)
        {
            int item = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                emptySlots.Wait(cancellationToken);

                lock (lockObject)
                {
                    item++;
                    buffer.Enqueue(item);
                    Console.WriteLine($"Производитель {producerId} добавил товар {item} (буфер: {buffer.Count})");
                }

                filledSlots.Release();
                Thread.Sleep(new Random().Next(300, 800));
            }
            Console.WriteLine($"Производитель {producerId} завершил работу");
        }

        private static void ConsumerWorkWithSemaphore(
            Queue<int> buffer,
            object lockObject,
            SemaphoreSlim emptySlots,
            SemaphoreSlim filledSlots,
            int consumerId,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!filledSlots.Wait(1000, cancellationToken))
                    continue;

                int item;
                lock (lockObject)
                {
                    if (buffer.Count == 0)
                    {
                        filledSlots.Release();
                        continue;
                    }
                    item = buffer.Dequeue();
                    Console.WriteLine($"Потребитель {consumerId} забрал товар {item} (буфер: {buffer.Count})");
                }

                emptySlots.Release();
                Thread.Sleep(new Random().Next(500, 1200));
            }
            Console.WriteLine($"Потребитель {consumerId} завершил работу");
        }
    }
}

