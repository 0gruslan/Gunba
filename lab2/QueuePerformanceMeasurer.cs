namespace Lab2;

public class QueuePerformanceMeasurer : CollectionPerformanceMeasurer
{
    private Queue<int>? _queue;

    public QueuePerformanceMeasurer()
    {
        CollectionName = "Queue<T>";
    }

    protected override void InitializeCollection(int size)
    {
        _queue = new Queue<int>(size); 
        for (int i = 0; i < size; i++)
        {
            _queue.Enqueue(i);
        }
    }

    protected override void AddToEnd(int item)
    {
        _queue!.Enqueue(item);
    }

    protected override void AddToBeginning(int item)
    {
        
        var temp = new Queue<int>();
        temp.Enqueue(item);
        while (_queue!.Count > 0)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        _queue = temp;
    }

    protected override void AddToMiddle(int item)
    {
        
        var temp = new Queue<int>();
        int middle = _queue!.Count / 2;
        for (int i = 0; i < middle; i++)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        temp.Enqueue(item);
        while (_queue.Count > 0)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        _queue = temp;
    }

    protected override void RemoveFromBeginning()
    {
        _queue!.Dequeue();
    }

    protected override void RemoveFromEnd()
    {
        
        var temp = new Queue<int>();
        int count = _queue!.Count;
        for (int i = 0; i < count - 1; i++)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        _queue = temp;
    }

    protected override void RemoveFromMiddle()
    {
        
        var temp = new Queue<int>();
        int middle = _queue!.Count / 2;
        for (int i = 0; i < middle; i++)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        _queue.Dequeue(); 
        while (_queue.Count > 0)
        {
            temp.Enqueue(_queue.Dequeue());
        }
        _queue = temp;
    }

    protected override bool FindByValue(int value)
    {
        return _queue!.Contains(value);
    }

    protected override int? GetByIndex(int index)
    {
        return null;
    }

    protected override bool SupportsIndexAccess() => false;
}

