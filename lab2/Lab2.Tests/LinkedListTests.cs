using Xunit;

namespace Lab2.Tests;

public class LinkedListTests
{
    [Fact]
    public void AddToEnd_ShouldAddElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        // Act
        list.AddLast(4);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(4, list.Last!.Value);
    }

    [Fact]
    public void AddToBeginning_ShouldAddElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        // Act
        list.AddFirst(0);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(0, list.First!.Value);
        Assert.Equal(1, list.First.Next!.Value);
    }

    [Fact]
    public void AddToMiddle_ShouldAddElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(4);
        list.AddLast(5);

        var middleNode = list.First!.Next!.Next!; // Узел со значением 4

        // Act
        list.AddBefore(middleNode, 3);

        // Assert
        Assert.Equal(5, list.Count);
        var values = list.ToList();
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, values);
    }

    [Fact]
    public void RemoveFromBeginning_ShouldRemoveElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        list.AddLast(4);

        // Act
        list.RemoveFirst();

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(2, list.First!.Value);
    }

    [Fact]
    public void RemoveFromEnd_ShouldRemoveElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        list.AddLast(4);

        // Act
        list.RemoveLast();

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(3, list.Last!.Value);
    }

    [Fact]
    public void RemoveFromMiddle_ShouldRemoveElement()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        list.AddLast(4);
        list.AddLast(5);

        var middleNode = list.First!.Next!.Next!; // Узел со значением 3

        // Act
        list.Remove(middleNode);

        // Assert
        Assert.Equal(4, list.Count);
        var values = list.ToList();
        Assert.Equal(new[] { 1, 2, 4, 5 }, values);
    }

    [Fact]
    public void FindByValue_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        list.AddLast(4);
        list.AddLast(5);

        // Act
        var result = list.Contains(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FindByValue_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        // Act
        var result = list.Contains(10);

        // Assert
        Assert.False(result);
    }
}

