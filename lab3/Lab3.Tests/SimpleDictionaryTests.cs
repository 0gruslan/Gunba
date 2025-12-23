using Lab3;
using Xunit;

namespace Lab3.Tests;

public class SimpleDictionaryTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateEmptyDictionary()
    {
        // Arrange & Act
        var dict = new SimpleDictionary<int, string>();

        // Assert
        Assert.Equal(0, dict.Count);
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateDictionary()
    {
        // Arrange & Act
        var dict = new SimpleDictionary<int, string>(32);

        // Assert
        Assert.Equal(0, dict.Count);
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleDictionary<int, string>(-1));
    }

    [Fact]
    public void Add_ShouldIncreaseCount()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();

        // Act
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");

        // Assert
        Assert.Equal(3, dict.Count);
    }

    [Fact]
    public void Add_DuplicateKey_ShouldThrowException()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dict.Add(1, "another"));
    }

    [Fact]
    public void Add_WithNullKey_ShouldThrowException()
    {
        // Arrange
        var dict = new SimpleDictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dict.Add(null!, 10));
    }

    [Fact]
    public void Indexer_Get_ShouldReturnCorrectValue()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");

        // Act & Assert
        Assert.Equal("one", dict[1]);
        Assert.Equal("two", dict[2]);
    }

    [Fact]
    public void Indexer_Get_NonExistingKey_ShouldThrowException()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => dict[2]);
    }

    [Fact]
    public void Indexer_Set_ShouldUpdateValue()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act
        dict[1] = "updated";

        // Assert
        Assert.Equal("updated", dict[1]);
    }

    [Fact]
    public void Indexer_Set_NewKey_ShouldAddNewItem()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();

        // Act
        dict[1] = "one";

        // Assert
        Assert.Equal(1, dict.Count);
        Assert.Equal("one", dict[1]);
    }

    [Fact]
    public void ContainsKey_ExistingKey_ShouldReturnTrue()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");

        // Act & Assert
        Assert.True(dict.ContainsKey(1));
        Assert.True(dict.ContainsKey(2));
    }

    [Fact]
    public void ContainsKey_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act & Assert
        Assert.False(dict.ContainsKey(2));
    }

    [Fact]
    public void ContainsKey_NullKey_ShouldThrowException()
    {
        // Arrange
        var dict = new SimpleDictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dict.ContainsKey(null!));
    }

    [Fact]
    public void TryGetValue_ExistingKey_ShouldReturnTrueAndValue()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act
        bool result = dict.TryGetValue(1, out string? value);

        // Assert
        Assert.True(result);
        Assert.Equal("one", value);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act
        bool result = dict.TryGetValue(2, out string? value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void Remove_ExistingKey_ShouldRemoveAndDecreaseCount()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");

        // Act
        bool result = dict.Remove(2);

        // Assert
        Assert.True(result);
        Assert.Equal(2, dict.Count);
        Assert.False(dict.ContainsKey(2));
        Assert.True(dict.ContainsKey(1));
        Assert.True(dict.ContainsKey(3));
    }

    [Fact]
    public void Remove_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act
        bool result = dict.Remove(2);

        // Assert
        Assert.False(result);
        Assert.Equal(1, dict.Count);
    }

    [Fact]
    public void Remove_NullKey_ShouldThrowException()
    {
        // Arrange
        var dict = new SimpleDictionary<string, int>();
        dict.Add("key", 10);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dict.Remove(null!));
    }

    [Fact]
    public void Clear_ShouldRemoveAllItems()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");

        // Act
        dict.Clear();

        // Assert
        Assert.Equal(0, dict.Count);
        Assert.False(dict.ContainsKey(1));
    }

    [Fact]
    public void Contains_ExistingPair_ShouldReturnTrue()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act & Assert
        Assert.True(dict.Contains(new KeyValuePair<int, string>(1, "one")));
    }

    [Fact]
    public void Contains_NonExistingPair_ShouldReturnFalse()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");

        // Act & Assert
        Assert.False(dict.Contains(new KeyValuePair<int, string>(1, "two")));
        Assert.False(dict.Contains(new KeyValuePair<int, string>(2, "one")));
    }

    [Fact]
    public void CopyTo_ShouldCopyAllPairs()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");
        var array = new KeyValuePair<int, string>[5];

        // Act
        dict.CopyTo(array, 1);

        // Assert
        Assert.Equal(default(KeyValuePair<int, string>), array[0]);
        Assert.True(array[1].Key == 1 || array[1].Key == 2 || array[1].Key == 3);
        Assert.True(array[2].Key == 1 || array[2].Key == 2 || array[2].Key == 3);
        Assert.True(array[3].Key == 1 || array[3].Key == 2 || array[3].Key == 3);
    }

    [Fact]
    public void GetEnumerator_ShouldIterateAllPairs()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");
        var pairs = new List<KeyValuePair<int, string>>();

        // Act
        foreach (var pair in dict)
        {
            pairs.Add(pair);
        }

        // Assert
        Assert.Equal(3, pairs.Count);
        Assert.Contains(pairs, p => p.Key == 1 && p.Value == "one");
        Assert.Contains(pairs, p => p.Key == 2 && p.Value == "two");
        Assert.Contains(pairs, p => p.Key == 3 && p.Value == "three");
    }

    [Fact]
    public void Keys_ShouldReturnAllKeys()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");

        // Act
        var keys = dict.Keys;

        // Assert
        Assert.Equal(3, keys.Count);
        Assert.Contains(1, keys);
        Assert.Contains(2, keys);
        Assert.Contains(3, keys);
    }

    [Fact]
    public void Values_ShouldReturnAllValues()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();
        dict.Add(1, "one");
        dict.Add(2, "two");
        dict.Add(3, "three");

        // Act
        var values = dict.Values;

        // Assert
        Assert.Equal(3, values.Count);
        Assert.Contains("one", values);
        Assert.Contains("two", values);
        Assert.Contains("three", values);
    }

    [Fact]
    public void Add_ManyItems_ShouldResizeCorrectly()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>(4);

        // Act
        for (int i = 0; i < 100; i++)
        {
            dict.Add(i, $"value{i}");
        }

        // Assert
        Assert.Equal(100, dict.Count);
        for (int i = 0; i < 100; i++)
        {
            Assert.True(dict.ContainsKey(i));
            Assert.Equal($"value{i}", dict[i]);
        }
    }

    [Fact]
    public void IsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var dict = new SimpleDictionary<int, string>();

        // Act & Assert
        Assert.False(dict.IsReadOnly);
    }

    [Fact]
    public void IReadOnlyDictionary_Keys_ShouldWork()
    {
        // Arrange
        IReadOnlyDictionary<int, string> dict = new SimpleDictionary<int, string>();
        ((SimpleDictionary<int, string>)dict).Add(1, "one");
        ((SimpleDictionary<int, string>)dict).Add(2, "two");

        // Act
        var keys = dict.Keys;

        // Assert
        Assert.Equal(2, keys.Count());
        Assert.Contains(1, keys);
        Assert.Contains(2, keys);
    }

    [Fact]
    public void IReadOnlyDictionary_Values_ShouldWork()
    {
        // Arrange
        IReadOnlyDictionary<int, string> dict = new SimpleDictionary<int, string>();
        ((SimpleDictionary<int, string>)dict).Add(1, "one");
        ((SimpleDictionary<int, string>)dict).Add(2, "two");

        // Act
        var values = dict.Values;

        // Assert
        Assert.Equal(2, values.Count());
        Assert.Contains("one", values);
        Assert.Contains("two", values);
    }
}

