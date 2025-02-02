using System.Collections;
using System.Runtime.CompilerServices;

namespace ConsoleApp1;

[Serializable]
public class Stack<T>
{
    private T[] _array;
    private int _size;
    
    private const int DefaultCapacity = 0;

    public Stack()
    {
        _array = new T[20];
    }

    public Stack(Stack<T> stack)
    {
        _array = stack._array.ToArray();
        _size = Int32.Parse(stack._size.ToString());
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public void Push(T item)
    {
        _array[_size] = item;
        _size++;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public T Pop()
    {
        _size--;
        return _array[_size];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Peek() => Peek(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Peek(byte k)
    {
        return _array[_size - k - 1];
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PushWithResize(T item)
    {
        Grow(_size + 1);
        _array[_size] = item;
        _size++;
    }
    
    private void Grow(int capacity)
    {
        int newcapacity = _array.Length == 0 ? DefaultCapacity : 2 * _array.Length;

        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast.
        if ((uint)newcapacity > Array.MaxLength) newcapacity = Array.MaxLength;

        // If computed capacity is still less than specified, set to the original argument.
        // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
        if (newcapacity < capacity) newcapacity = capacity;

        Array.Resize(ref _array, newcapacity);
    }
    
    public int Count => _size;
    
    public void Clear()
    {
        Array.Clear(_array, 0, 15);
        _size = 0;
    }
}