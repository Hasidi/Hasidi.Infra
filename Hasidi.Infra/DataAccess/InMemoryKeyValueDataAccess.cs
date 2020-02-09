using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Hasidi.Infra.Utils;

namespace Hasidi.Infra.DataAccess
{
    public class InMemoryKeyValueDataAccess<TKey, TValue> : IKeyValueDataAccess<TKey, TValue> where TValue : ICloneable
    {
        private ConcurrentDictionary<TKey, TValue> _dic = new ConcurrentDictionary<TKey, TValue>();

        public bool InsertData(TKey key, TValue value)
        {
            return _dic.TryAdd(key, value);
        }

        public TValue GetItem(TKey key)
        {
            if (_dic.TryGetValue(key, out var res))
            {
                return (TValue)res.Clone();
            }
            return default;
        }

        public bool GetAndUpdate(TKey key, Func<TValue, TValue> func)
        {
            if (!_dic.ContainsKey(key))
                throw new KeyNotFoundException($"key:[{key}] doesn't exists");

            using (var l = new KeyLock<TKey>(key))
            {
                //l.AcquireLock();
                var succeeded = _dic.TryGetValue(key, out var value);
                if (!succeeded)
                    return false;
                var newVal = func(value);
                _dic[key] = newVal;
            }
            return true;
        }

        public void GetItems(List<TKey> keys, Action<Dictionary<TKey, TValue>> action)
        {
            var locks = new List<KeyLock<TKey>>();
            try
            {
                var items = new Dictionary<TKey, TValue>();
                foreach (var k in keys)
                {
                    var currentLock = new KeyLock<TKey>(k);
                    if (_dic.TryGetValue(k, out var value))
                    {
                        items.Add(k, value);
                    }

                    locks.Add(currentLock);
                }
                action(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                locks.ForEach(l => l.Dispose());
            }
        }

        public void GetAllItems(Action<Dictionary<TKey, TValue>> action)
        {
            GetItems(_dic.Keys.ToList(), action);
        }

    }
}