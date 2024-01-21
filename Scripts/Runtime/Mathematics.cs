using UnityEngine;

namespace Framework.Mathematics
{
    public static class Mathematics
    {
        /// <summary>
        /// Maps a value given from `a` space to `b`.
        /// </summary>
        public static float MapValue(this float x, float a0, float a1, float b0, float b1)
        {
            float value = b0 + (b1 - b0) * ((x - a0) / (a1 - a0));
            return Mathf.Clamp(value, Mathf.Min(b0, b1), Mathf.Max(b0, b1));
        }
    }
}