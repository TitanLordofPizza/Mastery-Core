using System.Collections.Generic;

namespace Mastery.Core.Utility
{
    public static class Collections
    {
        public static T Add<T>(ref List<T> list, T value)
        {
            list.Add(value);

            return value;
        }
    }
}