using Lab3;
using Xunit;

namespace Lab3.Tests;

public class SimpleListTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyList()
    {
        // Arrange & Act
        var list = new SimpleList();

        // Assert
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateListWithCapacity()
    {
        // Arrange & Act
        var list = new SimpleList(10);

        // Assert
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleList(-1));
    }

    [Fact]
    public void Add_ShouldIncreaseCount()
    {
        // Arrange
        var list = new SimpleList();

        // Act
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // Assert
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Add_ShouldReturnIndex()
    {
        // Arrange
        var list = new SimpleList();

        // Act
        int index1 = list.Add(10);
        int index2 = list.Add(20);

        // Assert
        Assert.Equal(0, index1);
        Assert.Equal(1, index2);
    }

    [Fact]
    public void Indexer_Get_ShouldReturnCorrectValue()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act & Assert
        Assert.Equal(10, list[0]);
        Assert.Equal(20, list[1]);
        Assert.Equal(30, list[2]);
    }

    [Fact]
    public void Indexer_Get_OutOfRange_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
    }

    [Fact]
    public void Indexer_Set_ShouldUpdateValue()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act
        list[0] = 100;
        list[1] = 200;

        // Assert
        Assert.Equal(100, list[0]);
        Assert.Equal(200, list[1]);
    }

    [Fact]
    public void Indexer_Set_OutOfRange_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1] = 20);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = 20);
    }

    [Fact]
    public void Contains_ExistingItem_ShouldReturnTrue()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act & Assert
        Assert.True(list.Contains(10));
        Assert.True(list.Contains(20));
        Assert.True(list.Contains(30));
    }

    [Fact]
    public void Contains_NonExistingItem_ShouldReturnFalse()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act & Assert
        Assert.False(list.Contains(30));
        Assert.False(list.Contains(null));
    }

    [Fact]
    public void IndexOf_ExistingItem_ShouldReturnCorrectIndex()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act & Assert
        Assert.Equal(0, list.IndexOf(10));
        Assert.Equal(1, list.IndexOf(20));
        Assert.Equal(2, list.IndexOf(30));
    }

    [Fact]
    public void IndexOf_NonExistingItem_ShouldReturnMinusOne()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act & Assert
        Assert.Equal(-1, list.IndexOf(30));
    }

    [Fact]
    public void Insert_AtBeginning_ShouldInsertCorrectly()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act
        list.Insert(0, 5);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(5, list[0]);
        Assert.Equal(10, list[1]);
        Assert.Equal(20, list[2]);
    }

    [Fact]
    public void Insert_AtMiddle_ShouldInsertCorrectly()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act
        list.Insert(1, 15);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal(10, list[0]);
        Assert.Equal(15, list[1]);
        Assert.Equal(20, list[2]);
        Assert.Equal(30, list[3]);
    }

    [Fact]
    public void Insert_AtEnd_ShouldInsertCorrectly()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act
        list.Insert(2, 30);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(10, list[0]);
        Assert.Equal(20, list[1]);
        Assert.Equal(30, list[2]);
    }

    [Fact]
    public void Insert_OutOfRange_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(2, 5));
    }

    [Fact]
    public void Remove_ExistingItem_ShouldRemoveAndDecreaseCount()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act
        list.Remove(20);

        // Assert
        Assert.Equal(2, list.Count);
        Assert.Equal(10, list[0]);
        Assert.Equal(30, list[1]);
        Assert.False(list.Contains(20));
    }

    [Fact]
    public void Remove_NonExistingItem_ShouldNotChangeList()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);

        // Act
        list.Remove(30);

        // Assert
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void RemoveAt_ShouldRemoveItemAtIndex()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act
        list.RemoveAt(1);

        // Assert
        Assert.Equal(2, list.Count);
        Assert.Equal(10, list[0]);
        Assert.Equal(30, list[1]);
    }

    [Fact]
    public void RemoveAt_OutOfRange_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(1));
    }

    [Fact]
    public void Clear_ShouldRemoveAllItems()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // Act
        list.Clear();

        // Assert
        Assert.Equal(0, list.Count);
        Assert.False(list.Contains(10));
    }

    [Fact]
    public void CopyTo_ShouldCopyAllItems()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        var array = new object[5];

        // Act
        list.CopyTo(array, 1);

        // Assert
        Assert.Null(array[0]);
        Assert.Equal(10, array[1]);
        Assert.Equal(20, array[2]);
        Assert.Equal(30, array[3]);
        Assert.Null(array[4]);
    }

    [Fact]
    public void CopyTo_NullArray_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_InsufficientSpace_ShouldThrowException()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        var array = new object[1];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => list.CopyTo(array, 0));
    }

    [Fact]
    public void GetEnumerator_ShouldIterateAllItems()
    {
        // Arrange
        var list = new SimpleList();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        var items = new List<object?>();

        // Act
        foreach (var item in list)
        {
            items.Add(item);
        }

        // Assert
        Assert.Equal(3, items.Count);
        Assert.Equal(10, items[0]);
        Assert.Equal(20, items[1]);
        Assert.Equal(30, items[2]);
    }

    [Fact]
    public void Add_ManyItems_ShouldResizeCorrectly()
    {
        // Arrange
        var list = new SimpleList(2);

        // Act
        for (int i = 0; i < 100; i++)
        {
            list.Add(i);
        }

        // Assert
        Assert.Equal(100, list.Count);
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i, list[i]);
        }
    }

    [Fact]
    public void Properties_ShouldReturnCorrectValues()
    {
        // Arrange
        var list = new SimpleList();

        // Act & Assert
        Assert.False(list.IsReadOnly);
        Assert.False(list.IsFixedSize);
        Assert.False(list.IsSynchronized);
        Assert.NotNull(list.SyncRoot);
    }
}

