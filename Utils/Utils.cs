using UnityEngine;
using System.Collections;

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
 
}