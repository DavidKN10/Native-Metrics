using Microsoft.UI.Xaml.Automation.Peers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class HistoryBuffer<T> : IEnumerable<T>
{
    private readonly T[] _items;
    private int _head;
    private int _tail;
    private int _count;
    private const int MAX_HISTORY = 60;

    public int Count => _count;
    public bool isFull => _count == MAX_HISTORY;
    
    public HistoryBuffer()
    {
        _items = new T[MAX_HISTORY];
        _head = 0;
        _tail = 0;
        _count = 0;
    }
    
    public void Add(T item)
    {
        _items[_tail] = item;
        _tail = (_tail + 1) % MAX_HISTORY;

        if (_count < MAX_HISTORY) 
        {
            _count++; 
        }
        else
        {
            _head = (_head + 1) % MAX_HISTORY;  // overwrite oldest, advance head
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }
            int actualIndex = (_head + index) % MAX_HISTORY;
            return _items[actualIndex];
        }
    }

    public void Clear()
    {
        Array.Clear(_items, 0, _items.Length);
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return this[i];
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
