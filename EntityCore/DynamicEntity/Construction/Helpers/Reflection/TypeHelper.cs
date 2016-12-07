﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EntityCore.DynamicEntity.Construction.Helper.Reflection
{
    static class TypeHelper
    {
        /// <summary>
        /// [ <c>public static Type GetNullableType(Type TypeToConvert)</c> ]
        /// <para></para>
        /// Convert any Type to its Nullable&lt;T&gt; form, if possible
        /// </summary>
        /// <param name="typeToConvert">The Type to convert</param>
        /// <returns>
        /// The Nullable&lt;T&gt; converted from the original type, the original type if it was already nullable, or null 
        /// if either <paramref name="typeToConvert"/> could not be converted or if it was null.
        /// </returns>
        /// <remarks>
        /// To qualify to be converted to a nullable form, <paramref name="typeToConvert"/> must contain a non-nullable value 
        /// type other than System.Void.  Otherwise, this method will return a null.
        /// </remarks>
        /// <seealso cref="Nullable&lt;T&gt;"/>
        public static Type GetNullableType(Type typeToConvert)
        {
            // Abort if no type supplied
            if (typeToConvert == null)
                return null;

            // If the given type is already nullable, just return it
            if (IsTypeNullable(typeToConvert))
                return typeToConvert;

            // If the type is a ValueType and is not System.Void, convert it to a Nullable<Type>
            if (typeToConvert.IsValueType && typeToConvert != typeof(void))
                return typeof(Nullable<>).MakeGenericType(typeToConvert);

            // Done - no conversion
            return null;
        }

        /// <summary>
        /// [ <c>public static bool IsTypeNullable(Type TypeToTest)</c> ]
        /// <para></para>
        /// Reports whether a given Type is nullable (Nullable&lt; Type &gt;)
        /// </summary>
        /// <param name="typeToTest">The Type to test</param>
        /// <returns>
        /// true = The given Type is a Nullable&lt; Type &gt;; false = The type is not nullable, or <paramref name="typeToTest"/> 
        /// is null.
        /// </returns>
        /// <remarks>
        /// This method tests <paramref name="typeToTest"/> and reports whether it is nullable (i.e. whether it is either a 
        /// reference type or a form of the generic Nullable&lt; T &gt; type).
        /// </remarks>
        /// <seealso cref="GetNullableType"/>
        public static bool IsTypeNullable(Type typeToTest)
        {
            // Abort if no type supplied
            if (typeToTest == null)
                return false;

            // If this is not a value type, it is a reference type, so it is automatically nullable
            //  (NOTE: All forms of Nullable<T> are value types)
            if (!typeToTest.IsValueType)
                return true;

            // Report whether TypeToTest is a form of the Nullable<> type
            return typeToTest.IsGenericType && typeToTest.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
