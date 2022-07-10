using System;

namespace WeatherCast
{
    internal static class Guard
    {
        internal static void NotNull<T>(T target, string name)
        {
            if (target == null)
            {
                throw new InvalidOperationException($"'{name}' can't be null!");
            }
        }
    }
}
