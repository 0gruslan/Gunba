using Lab2.Models;

namespace Lab2.Services;

public class StackPerformanceMeasurer : CollectionPerformanceMeasurer
{
    private Stack<int>? _stack;

    public StackPerformanceMeasurer()
    {
        CollectionName = "Stack<T>";
    }

    protected override void InitializeCollection(int size)
    {
        _stack = new Stack<int>(size);
        for (int i = 0; i < size; i++)
        {
            _stack.Push(i);
        }
    }

    protected override void AddToEnd(int item)
    {
        _stack!.Push(item);
    }

    protected override void AddToBeginning(int item)
    {
        var temp = new Stack<int>();
        temp.Push(item);
        var list = _stack!.ToList();
        list.Reverse();
        foreach (var element in list)
        {
            temp.Push(element);
        }
        _stack = temp;
    }

    protected override void AddToMiddle(int item)
    {
        var list = _stack!.ToList();
        list.Insert(list.Count / 2, item);
        list.Reverse();
        _stack = new Stack<int>(list);
    }

    protected override void RemoveFromBeginning()
    {
        var list = _stack!.ToList();
        list.RemoveAt(0);
        list.Reverse();
        _stack = new Stack<int>(list);
    }

    protected override void RemoveFromEnd()
    {
        _stack!.Pop();
    }

    protected override void RemoveFromMiddle()
    {
        var list = _stack!.ToList();
        list.RemoveAt(list.Count / 2);
        list.Reverse();
        _stack = new Stack<int>(list);
    }

    protected override bool FindByValue(int value)
    {
        return _stack!.Contains(value);
    }

    protected override int? GetByIndex(int index)
    {
        return null;
    }

    protected override bool SupportsIndexAccess() => false;
}

