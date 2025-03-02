using System;
using System.Collections;
using System.Collections.Generic;

namespace ST.Common
{
    public class ListPoolMgr : AListPoolMgr<ListPoolMgr> { }
    
    public abstract class AListPoolMgr<T0> : Singleton<T0> where T0 : new()
    {
        private readonly Dictionary<Type, IPool> mPools = new Dictionary<Type, IPool>();

        public T Get<T>() where T : class, IList
        {
            IPool pool = null;
            Type t = typeof(T);
            if (!mPools.TryGetValue(typeof(T), out pool)) {
                pool = new ListPool<T>();
                mPools.Add(t, pool);
            }
            return ((ListPool<T>)pool).Get();
        }
        
        public void Return<T>(T obj) where T : class, IList
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
}