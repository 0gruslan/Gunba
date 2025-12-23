using System.Text;

namespace Lab1;

public class FileResourceManager : IDisposable
{
    private FileStream? _fileStream;
    private StreamWriter? _writer;
    private StreamReader? _reader;
    private bool _disposed = false;
    private readonly string _filePath;
    private readonly FileMode _fileMode;


    public FileResourceManager(string filePath, FileMode fileMode = FileMode.OpenOrCreate)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        _filePath = filePath;
        _fileMode = fileMode;
    }


    public void OpenForWriting()
    {
        ThrowIfDisposed();

        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _fileStream = new FileStream(_filePath, _fileMode, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_fileStream, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при открытии файла для записи {_filePath}: {ex.Message}", ex);
            Dispose();
            throw;
        }
    }


    public void OpenForReading()
    {
        ThrowIfDisposed();

        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Файл не найден: {_filePath}");

        try
        {
            _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _reader = new StreamReader(_fileStream, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при открытии файла для чтения {_filePath}: {ex.Message}", ex);
            Dispose();
            throw;
        }
    }


    public void WriteLine(string text)
    {
        ThrowIfDisposed();

        if (_writer == null)
            throw new InvalidOperationException("Файл не открыт для записи. Вызовите OpenForWriting()");

        try
        {
            _writer.WriteLine(text);
            _writer.Flush();
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при записи в файл {_filePath}: {ex.Message}", ex);
            throw;
        }
    }


    public string ReadAllText()
    {
        ThrowIfDisposed();

        if (_reader == null)
            throw new InvalidOperationException("Файл не открыт для чтения. Вызовите OpenForReading()");

        try
        {
            _reader.BaseStream.Position = 0;
            return _reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при чтении файла {_filePath}: {ex.Message}", ex);
            throw;
        }
    }


    public void AppendText(string text)
    {
        ThrowIfDisposed();

        try
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(text);
                writer.Flush();
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при добавлении текста в файл {_filePath}: {ex.Message}", ex);
            throw;
        }
    }

    public FileInfo GetFileInfo()
    {
        ThrowIfDisposed();

        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Файл не найден: {_filePath}");

        try
        {
            return new FileInfo(_filePath);
        }
        catch (Exception ex)
        {
            ErrorLogger.LogError($"Ошибка при получении информации о файле {_filePath}: {ex.Message}", ex);
            throw;
        }
    }


    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(FileResourceManager));
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                
                _writer?.Dispose();
                _reader?.Dispose();
                _fileStream?.Dispose();
            }

            _writer = null;
            _reader = null;
            _fileStream = null;
            _disposed = true;
        }
    }


    ~FileResourceManager()
    {
        Dispose(false);
    }
}


