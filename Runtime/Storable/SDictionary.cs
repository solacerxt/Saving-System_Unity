using System.Collections;
using System.Collections.Generic;

namespace solacerxt.SaveSystem
{
    [System.Serializable]
    public struct SDictionary<K, V> : IStorable, IEnumerable<KeyValuePair<K, V>>
    {
        public KeyValuePair[] Items;

        public SDictionary(Dictionary<K, V> dct)
        {
            Items = new KeyValuePair[dct.Keys.Count];

            int i = 0;
            foreach (var item in dct)
                Items[i++] = new KeyValuePair(item.Key, item.Value);
        }

        public static implicit operator SDictionary<K, V>(Dictionary<K, V> dct) => 
            new SDictionary<K, V>(dct);

        public Dictionary<K, V> ToDictionary() => new Dictionary<K, V>(this);

        IEnumerator IEnumerable.GetEnumerator() 
        {
            int n = Items.Length;
            for (int i = 0; i < n; ++i)
                yield return new KeyValuePair<K, V>(Items[i].Key, Items[i].Value);
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() 
        {
            int n = Items.Length;
            for (int i = 0; i < n; ++i)
                yield return new KeyValuePair<K, V>(Items[i].Key, Items[i].Value);
        }

        [System.Serializable]
        public struct KeyValuePair
        {
            public K Key;
            public V Value;

            public KeyValuePair(K key, V value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
