using Xunit;
using Lab1;

namespace Lab1.Tests;


public class PersonSerializerTests : IDisposable
{
    private readonly PersonSerializer _serializer;
    private readonly string _testDirectory;
    private readonly List<string> _testFiles = new();

    public PersonSerializerTests()
    {
        _serializer = new PersonSerializer();
        _testDirectory = Path.Combine(Path.GetTempPath(), "Lab1Tests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void SerializeToJson_ValidPerson_ReturnsValidJson()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Иван",
            LastName = "Филиппов",
            Age = 19,
            Email = "ivan@filipov.com",
            Password = "elizavetaalexandrovnasamayaluchshaya"
        };

        // Act
        var json = _serializer.SerializeToJson(person);

        // Assert
        Assert.NotNull(json);
        Assert.Contains("Иван", json);
        Assert.Contains("Филиппов", json);
        Assert.Contains("19", json);
        Assert.Contains("ivan@filipov.com", json);
        Assert.DoesNotContain("elizavetaalexandrovnasamayaluchshaya", json); 
    }

    [Fact]
    public void SerializeToJson_NullPerson_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _serializer.SerializeToJson(null!));
    }

    [Fact]
    public void DeserializeFromJson_ValidJson_ReturnsPerson()
    {
        // Arrange
        var json = @"{
  ""firstName"": ""Мария"",
  ""lastName"": ""Максикова"",
  ""age"": 19,
  ""email"": ""maria@maksik.com""
}";

        // Act
        var person = _serializer.DeserializeFromJson(json);

        // Assert
        Assert.NotNull(person);
        Assert.Equal("Мария", person.FirstName);
        Assert.Equal("Максикова", person.LastName);
        Assert.Equal(19, person.Age);
        Assert.Equal("maria@maksik.com", person.Email);
        Assert.Equal("Мария Петрова", person.FullName);
        Assert.True(person.IsAdult);
    }

    [Fact]
    public void DeserializeFromJson_EmptyString_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _serializer.DeserializeFromJson(""));
    }

    [Fact]
    public void SaveToFile_ValidPerson_CreatesFile()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Алексей",
            LastName = "Потапов",
            Age = 20,
            Email = "alex@potapov.com"
        };
        var filePath = GetTestFilePath("person.json");

        // Act
        _serializer.SaveToFile(person, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = File.ReadAllText(filePath);
        Assert.Contains("Алексей", content);
        Assert.Contains("Потапов", content);
    }

    [Fact]
    public void LoadFromFile_ValidFile_ReturnsPerson()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Елена",
            LastName = "Козлова",
            Age = 22,
            Email = "elena@kozlova.com"
        };
        var filePath = GetTestFilePath("person.json");
        _serializer.SaveToFile(person, filePath);

        // Act
        var loadedPerson = _serializer.LoadFromFile(filePath);

        // Assert
        Assert.NotNull(loadedPerson);
        Assert.Equal("Елена", loadedPerson.FirstName);
        Assert.Equal("Козлова", loadedPerson.LastName);
        Assert.Equal(22, loadedPerson.Age);
        Assert.Equal("elena@kozlova.com", loadedPerson.Email);
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var filePath = GetTestFilePath("nonexistent.json");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _serializer.LoadFromFile(filePath));
    }

    [Fact]
    public async Task SaveToFileAsync_ValidPerson_CreatesFile()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Дмитрий",
            LastName = "Смирнов",
            Age = 35,
            Email = "dmitry@smirnov.com"
        };
        var filePath = GetTestFilePath("person_async.json");

        // Act
        await _serializer.SaveToFileAsync(person, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Contains("Дмитрий", content);
    }

    [Fact]
    public async Task LoadFromFileAsync_ValidFile_ReturnsPerson()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Ольга",
            LastName = "Новикова",
            Age = 27,
            Email = "olga@novikova.com"
        };
        var filePath = GetTestFilePath("person_async.json");
        await _serializer.SaveToFileAsync(person, filePath);

        // Act
        var loadedPerson = await _serializer.LoadFromFileAsync(filePath);

        // Assert
        Assert.NotNull(loadedPerson);
        Assert.Equal("Ольга", loadedPerson.FirstName);
        Assert.Equal("Новикова", loadedPerson.LastName);
    }

    [Fact]
    public void SaveListToFile_ValidList_CreatesFile()
    {
        // Arrange
        var people = new List<Person>
        {
            new Person { FirstName = "Иван", LastName = "Иванов", Age = 25, Email = "ivan@example.com" },
            new Person { FirstName = "Мария", LastName = "Петрова", Age = 30, Email = "maria@example.com" },
            new Person { FirstName = "Алексей", LastName = "Сидоров", Age = 28, Email = "alex@example.com" }
        };
        var filePath = GetTestFilePath("people.json");

        // Act
        _serializer.SaveListToFile(people, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = File.ReadAllText(filePath);
        Assert.Contains("Иван", content);
        Assert.Contains("Мария", content);
        Assert.Contains("Алексей", content);
    }

    [Fact]
    public void LoadListFromFile_ValidFile_ReturnsList()
    {
        // Arrange
        var people = new List<Person>
        {
            new Person { FirstName = "Иван", LastName = "Иванов", Age = 25, Email = "ivan@example.com" },
            new Person { FirstName = "Мария", LastName = "Петрова", Age = 30, Email = "maria@example.com" }
        };
        var filePath = GetTestFilePath("people.json");
        _serializer.SaveListToFile(people, filePath);

        // Act
        var loadedPeople = _serializer.LoadListFromFile(filePath);

        // Assert
        Assert.NotNull(loadedPeople);
        Assert.Equal(2, loadedPeople.Count);
        Assert.Equal("Иван", loadedPeople[0].FirstName);
        Assert.Equal("Мария", loadedPeople[1].FirstName);
    }

    [Fact]
    public async Task SaveToFile_ThreadSafety_MultipleThreads()
    {
        // Arrange
        var filePath = GetTestFilePath("thread_test.json");
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            int index = i;
            tasks.Add(Task.Run(() =>
            {
                var person = new Person
                {
                    FirstName = $"Person{index}",
                    LastName = "Test",
                    Age = 20 + index,
                    Email = $"person{index}@example.com"
                };
                _serializer.SaveToFile(person, filePath);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.True(File.Exists(filePath));
        var loadedPerson = _serializer.LoadFromFile(filePath);
        Assert.NotNull(loadedPerson);
    }

    [Fact]
    public void SerializeToJson_PersonWithFullName_DoesNotSerializeFullName()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Тест",
            LastName = "Тестов",
            Age = 25,
            Email = "test@testov.com"
        };

        // Act
        var json = _serializer.SerializeToJson(person);

        // Assert
        Assert.DoesNotContain("FullName", json);
        Assert.DoesNotContain("Тест Тестов", json);
    }

    [Fact]
    public void SerializeToJson_PersonWithIsAdult_DoesNotSerializeIsAdult()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Тест",
            LastName = "Тестов",
            Age = 25,
            Email = "test@testov.com"
        };

        // Act
        var json = _serializer.SerializeToJson(person);

        // Assert
        Assert.DoesNotContain("IsAdult", json);
    }

    private string GetTestFilePath(string fileName)
    {
        var filePath = Path.Combine(_testDirectory, fileName);
        _testFiles.Add(filePath);
        return filePath;
    }

    public void Dispose()
    {
        // Очистка тестовых файлов
        foreach (var file in _testFiles)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch { }
        }

        try
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }
        catch { }
    }
}

