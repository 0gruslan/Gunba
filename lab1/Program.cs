namespace Lab1;


class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Лабораторная работа 1: Сериализация JSON, управление ресурсами и работа с файлами");
        Console.WriteLine(new string('=', 80));

        try
        {

            DemonstratePerson();

            DemonstratePersonSerializer();

            DemonstrateFileResourceManager();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
            ErrorLogger.LogError("Критическая ошибка в Main", ex);
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void DemonstratePerson()
    {
        Console.WriteLine("\n--- Демонстрация класса Person ---");
        
        var person = new Person
        {
            FirstName = "Дмитрий",
            LastName = "Кривощеков",
            Age = 19,
            Email = "djekti@gmail.com",
            Password = "lalala"
        };

        Console.WriteLine($"Полное имя: {person.FullName}");
        Console.WriteLine($"Возраст: {person.Age}");
        Console.WriteLine($"Совершеннолетний: {person.IsAdult}");
        Console.WriteLine($"Email: {person.Email}");

        try
        {
            person.Email = "invalid-email";
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка валидации: {ex.Message}");
        }
    }

    static void DemonstratePersonSerializer()
    {
        Console.WriteLine("\n--- Демонстрация класса PersonSerializer ---");
        
        var serializer = new PersonSerializer();
        var person = new Person
        {
            FirstName = "Мария",
            LastName = "Максикова",
            Age = 20,
            Email = "mashamaksik@mail.ru",
            Password = "mashamaksik123"
        };

        var json = serializer.SerializeToJson(person);
        Console.WriteLine("Сериализованный JSON:");
        Console.WriteLine(json);

        var deserializedPerson = serializer.DeserializeFromJson(json);
        Console.WriteLine($"\nДесериализованный объект: {deserializedPerson.FullName}");

        var filePath = "person.json";
        serializer.SaveToFile(person, filePath);
        Console.WriteLine($"\nОбъект сохранен в файл: {filePath}");

        var loadedPerson = serializer.LoadFromFile(filePath);
        Console.WriteLine($"Объект загружен из файла: {loadedPerson.FullName}");

        var people = new List<Person>
        {
            new Person { FirstName = "Артем", LastName = "Аведися", Age = 19, Email = "artavedisiyan@gmail.com" },
            new Person { FirstName = "Елизавета", LastName = "Красова", Age = 20, Email = "krasova.ea@talantiuspeh.ru" }
        };

        var listFilePath = "people.json";
        serializer.SaveListToFile(people, listFilePath);
        Console.WriteLine($"\nСписок сохранен в файл: {listFilePath}");

        var loadedPeople = serializer.LoadListFromFile(listFilePath);
        Console.WriteLine($"Загружено объектов: {loadedPeople.Count}");
    }

    static void DemonstrateFileResourceManager()
    {
        Console.WriteLine("\n--- Демонстрация класса FileResourceManager ---");
        
        var filePath = "test_file.txt";

        using (var manager = new FileResourceManager(filePath, FileMode.Create))
        {
            manager.OpenForWriting();
            manager.WriteLine("Первая строка");
            manager.WriteLine("Вторая строка");
            manager.WriteLine("Третья строка");
        }

        using (var manager = new FileResourceManager(filePath))
        {
            manager.OpenForReading();
            var content = manager.ReadAllText();
            Console.WriteLine("Содержимое файла:");
            Console.WriteLine(content);

            var fileInfo = manager.GetFileInfo();
            Console.WriteLine($"\nИнформация о файле:");
            Console.WriteLine($"Размер: {fileInfo.Length} байт");
            Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
            Console.WriteLine($"Дата изменения: {fileInfo.LastWriteTime}");
        }

        using (var manager = new FileResourceManager(filePath))
        {
            manager.AppendText("\nДобавленный текст");
        }

        Console.WriteLine("\nТекст добавлен в конец файла");
    }
}


