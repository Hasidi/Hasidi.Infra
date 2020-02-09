using System;
using System.Collections.Generic;
using System.Threading;

namespace Hasidi.Infra.Utils
{
    public class KeyLock<T> : IDisposable
    {
        private readonly T _key;
        private static readonly Dictionary<T, LockedData> _LockedDic = new Dictionary<T, LockedData>();
        private static readonly object _Lock = new object();

        public KeyLock(T key)
        {
            _key = key;
            AcquireLock();
        }

        private void AcquireLock()
        {
            lock (_Lock)
            {
                if (!_LockedDic.ContainsKey(_key))
                    _LockedDic.Add(_key, new LockedData());
                else
                    _LockedDic[_key]._n++;
            }

            Monitor.Enter(_LockedDic[_key]._lockObj);
        }

        public void Dispose()
        {
            lock (_Lock)
            {
                if (!_LockedDic.ContainsKey(_key))
                    throw new Exception("Unexpected state");
                Monitor.Exit(_LockedDic[_key]._lockObj);
                if (_LockedDic[_key]._n == 1)
                    _LockedDic.Remove(_key);
                else
                    _LockedDic[_key]._n--;
            }
        }


        private class LockedData
        {
            public readonly object _lockObj;
            public uint _n;

            public LockedData()
            {
                _lockObj = new object();
                _n = 1;
            }
        }
    }
}
