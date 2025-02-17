using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
        
    [Serializable]
    public class Stack<T>
    {
        private T[] _array;
        private int _size;
        
        private const int DefaultCapacity = 64;

        public Stack()
        {
            _array = ArrayPool<T>.Shared.Rent(DefaultCapacity);
        }

        public Stack(Stack<T> stack)
        {
            _array = ArrayPool<T>.Shared.Rent(stack._array.Length);
            Array.Copy(stack._array, _array, stack._size);
            _size = stack._size;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T item) => _array[_size++] = item;
        
        public T Pop()
        {
            _size--;
            return _array[_size];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek() => _array[_size - 1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek(byte k) => _array[_size - k - 1];
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void PushWithResize(T item)
        {
            Grow(_size + 1);
            _array[_size] = item;
            _size++;
        }

        /// <summary>
        ///  Only use in COMPILER PLEASE
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Switch(int a, int b)
        {
            T temp = _array[a];
            T temp2 = _array[b];
            _array[a] = temp2;
            _array[b] = temp;
        }
        
        /// <summary>
        ///  Only use in COMPILER PLEASE
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
        {
            T[] array = new T[_size];
            Array.Copy(_array, array, _size);
            return array;
        }
        
        private void Grow(int capacity)
        {
            /*int newcapacity = _array.Length == 0 ? DefaultCapacity : 2 * _array.Length;

            // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast.
            if ((uint)newcapacity > Array.MaxLength) newcapacity = Array.MaxLength;

            // If computed capacity is still less than specified, set to the original argument.
            // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
            if (newcapacity < capacity) newcapacity = capacity;

            Array.Resize(ref _array, newcapacity);*/
        }
        
        public int Count => _size;
        
        public void Clear()
        {
            Array.Clear(_array, 0, 15);
            _size = 0;
        }
    }
}