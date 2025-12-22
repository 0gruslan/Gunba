using System.Collections.Immutable;
using System.Diagnostics;

namespace Lab2;

public abstract class CollectionPerformanceMeasurer
{
    public const int CollectionSize = 100_000;
    protected const int WarmupIterations = 3;
    public const int MeasurementIterations = 5;

    public string CollectionName { get; protected set; } = string.Empty;

    protected abstract void InitializeCollection(int size);

    protected abstract void AddToEnd(int item);

    protected abstract void AddToBeginning(int item);

    protected abstract void AddToMiddle(int item);

    protected abstract void RemoveFromBeginning();

    protected abstract void RemoveFromEnd();

    protected abstract void RemoveFromMiddle();

    protected abstract bool FindByValue(int value);

    protected abstract int? GetByIndex(int index);

    protected double MeasureOperation(Action operation, int iterations = MeasurementIterations)
    {
        for (int i = 0; i < WarmupIterations; i++)
        {
            operation();
        }

        var times = new List<long>();
        for (int i = 0; i < iterations; i++)
        {
            var sw = Stopwatch.StartNew();
            operation();
            sw.Stop();
            times.Add(sw.ElapsedTicks);
        }

        return times.Average();
    }

    public PerformanceResults MeasureAll()
    {
        var results = new PerformanceResults { CollectionName = CollectionName };

        InitializeCollection(CollectionSize);

        results.AddToEndTime = MeasureOperation(() => AddToEnd(999999));

        InitializeCollection(CollectionSize);
        results.AddToBeginningTime = MeasureOperation(() => AddToBeginning(999999));

        InitializeCollection(CollectionSize);
        results.AddToMiddleTime = MeasureOperation(() => AddToMiddle(999999));

        InitializeCollection(CollectionSize);
        results.RemoveFromBeginningTime = MeasureOperation(() => RemoveFromBeginning());

        InitializeCollection(CollectionSize);
        results.RemoveFromEndTime = MeasureOperation(() => RemoveFromEnd());

        InitializeCollection(CollectionSize);
        results.RemoveFromMiddleTime = MeasureOperation(() => RemoveFromMiddle());

        InitializeCollection(CollectionSize);
        results.FindByValueTime = MeasureOperation(() => FindByValue(CollectionSize / 2));

        InitializeCollection(CollectionSize);
        if (SupportsIndexAccess())
        {
            results.GetByIndexTime = MeasureOperation(() => GetByIndex(CollectionSize / 2));
        }

        return results;
    }

    
    protected abstract bool SupportsIndexAccess();
}


public class PerformanceResults
{
    public string CollectionName { get; set; } = string.Empty;
    public double AddToEndTime { get; set; }
    public double AddToBeginningTime { get; set; }
    public double AddToMiddleTime { get; set; }
    public double RemoveFromBeginningTime { get; set; }
    public double RemoveFromEndTime { get; set; }
    public double RemoveFromMiddleTime { get; set; }
    public double FindByValueTime { get; set; }
    public double? GetByIndexTime { get; set; }
}

