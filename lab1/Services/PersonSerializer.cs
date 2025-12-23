using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace Lab1;


public class PersonSerializer
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new();

    
    public string SerializeToJson(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        try
        {
            return JsonSerializer.Serialize(person, _jsonOptions);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при сериализации Person: {ex.Message}", ex);
            throw;
        }
    }

    
    public Person DeserializeFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON строка не может быть пустой", nameof(json));

        try
        {
            var person = JsonSerializer.Deserialize<Person>(json, _jsonOptions);
            if (person == null)
                throw new InvalidOperationException("Не удалось десериализовать объект Person");
            
            return person;
        }
        catch (JsonException ex)
        {
            ErrorLogger.LogError($"Ошибка при десериализации JSON: {ex.Message}", ex);
            throw;
        }
    }

    
    public void SaveToFile(Person person, string filePath)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        semaphore.Wait();

        try
        {
            var json = SerializeToJson(person);
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при сохранении в файл {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

    
    public Person LoadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        semaphore.Wait();

        try
        {
            var json = File.ReadAllText(filePath, Encoding.UTF8);
            return DeserializeFromJson(json);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при загрузке из файла {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

   
    public async Task SaveToFileAsync(Person person, string filePath)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();

        try
        {
            var json = SerializeToJson(person);
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при асинхронном сохранении в файл {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

  
    public async Task<Person> LoadFromFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();

        try
        {
            var json = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
            return DeserializeFromJson(json);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при асинхронной загрузке из файла {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

  
    public void SaveListToFile(List<Person> people, string filePath)
    {
        if (people == null)
            throw new ArgumentNullException(nameof(people));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        semaphore.Wait();

        try
        {
            var json = JsonSerializer.Serialize(people, _jsonOptions);
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при сохранении списка в файл {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

  
    public List<Person> LoadListFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        var semaphore = _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        semaphore.Wait();

        try
        {
            var json = File.ReadAllText(filePath, Encoding.UTF8);
            var people = JsonSerializer.Deserialize<List<Person>>(json, _jsonOptions);
            
            if (people == null)
                throw new InvalidOperationException("Не удалось десериализовать список Person");
            
            return people;
        }
        catch (JsonException ex)
        {
            ErrorLogger.LogError($"Ошибка при загрузке списка из файла {filePath}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }
}


