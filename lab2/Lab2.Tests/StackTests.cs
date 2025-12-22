using Xunit;

namespace Lab2.Tests;

public class StackTests
{
    [Fact]
    public void AddToEnd_ShouldPushElement()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        stack.Push(4);

        // Assert
        Assert.Equal(4, stack.Count);
        Assert.Equal(4, stack.Peek());
    }

    [Fact]
    public void RemoveFromEnd_ShouldPopElement()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);

        // Act
        var result = stack.Pop();

        // Assert
        Assert.Equal(4, result);
        Assert.Equal(3, stack.Count);
        Assert.Equal(3, stack.Peek());
    }

    [Fact]
    public void FindByValue_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        stack.Push(5);

        // Act
        var result = stack.Contains(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FindByValue_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var result = stack.Contains(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Stack_LIFO_Order_ShouldBeMaintained()
    {
        // Arrange
        var stack = new Stack<int>();

        // Act
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Assert
        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.Equal(0, stack.Count);
    }
}

