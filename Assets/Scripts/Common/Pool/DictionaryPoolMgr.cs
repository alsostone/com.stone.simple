using System;
using System.Collections;
using System.Collections.Generic;

namespace ST.Common
{
    public abstract class ADictionaryPoolMgr<T0> : Singleton<T0> where T0 : new()
    {
        private readonly Dictionary<Type, IPool> mPools = new Dictionary<Type, IPool>();

        public T Get<T>() where T : class, IDictionary
        {
            IPool pool = null;
            Type t = typeof(T);
            if (!mPools.TryGetValue(typeof(T), out pool)) {
                pool = new DictionaryPool<T>();
                mPools.Add(t, pool);
            }
            return ((DictionaryPool<T>)pool).Get();
        }
        
        public void Return<T>(T obj) where T : class, IDictionary
        {
            if (mPools.TryGetValue(obj.GetType(), out var pool)) {
                pool.Return(obj);
            }
        }
        
        public void ClearAllPools()
        {
            foreach (var pool in mPools.Values) {
                pool.Clear();
            }
            mPools.Clear();
        }
    }
    public class DictionaryPoolMgr : ADictionaryPoolMgr<DictionaryPoolMgr> { }
}