using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> where T : IComparable<T>
{
    private List<T> heap;
    [SerializeField] public int size;

    public MinHeap()
    {
        heap = new List<T> { default(T) }; // Dummy element at index 0 for 1-based indexing
        size = 0;
    }

    public void Add(T value)
    {
        size++;
        if (size < heap.Count)
        {
            heap[size] = value;
        }
        else
        {
            heap.Add(value);
        }

        int index = size;
        while (index > 1)
        {
            int parentIndex = index / 2;
            if (heap[parentIndex].CompareTo(value) > 0) // MinHeap comparison
            {
                heap[index] = heap[parentIndex];
                heap[parentIndex] = value;
                index = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    public T Remove()
    {
        if (size == 0) throw new InvalidOperationException("Heap is empty");

        T rootValue = heap[1];
        T lastValue = heap[size];
        heap[1] = lastValue;
        size--;

        int index = 1;
        while (index * 2 <= size)
        {
            int leftIndex = index * 2;
            int rightIndex = leftIndex + 1;

            int smallerChildIndex = leftIndex;
            if (rightIndex <= size && heap[rightIndex].CompareTo(heap[leftIndex]) < 0)
            {
                smallerChildIndex = rightIndex;
            }

            if (heap[smallerChildIndex].CompareTo(lastValue) < 0)
            {
                heap[index] = heap[smallerChildIndex];
                heap[smallerChildIndex] = lastValue;
                index = smallerChildIndex;
            }
            else
            {
                break;
            }
        }

        return rootValue;
    }

    public bool Contains(T value)
    {
        return heap.Contains(value);
    }
    
    // Clear method to reset the heap
    public void Clear()
    {
        heap.Clear();
        heap.Add(default(T)); // Re-add dummy element at index 0
        size = 0;
    }

    public int Count => size;
}