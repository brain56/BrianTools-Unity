using UnityEngine;
using System.Collections;
using System;

namespace BrianTools
{
    public class MathUtils
    {
        public static float GetDistanceSquared(Vector3 position1, Vector3 position2)
        {
            return (position2.x - position1.x) * (position2.x - position1.x) +
            (position2.y - position1.y) * (position2.y - position1.y) +
            (position2.z - position1.z) * (position2.z - position1.z);
        }
    }

    /// <summary>
    /// A serializable int tuple class.
    /// </summary>
    [Serializable]
    public struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IntVector2(float x, float y)
        {
            this.x = Mathf.RoundToInt(x);
            this.y = Mathf.RoundToInt(y);
        }
    }

}