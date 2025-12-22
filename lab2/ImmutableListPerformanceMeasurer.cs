using System.Collections.Immutable;

namespace Lab2;

public class ImmutableListPerformanceMeasurer : CollectionPerformanceMeasurer
{
    private ImmutableList<int>? _immutableList;

    public ImmutableListPerformanceMeasurer()
    {
        CollectionName = "ImmutableList<T>";
    }

    protected override void InitializeCollection(int size)
    {
        var builder = ImmutableList.CreateBuilder<int>();
        for (int i = 0; i < size; i++)
        {
            builder.Add(i);
        }
        _immutableList = builder.ToImmutable();
    }

    protected override void AddToEnd(int item)
    {
        _immutableList = _immutableList!.Add(item);
    }

    protected override void AddToBeginning(int item)
    {
        _immutableList = _immutableList!.Insert(0, item);
    }

    protected override void AddToMiddle(int item)
    {
        _immutableList = _immutableList!.Insert(_immutableList.Count / 2, item);
    }

    protected override void RemoveFromBeginning()
    {
        _immutableList = _immutableList!.RemoveAt(0);
    }

    protected override void RemoveFromEnd()
    {
        _immutableList = _immutableList!.RemoveAt(_immutableList.Count - 1);
    }

    protected override void RemoveFromMiddle()
    {
        _immutableList = _immutableList!.RemoveAt(_immutableList.Count / 2);
    }

    protected override bool FindByValue(int value)
    {
        return _immutableList!.Contains(value);
    }

    protected override int? GetByIndex(int index)
    {
        return _immutableList![index];
    }

    protected override bool SupportsIndexAccess() => true;
}

