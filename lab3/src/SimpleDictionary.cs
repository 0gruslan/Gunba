using System.Collections;

namespace Lab3;

public class SimpleDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    where TKey : notnull
{
    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;
    
    private Bucket[] _buckets;
    private int _count;
    private int _version;

    private class Bucket
    {
        public List<KeyValuePair<TKey, TValue>> Items { get; } = new List<KeyValuePair<TKey, TValue>>();
    }

    public SimpleDictionary() : this(DefaultCapacity)
    {
    }

    public SimpleDictionary(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Емкость не может быть отрицательной.");
        
        _buckets = new Bucket[capacity > 0 ? capacity : DefaultCapacity];
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = new Bucket();
        }
        _count = 0;
        _version = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            
            if (TryGetValue(key, out TValue? value))
                return value;
            
            throw new KeyNotFoundException($"Ключ '{key}' не найден в словаре.");
        }
        set
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            
            AddOrUpdate(key, value);
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            var keys = new List<TKey>();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
            }
            return keys;
        }
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    public ICollection<TValue> Values
    {
        get
        {
            var values = new List<TValue>();
            foreach (var pair in this)
            {
                values.Add(pair.Value);
            }
            return values;
        }
    }

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    public int Count => _count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        
        if (ContainsKey(key))
            throw new ArgumentException($"Ключ '{key}' уже существует в словаре.", nameof(key));
        
        AddOrUpdate(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        foreach (var bucket in _buckets)
        {
            bucket.Items.Clear();
        }
        _count = 0;
        _version++;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null)
            return false;
        
        var bucket = GetBucket(item.Key);
        return bucket.Items.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Индекс не может быть отрицательным.");
        
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Недостаточно места в целевом массиве.");
        
        int index = arrayIndex;
        foreach (var pair in this)
        {
            array[index++] = pair;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        int version = _version;
        foreach (var bucket in _buckets)
        {
            foreach (var pair in bucket.Items)
            {
                if (version != _version)
                    throw new InvalidOperationException("Коллекция была изменена во время перебора.");
                
                yield return pair;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        
        var bucket = GetBucket(key);
        for (int i = 0; i < bucket.Items.Count; i++)
        {
            if (Equals(bucket.Items[i].Key, key))
            {
                bucket.Items.RemoveAt(i);
                _count--;
                _version++;
                return true;
            }
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null)
            return false;
        
        var bucket = GetBucket(item.Key);
        if (bucket.Items.Remove(item))
        {
            _count--;
            _version++;
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        
        var bucket = GetBucket(key);
        foreach (var pair in bucket.Items)
        {
            if (Equals(pair.Key, key))
            {
                value = pair.Value;
                return true;
            }
        }
        
        value = default(TValue)!;
        return false;
    }

    private void AddOrUpdate(TKey key, TValue value)
    {
        var bucket = GetBucket(key);
        
        for (int i = 0; i < bucket.Items.Count; i++)
        {
            if (Equals(bucket.Items[i].Key, key))
            {
                bucket.Items[i] = new KeyValuePair<TKey, TValue>(key, value);
                _version++;
                return;
            }
        }
        
        bucket.Items.Add(new KeyValuePair<TKey, TValue>(key, value));
        _count++;
        _version++;
        
        if (_count > _buckets.Length * LoadFactor)
        {
            Resize();
        }
    }

    private Bucket GetBucket(TKey key)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode % _buckets.Length);
        return _buckets[index];
    }

    private void Resize()
    {
        int newCapacity = _buckets.Length * 2;
        var oldBuckets = _buckets;
        _buckets = new Bucket[newCapacity];
        
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = new Bucket();
        }
        
        _count = 0;
        foreach (var oldBucket in oldBuckets)
        {
            foreach (var pair in oldBucket.Items)
            {
                var newBucket = GetBucket(pair.Key);
                newBucket.Items.Add(pair);
                _count++;
            }
        }
        _version++;
    }
}

