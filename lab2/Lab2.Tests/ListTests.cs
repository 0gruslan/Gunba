using Xunit;

namespace Lab2.Tests;

public class ListTests
{
    [Fact]
    public void AddToEnd_ShouldAddElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        list.Add(4);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(4, list[3]);
    }

    [Fact]
    public void AddToBeginning_ShouldAddElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        list.Insert(0, 0);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(0, list[0]);
        Assert.Equal(1, list[1]);
    }

    [Fact]
    public void AddToMiddle_ShouldAddElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 4, 5 };

        // Act
        list.Insert(2, 3);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
    }

    [Fact]
    public void RemoveFromBeginning_ShouldRemoveElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };

        // Act
        list.RemoveAt(0);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(2, list[0]);
    }

    [Fact]
    public void RemoveFromEnd_ShouldRemoveElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };

        // Act
        list.RemoveAt(list.Count - 1);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(3, list[list.Count - 1]);
    }

    [Fact]
    public void RemoveFromMiddle_ShouldRemoveElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        list.RemoveAt(2);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(4, list[2]);
        Assert.Equal(5, list[3]);
    }

    [Fact]
    public void FindByValue_ExistingElement_ShouldReturnTrue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = list.Contains(3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FindByValue_NonExistingElement_ShouldReturnFalse()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = list.Contains(10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetByIndex_ShouldReturnCorrectElement()
    {
        // Arrange
        var list = new List<int> { 10, 20, 30, 40, 50 };

        // Act
        var result = list[2];

        // Assert
        Assert.Equal(30, result);
    }

    [Fact]
    public void GetByIndex_InvalidIndex_ShouldThrowException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list[10]);
    }
}

