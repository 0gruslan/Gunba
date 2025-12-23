using System.Collections.Immutable;
using Xunit;

namespace Lab2.Tests;

public class ImmutableListTests
{
    [Fact]
    public void AddToEnd_ShouldAddElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3);

        // Act
        var newList = list.Add(4);

        // Assert
        Assert.Equal(3, list.Count); // Исходный список не изменился
        Assert.Equal(4, newList.Count);
        Assert.Equal(4, newList[3]);
    }

    [Fact]
    public void AddToBeginning_ShouldAddElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3);

        // Act
        var newList = list.Insert(0, 0);

        // Assert
        Assert.Equal(3, list.Count); // Исходный список не изменился
        Assert.Equal(4, newList.Count);
        Assert.Equal(0, newList[0]);
        Assert.Equal(1, newList[1]);
    }

    [Fact]
    public void AddToMiddle_ShouldAddElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(4).Add(5);

        // Act
        var newList = list.Insert(2, 3);

        // Assert
        Assert.Equal(4, list.Count); // Исходный список не изменился
        Assert.Equal(5, newList.Count);
        Assert.Equal(3, newList[2]);
        Assert.Equal(4, newList[3]);
    }

    [Fact]
    public void RemoveFromBeginning_ShouldRemoveElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3).Add(4);

        // Act
        var newList = list.RemoveAt(0);

        // Assert
        Assert.Equal(4, list.Count); // Исходный список не изменился
        Assert.Equal(3, newList.Count);
        Assert.Equal(2, newList[0]);
    }

    [Fact]
    public void RemoveFromEnd_ShouldRemoveElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3).Add(4);

        // Act
        var newList = list.RemoveAt(list.Count - 1);

        // Assert
        Assert.Equal(4, list.Count); // Исходный список не изменился
        Assert.Equal(3, newList.Count);
        Assert.Equal(3, newList[newList.Count - 1]);
    }

    [Fact]
    public void RemoveFromMiddle_ShouldRemoveElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3).Add(4).Add(5);

        // Act
        var newList = list.RemoveAt(2);

        // Assert
        Assert.Equal(5, list.Count); // Исходный список не изменился
        Assert.Equal(4, newList.Count);
        Assert.Equal(4, newList[2]);
        Assert.Equal(5, newList[3]);
    }

    [Fact]
    public void FindByValue_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3).Add(4).Add(5);

        // Act
        var result = list.Contains(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FindByValue_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(1).Add(2).Add(3);

        // Act
        var result = list.Contains(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetByIndex_ShouldReturnCorrectElement()
    {
        // Arrange
        var list = ImmutableList<int>.Empty.Add(10).Add(20).Add(30).Add(40).Add(50);

        // Act
        var result = list[2];

        // Assert
        Assert.Equal(30, result);
    }

    [Fact]
    public void Immutability_ShouldBePreserved()
    {
        // Arrange
        var list1 = ImmutableList<int>.Empty.Add(1).Add(2).Add(3);
        var list2 = list1.Add(4);

        // Assert
        Assert.Equal(3, list1.Count);
        Assert.Equal(4, list2.Count);
        Assert.NotSame(list1, list2);
    }
}

