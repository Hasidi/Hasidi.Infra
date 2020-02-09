using System;
using System.Collections.Generic;

namespace Hasidi.Infra.DataAccess
{
    public interface IKeyValueDataAccess<TKey, TValue>
    {
        bool InsertData(TKey key, TValue value);
        TValue GetItem(TKey key);
        bool GetAndUpdate(TKey key, Func<TValue, TValue> func);
        void GetItems(List<TKey> keys, Action<Dictionary<TKey, TValue>> func);
        void GetAllItems(Action<Dictionary<TKey, TValue>> action);
    }
}