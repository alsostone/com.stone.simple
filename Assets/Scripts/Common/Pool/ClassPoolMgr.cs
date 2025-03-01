using System;
using System.Collections.Generic;

namespace ST.Common
{
    public abstract class ClassPoolMgr<T0> : Singleton<T0> where T0 : new()
    {
        private readonly Dictionary<Type, IPool> mPools = new Dictionary<Type, IPool>();

        public T GetPool<T>()
        {
            mPools.TryGetValue(typeof(T), out var pool);
            return (T)pool;
        }
        
        public T Get<T>() where T : class, IPoolObject
        {
            Type t = typeof(T);
            if (!mPools.TryGetValue(t, out var pool)) {
                pool = new ClassPool<T>();
                mPools.Add(t, pool);
            }
            return ((ClassPool<T>)pool).Get();
        }
        
        public void Return<T>(T obj) where T : class, IPoolObject
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