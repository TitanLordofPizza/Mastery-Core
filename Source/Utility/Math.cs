using UnityEngine;

namespace Mastery.Core.Utility
{
    public static class Math
    {
        public static float RoundUp(float value, int decimalPoint)
        {
            float scale = Mathf.Pow(10, decimalPoint);
            return Mathf.Ceil(value * scale) / scale;
        }
    }
}