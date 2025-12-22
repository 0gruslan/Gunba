using Xunit;

namespace Lab2.Tests;

public class QueueTests
{
    [Fact]
    public void AddToEnd_ShouldEnqueueElement()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        queue.Enqueue(4);

        // Assert
        Assert.Equal(4, queue.Count);
    }

    [Fact]
    public void RemoveFromBeginning_ShouldDequeueElement()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        // Act
        var result = queue.Dequeue();

        // Assert
        Assert.Equal(1, result);
        Assert.Equal(3, queue.Count);
        Assert.Equal(2, queue.Peek());
    }

    [Fact]
    public void FindByValue_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5);

        // Act
        var result = queue.Contains(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FindByValue_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = queue.Contains(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Queue_FIFO_Order_ShouldBeMaintained()
    {
        // Arrange
        var queue = new Queue<int>();

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Assert
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(0, queue.Count);
    }
}

