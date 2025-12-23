# Лабораторная работа 1: Сериализация JSON, управление ресурсами и работа с файлами в C#

## Описание проекта

Данный проект демонстрирует работу с JSON сериализацией, управлением ресурсами через паттерн `IDisposable` и файловыми операциями в C#.

## Задание 1: Класс Person

Класс `Person` представляет информацию о человеке с использованием JSON атрибутов для управления сериализацией.

### Свойства:

- **FirstName** (`string`) - имя, сериализуется как `firstName`
- **LastName** (`string`) - фамилия, сериализуется как `lastName`
- **Age** (`int`) - возраст, сериализуется как `age`
- **Email** (`string`) - электронная почта с валидацией (должен содержать '@'), сериализуется как `email`
- **Password** (`string`) - пароль, **не сериализуется** (помечен `[JsonIgnore]`)
- **FullName** (`string`, только чтение) - полное имя (FirstName + LastName), **не сериализуется**
- **IsAdult** (`bool`, только чтение) - проверка совершеннолетия (Age >= 18), **не сериализуется**

### Особенности:

- Валидация Email при установке значения (проверка наличия символа '@')
- Вычисляемые свойства `FullName` и `IsAdult` доступны только для чтения

## Задание 2: Класс PersonSerializer

Класс `PersonSerializer` предоставляет методы для работы с JSON сериализацией объектов `Person`.

### Методы:

1. **SerializeToJson(Person person)** - сериализация объекта в строку JSON
2. **DeserializeFromJson(string json)** - десериализация объекта из строки JSON
3. **SaveToFile(Person person, string filePath)** - синхронное сохранение в файл
4. **LoadFromFile(string filePath)** - синхронная загрузка из файла
5. **SaveToFileAsync(Person person, string filePath)** - асинхронное сохранение в файл
6. **LoadFromFileAsync(string filePath)** - асинхронная загрузка из файла
7. **SaveListToFile(List<Person> people, string filePath)** - сохранение списка объектов
8. **LoadListFromFile(string filePath)** - загрузка списка объектов

### Особенности реализации:

- Использование `JsonSerializerOptions` с красивым форматированием (`WriteIndented = true`)
- Кодировка UTF-8 для всех файловых операций
- Обработка исключений с логированием ошибок
- Потокобезопасность через `SemaphoreSlim` для каждого файла
- Автоматическое создание директорий при необходимости

## Задание 3: Класс FileResourceManager

Класс `FileResourceManager` реализует паттерн `IDisposable` для корректного управления файловыми ресурсами.

### Поля:

- `FileStream _fileStream` - поток для работы с файлом
- `StreamWriter _writer` - писатель для текстовых операций
- `StreamReader _reader` - читатель для текстовых операций
- `bool _disposed` - флаг освобождения ресурсов
- `string _filePath` - путь к файлу

### Методы:

- **Конструктор(string filePath, FileMode fileMode)** - инициализация с путем и режимом открытия
- **OpenForWriting()** - открытие файла для записи
- **OpenForReading()** - открытие файла для чтения
- **WriteLine(string text)** - запись строки в файл
- **ReadAllText()** - чтение всего содержимого файла
- **AppendText(string text)** - добавление текста в конец файла
- **GetFileInfo()** - получение информации о файле (размер, дата создания)
- **Dispose()** - явное освобождение ресурсов

### Особенности реализации:

- Правильная реализация `IDisposable` с защищенным методом `Dispose(bool disposing)`
- Финализатор для автоматического освобождения ресурсов
- Вызов `GC.SuppressFinalize(this)` в `Dispose()` для предотвращения двойного освобождения
- Проверка состояния `_disposed` перед операциями
- Обработка исключений при работе с файлами
- Использование `using` для вложенных ресурсов в методе `AppendText`

## Задание 4: Тестирование

Проект содержит автоматизированные тесты с использованием фреймворка **xUnit**.

### Тестовые классы:

1. **PersonTests** - тесты для класса `Person`:
   - Проверка свойства `FullName`
   - Проверка свойства `IsAdult`
   - Валидация Email

2. **PersonSerializerTests** - тесты для класса `PersonSerializer`:
   - Сериализация и десериализация
   - Синхронные и асинхронные операции с файлами
   - Работа со списками объектов
   - Потокобезопасность
   - Обработка ошибок

3. **FileResourceManagerTests** - тесты для класса `FileResourceManager`:
   - Открытие файлов для чтения/записи
   - Запись и чтение данных
   - Добавление текста
   - Получение информации о файле
   - Освобождение ресурсов

### Запуск тестов:

```bash
dotnet test
```

## Запуск проекта

### Требования:

- .NET 8.0 SDK или выше

### Компиляция и запуск:

```bash
cd lab1
dotnet build
dotnet run
```

## Логирование ошибок

Все ошибки автоматически логируются в файл `errors.log` в директории приложения. Класс `ErrorLogger` обеспечивает потокобезопасное логирование с информацией о времени, типе исключения и стеке вызовов.

## Примеры использования

### Пример 1: Создание и сериализация Person

```csharp
var person = new Person
{
    FirstName = "Дмитрий",
    LastName = "Кривощеков",
    Age = 19,
    Email = "djekti@gmail.com",
    Password = "lalala"
};

var serializer = new PersonSerializer();
var json = serializer.SerializeToJson(person);
Console.WriteLine(json);
```

### Пример 2: Работа с файлами

```csharp
var serializer = new PersonSerializer();
var person = new Person { FirstName = "Мария", LastName = "Максикова", Age = 20, Email = "mashamaksik@mail.ru" };


serializer.SaveToFile(person, "person.json");


var loadedPerson = serializer.LoadFromFile("person.json");
```

### Пример 3: Использование FileResourceManager

```csharp
using (var manager = new FileResourceManager("test.txt", FileMode.Create))
{
    manager.OpenForWriting();
    manager.WriteLine("Первая строка");
    manager.WriteLine("Вторая строка");
} 
```


