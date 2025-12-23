using Xunit;
using Lab1;
using System.Text;

namespace Lab1.Tests;


public class FileResourceManagerTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<string> _testFiles = new();

    public FileResourceManagerTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "Lab1FileManagerTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void OpenForWriting_CreatesFile()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        using var manager = new FileResourceManager(filePath, FileMode.Create);

        // Act
        manager.OpenForWriting();
        manager.WriteLine("Test line");

        // Assert
        Assert.True(File.Exists(filePath));
    }

    [Fact]
    public void WriteLine_WritesToFile()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        using var manager = new FileResourceManager(filePath, FileMode.Create);
        manager.OpenForWriting();

        // Act
        manager.WriteLine("First line");
        manager.WriteLine("Second line");

        // Assert
        var content = File.ReadAllText(filePath, Encoding.UTF8);
        Assert.Contains("First line", content);
        Assert.Contains("Second line", content);
    }

    [Fact]
    public void OpenForReading_NonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var filePath = GetTestFilePath("nonexistent.txt");
        using var manager = new FileResourceManager(filePath);

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => manager.OpenForReading());
    }

    [Fact]
    public void ReadAllText_ReadsFileContent()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        File.WriteAllText(filePath, "Line 1\nLine 2\nLine 3", Encoding.UTF8);
        using var manager = new FileResourceManager(filePath);
        manager.OpenForReading();

        // Act
        var content = manager.ReadAllText();

        // Assert
        Assert.Contains("Line 1", content);
        Assert.Contains("Line 2", content);
        Assert.Contains("Line 3", content);
    }

    [Fact]
    public void AppendText_AppendsToFile()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        File.WriteAllText(filePath, "Original text", Encoding.UTF8);
        using var manager = new FileResourceManager(filePath);

        // Act
        manager.AppendText("\nAppended text");

        // Assert
        var content = File.ReadAllText(filePath, Encoding.UTF8);
        Assert.Contains("Original text", content);
        Assert.Contains("Appended text", content);
    }

    [Fact]
    public void GetFileInfo_ReturnsFileInfo()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        File.WriteAllText(filePath, "Test content", Encoding.UTF8);
        using var manager = new FileResourceManager(filePath);

        // Act
        var fileInfo = manager.GetFileInfo();

        // Assert
        Assert.NotNull(fileInfo);
        Assert.True(fileInfo.Exists);
        Assert.True(fileInfo.Length > 0);
    }

    [Fact]
    public void Dispose_ReleasesResources()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        var manager = new FileResourceManager(filePath, FileMode.Create);
        manager.OpenForWriting();
        manager.WriteLine("Test");

        // Act
        manager.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => manager.WriteLine("Test"));
    }

    [Fact]
    public void UsingStatement_AutomaticallyDisposes()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");

        // Act
        using (var manager = new FileResourceManager(filePath, FileMode.Create))
        {
            manager.OpenForWriting();
            manager.WriteLine("Test");
        }

        // Assert
        Assert.True(File.Exists(filePath));
        // Попытка использовать после using должна вызвать исключение
        var manager2 = new FileResourceManager(filePath);
        manager2.Dispose();
        Assert.Throws<ObjectDisposedException>(() => manager2.OpenForReading());
    }

    [Fact]
    public void WriteLine_WithoutOpenForWriting_ThrowsInvalidOperationException()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        using var manager = new FileResourceManager(filePath, FileMode.Create);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => manager.WriteLine("Test"));
    }

    [Fact]
    public void ReadAllText_WithoutOpenForReading_ThrowsInvalidOperationException()
    {
        // Arrange
        var filePath = GetTestFilePath("test.txt");
        File.WriteAllText(filePath, "Test", Encoding.UTF8);
        using var manager = new FileResourceManager(filePath);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => manager.ReadAllText());
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


