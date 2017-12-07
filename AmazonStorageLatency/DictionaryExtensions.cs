using System.Collections.Generic;

namespace AmazonStorageLatency
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreateNew<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key) where TValue : new ()
        {
            TValue value;
            if (!d.TryGetValue(key, out value))
            {
                value = new TValue();
                d[key] = value;
            }

            return value;
        }
    }
}