using System.Diagnostics;
using System.Globalization;

namespace Lab2;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторная работа 2: Измерение производительности коллекций ===\n");

        var measurers = new List<CollectionPerformanceMeasurer>
        {
            new ListPerformanceMeasurer(),
            new LinkedListPerformanceMeasurer(),
            new QueuePerformanceMeasurer(),
            new StackPerformanceMeasurer(),
            new ImmutableListPerformanceMeasurer()
        };

        var results = new List<PerformanceResults>();

        Console.WriteLine("Начало измерений...\n");

        foreach (var measurer in measurers)
        {
            Console.WriteLine($"Измерение производительности для {measurer.CollectionName}...");
            var sw = Stopwatch.StartNew();
            var result = measurer.MeasureAll();
            sw.Stop();
            results.Add(result);
            Console.WriteLine($"Завершено за {sw.ElapsedMilliseconds} мс\n");
        }

        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ИЗМЕРЕНИЙ ===\n");
        PrintResults(results);

        
        SaveResultsToFile(results);

        Console.WriteLine("\n=== АНАЛИЗ ПРОИЗВОДИТЕЛЬНОСТИ ===\n");
        AnalyzeResults(results);
    }

    static void PrintResults(List<PerformanceResults> results)
    {
        
        Console.WriteLine("Время выполнения операций (в тиках, среднее за 5 итераций):\n");
        
        Console.WriteLine($"{"Коллекция",-20} {"Доб. конец",-15} {"Доб. начало",-15} {"Доб. середина",-15} " +
                         $"{"Уд. начало",-15} {"Уд. конец",-15} {"Уд. середина",-15} {"Поиск",-15} {"Индекс",-15}");
        Console.WriteLine(new string('-', 140));

        foreach (var result in results)
        {
            var indexStr = result.GetByIndexTime.HasValue 
                ? result.GetByIndexTime.Value.ToString("F2", CultureInfo.InvariantCulture) 
                : "N/A";

            Console.WriteLine($"{result.CollectionName,-20} " +
                             $"{result.AddToEndTime,-15:F2} " +
                             $"{result.AddToBeginningTime,-15:F2} " +
                             $"{result.AddToMiddleTime,-15:F2} " +
                             $"{result.RemoveFromBeginningTime,-15:F2} " +
                             $"{result.RemoveFromEndTime,-15:F2} " +
                             $"{result.RemoveFromMiddleTime,-15:F2} " +
                             $"{result.FindByValueTime,-15:F2} " +
                             $"{indexStr,-15}");
        }
    }

    static void AnalyzeResults(List<PerformanceResults> results)
    {
        var listResult = results.First(r => r.CollectionName == "List<T>");
        var linkedListResult = results.First(r => r.CollectionName == "LinkedList<T>");
        var queueResult = results.First(r => r.CollectionName == "Queue<T>");
        var stackResult = results.First(r => r.CollectionName == "Stack<T>");
        var immutableResult = results.First(r => r.CollectionName == "ImmutableList<T>");

        Console.WriteLine("1. ДОБАВЛЕНИЕ В КОНЕЦ:");
        Console.WriteLine($"   List<T>: {listResult.AddToEndTime:F2} тиков");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.AddToEndTime:F2} тиков");
        Console.WriteLine($"   Queue<T>: {queueResult.AddToEndTime:F2} тиков");
        Console.WriteLine($"   Stack<T>: {stackResult.AddToEndTime:F2} тиков");
        Console.WriteLine($"   ImmutableList<T>: {immutableResult.AddToEndTime:F2} тиков");
        Console.WriteLine("   Вывод: List, Queue и Stack показывают O(1) амортизированную сложность.");
        Console.WriteLine("   ImmutableList создает новую коллекцию, поэтому медленнее.\n");

        Console.WriteLine("2. ДОБАВЛЕНИЕ В НАЧАЛО:");
        Console.WriteLine($"   List<T>: {listResult.AddToBeginningTime:F2} тиков (O(n) - сдвиг элементов)");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.AddToBeginningTime:F2} тиков (O(1))");
        Console.WriteLine($"   ImmutableList<T>: {immutableResult.AddToBeginningTime:F2} тиков");
        Console.WriteLine("   Вывод: LinkedList значительно быстрее для добавления в начало.\n");

        Console.WriteLine("3. ДОБАВЛЕНИЕ В СЕРЕДИНУ:");
        Console.WriteLine($"   List<T>: {listResult.AddToMiddleTime:F2} тиков (O(n))");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.AddToMiddleTime:F2} тиков (O(n) - поиск позиции)");
        Console.WriteLine("   Вывод: Обе коллекции имеют O(n) сложность, но List быстрее из-за кэш-локальности.\n");

        Console.WriteLine("4. УДАЛЕНИЕ ИЗ НАЧАЛА:");
        Console.WriteLine($"   List<T>: {listResult.RemoveFromBeginningTime:F2} тиков (O(n))");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.RemoveFromBeginningTime:F2} тиков (O(1))");
        Console.WriteLine($"   Queue<T>: {queueResult.RemoveFromBeginningTime:F2} тиков (O(1))");
        Console.WriteLine("   Вывод: LinkedList и Queue оптимизированы для удаления из начала.\n");

        Console.WriteLine("5. УДАЛЕНИЕ ИЗ КОНЦА:");
        Console.WriteLine($"   List<T>: {listResult.RemoveFromEndTime:F2} тиков (O(1))");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.RemoveFromEndTime:F2} тиков (O(1))");
        Console.WriteLine($"   Stack<T>: {stackResult.RemoveFromEndTime:F2} тиков (O(1))");
        Console.WriteLine("   Вывод: Все три коллекции эффективны для удаления из конца.\n");

        Console.WriteLine("6. ПОИСК ПО ЗНАЧЕНИЮ:");
        Console.WriteLine($"   List<T>: {listResult.FindByValueTime:F2} тиков (O(n))");
        Console.WriteLine($"   LinkedList<T>: {linkedListResult.FindByValueTime:F2} тиков (O(n))");
        Console.WriteLine("   Вывод: Обе коллекции имеют O(n) сложность, но List быстрее из-за кэш-локальности.\n");

        if (listResult.GetByIndexTime.HasValue)
        {
            Console.WriteLine("7. ДОСТУП ПО ИНДЕКСУ:");
            Console.WriteLine($"   List<T>: {listResult.GetByIndexTime:F2} тиков (O(1))");
            Console.WriteLine($"   ImmutableList<T>: {immutableResult.GetByIndexTime:F2} тиков (O(log n))");
            Console.WriteLine("   Вывод: List обеспечивает O(1) доступ, ImmutableList использует дерево (O(log n)).\n");
        }
    }

    static void SaveResultsToFile(List<PerformanceResults> results)
    {
        var lines = new List<string>
        {
            "=== РЕЗУЛЬТАТЫ ИЗМЕРЕНИЙ ПРОИЗВОДИТЕЛЬНОСТИ ===",
            $"Дата: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            $"Размер коллекции: {CollectionPerformanceMeasurer.CollectionSize:N0} элементов",
            $"Количество итераций: {CollectionPerformanceMeasurer.MeasurementIterations}",
            "",
            "Время выполнения операций (в тиках процессора, среднее за 5 итераций):",
            ""
        };

        lines.Add($"{"Коллекция",-20} {"Доб. конец",-15} {"Доб. начало",-15} {"Доб. середина",-15} " +
                 $"{"Уд. начало",-15} {"Уд. конец",-15} {"Уд. середина",-15} {"Поиск",-15} {"Индекс",-15}");
        lines.Add(new string('-', 140));

        foreach (var result in results)
        {
            var indexStr = result.GetByIndexTime.HasValue 
                ? result.GetByIndexTime.Value.ToString("F2", CultureInfo.InvariantCulture) 
                : "N/A";

            lines.Add($"{result.CollectionName,-20} " +
                     $"{result.AddToEndTime,-15:F2} " +
                     $"{result.AddToBeginningTime,-15:F2} " +
                     $"{result.AddToMiddleTime,-15:F2} " +
                     $"{result.RemoveFromBeginningTime,-15:F2} " +
                     $"{result.RemoveFromEndTime,-15:F2} " +
                     $"{result.RemoveFromMiddleTime,-15:F2} " +
                     $"{result.FindByValueTime,-15:F2} " +
                     $"{indexStr,-15}");
        }

        File.WriteAllLines("performance_results.txt", lines);
        Console.WriteLine("Результаты сохранены в файл: performance_results.txt\n");
    }
}

