using UnityEngine;
using System.Collections;
using System;

namespace BrianTools
{
    public struct Tuple<T, U>
    {

        public T x;
        public U y;

        public Tuple(T x, U y)
        {
            this.x = x;
            this.y = y;
        }

    }

    public static class UtilExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }
    }
}