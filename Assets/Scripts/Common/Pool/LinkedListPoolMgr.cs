using System;
using System.Collections.Generic;


namespace ST.Common
{
    public abstract class ALinkedListPoolMgr<T0> : Singleton<T0> where T0 : new()
    {
        private readonly Dictionary<Type, IPool> mPools = new Dictionary<Type, IPool>();

        public LinkedList<T> Get<T>()
        {
            IPool pool = null;
            Type t = typeof(T);
            if (!mPools.TryGetValue(typeof(T), out pool)) {
                pool = new LinkedListPool<T>();
                mPools.Add(t, pool);
            }
            return ((LinkedListPool<T>)pool).Get();
        }
        
        public void Return<T>(T obj)
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
    public class LinkedListPoolMgr : ALinkedListPoolMgr<LinkedListPoolMgr> { }
}