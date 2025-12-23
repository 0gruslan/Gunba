using Lab2.Models;

namespace Lab2.Services;

public class LinkedListPerformanceMeasurer : CollectionPerformanceMeasurer
{
    private LinkedList<int>? _linkedList;

    public LinkedListPerformanceMeasurer()
    {
        CollectionName = "LinkedList<T>";
    }

    protected override void InitializeCollection(int size)
    {
        _linkedList = new LinkedList<int>();
        for (int i = 0; i < size; i++)
        {
            _linkedList.AddLast(i);
        }
    }

    protected override void AddToEnd(int item)
    {
        _linkedList!.AddLast(item);
    }

    protected override void AddToBeginning(int item)
    {
        _linkedList!.AddFirst(item);
    }

    protected override void AddToMiddle(int item)
    {
        var node = _linkedList!.First;
        for (int i = 0; i < _linkedList.Count / 2; i++)
        {
            node = node!.Next;
        }
        _linkedList.AddBefore(node!, item);
    }

    protected override void RemoveFromBeginning()
    {
        _linkedList!.RemoveFirst();
    }

    protected override void RemoveFromEnd()
    {
        _linkedList!.RemoveLast();
    }

    protected override void RemoveFromMiddle()
    {
        var node = _linkedList!.First;
        for (int i = 0; i < _linkedList.Count / 2; i++)
        {
            node = node!.Next;
        }
        _linkedList.Remove(node!);
    }

    protected override bool FindByValue(int value)
    {
        return _linkedList!.Contains(value);
    }

    protected override int? GetByIndex(int index)
    {
        var node = _linkedList!.First;
        for (int i = 0; i < index && node != null; i++)
        {
            node = node.Next;
        }
        return node?.Value;
    }

    protected override bool SupportsIndexAccess() => false;
}

