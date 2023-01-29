using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TurtleGraphicsBlazor.Data;

namespace TurtleGraphicsBlazor
{
    public static class ListExtension
    {
        /// <summary>
        /// swap element with element above it
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="l">list</param>
        /// <param name="idx">index of source element</param>
        /// <returns>if sucessful new position index. if unsucessful same index is retured </returns>
        public static int MoveItemUpAt<T>(this List<T> l, int idx)
        {
            if (idx - 1 > -1)
            {
                var tmp = l[idx];
                l[idx] = l[idx - 1];
                l[idx - 1] = tmp;
                return idx - 1;
            }
            return idx;
        }
        /// <summary>
        /// swap element with element below it
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="l">list</param>
        /// <param name="idx">index of source element</param>
        /// <returns>if sucessful new position index. if unsucessful same index is retured</returns>
        public static int MoveItemDownAt<T>(this List<T> l, int idx)
        {
            if (idx + 1 < l.Count)
            {
                var tmp = l[idx];
                l[idx] = l[idx + 1];
                l[idx + 1] = tmp;
                return idx + 1;
            }
            return idx;
        }
    }
}
