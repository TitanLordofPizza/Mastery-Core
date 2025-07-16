using UnityEngine;

namespace Mastery.Core.Utility
{
    public static class MathUtility
    {
        public static float RoundUp(float value, int decimalPoint)
        {
            float scale = Mathf.Pow(10, decimalPoint);
            return Mathf.Ceil(value * scale) / scale;
        }

        public static int OperateInt(int value, int adjust, OperationType operation)
        {
            if (operation == OperationType.Additive)
            {
                value += adjust;
            }
            else if (operation == OperationType.Subtractive)
            {
                value -= adjust;
            }
            else if (operation == OperationType.Multiplicative)
            {
                value *= adjust;
            }
            else if (operation == OperationType.Divisive)
            {
                value /= adjust;
            }

            return value;
        }

        public static float OperateFloat(float value, float adjust, OperationType operation)
        {
            if (operation == OperationType.Additive)
            {
                value += adjust;
            }
            else if (operation == OperationType.Subtractive)
            {
                value -= adjust;
            }
            else if (operation == OperationType.Multiplicative)
            {
                value *= adjust;
            }
            else if (operation == OperationType.Divisive)
            {
                value /= adjust;
            }

            return value;
        }
    }

    public enum OperationType
    {
        Additive,
        Subtractive,
        Multiplicative,
        Divisive
    }
}