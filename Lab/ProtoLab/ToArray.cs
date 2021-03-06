﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoLab
{
    static class IEnumerarableToArrayExtension
    {
        public static TSource[] ToArrayTest<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new InvalidCastException();
            }

            Buffer<TSource> buffer = new Buffer<TSource>(source);
            return buffer.ToArray();
        }
    }
}
