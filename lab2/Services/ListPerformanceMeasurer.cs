using Lab2.Models;

namespace Lab2.Services;

public class ListPerformanceMeasurer : CollectionPerformanceMeasurer
{
    private List<int>? _list;

    public ListPerformanceMeasurer()
    {
        CollectionName = "List<T>";
    }

    protected override void InitializeCollection(int size)
    {
        _list = new List<int>(size);
        for (int i = 0; i < size; i++)
        {
            _list.Add(i);
        }
    }

    protected override void AddToEnd(int item)
    {
        _list!.Add(item);
    }

    protected override void AddToBeginning(int item)
    {
        _list!.Insert(0, item);
    }

    protected override void AddToMiddle(int item)
    {
        _list!.Insert(_list.Count / 2, item);
    }

    protected override void RemoveFromBeginning()
    {
        _list!.RemoveAt(0);
    }

    protected override void RemoveFromEnd()
    {
        _list!.RemoveAt(_list.Count - 1);
    }

    protected override void RemoveFromMiddle()
    {
        _list!.RemoveAt(_list.Count / 2);
    }

    protected override bool FindByValue(int value)
    {
        return _list!.Contains(value);
    }

    protected override int? GetByIndex(int index)
    {
        return _list![index];
    }

    protected override bool SupportsIndexAccess() => true;
}

