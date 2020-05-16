using System;
using System.Collections.Generic;

namespace Trudograd.NuclearEdition
{
    public static class PersistentRandom
    {
        private static readonly Random __random = new Random();
        private static readonly Dictionary<String, Int32> __values = new Dictionary<String, Int32>();

        public static Int32 Get(String ownerId, Int32 minValue, Int32 maxValue)
        {
            if (!__values.TryGetValue(ownerId, out var result))
            {
                result = __random.Next(minValue, maxValue);
                __values.Add(ownerId, result);
            }

            return result;
        }
    }
}