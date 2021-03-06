﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoLab
{
    /// <summary>
    /// Bind to an ICollection of an other type
    /// </summary>
    /// <typeparam name="A">Destination</typeparam>
    /// <typeparam name="T">Source</typeparam>
    class BindingCollection<A, T> : ICollection<T> where A : class, T
    {
        ICollection<A> collection;

        public BindingCollection(ICollection<A> collection)
        {
            this.collection = collection;
        }

        public void Add(T item)
        {
            collection.Add(item as A);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public bool Contains(T item)
        {
            return collection.Contains(item as A);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            A[] tmp = Array.ConvertAll(array, element => element as A);
            collection.CopyTo(tmp, arrayIndex);
            for (int i = arrayIndex; i < array.Length - arrayIndex; ++i)
                array[i] = tmp[i];
        }

        public int Count
        {
            get
            {
                return collection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return collection.IsReadOnly;
            }
        }

        public bool Remove(T item)
        {
            return collection.Remove(item as A);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator() as IEnumerator<T>;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
