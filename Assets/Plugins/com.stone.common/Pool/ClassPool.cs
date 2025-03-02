using System;
using System.Collections.Generic;

namespace ST.Common
{
    public interface IPoolObject
    {
        void Reset();
    }
    
    public interface IPool
    {
        void Clear();
        void Return(object obj);
    }

    public class ClassPool<T> : IPool where T : class, IPoolObject
    {
        private readonly Stack<T> mPools;
        private int mTotalCount;
        
        public ClassPool()
        {
            mPools = new Stack<T>(128);
        }
        
        public int CurCount => mPools.Count;
        public int TotalCount => mTotalCount;

        public T Get()
        {
            if (mPools.Count > 0) {
                var obj = mPools.Pop();
                return obj;
            } else {
                var obj = Activator.CreateInstance<T>();
                mTotalCount++;
                return obj;
            }
        }
        
        private void Return(T obj)
        {
#if UNITY_EDITOR
            if (obj == null) {
                SLogger.ErrorFormat("对象池中返回了空对象:{0}", typeof(T));
            }
            if (mPools.Contains(obj)) {
                SLogger.ErrorFormat("对象被重复交回对象池:{0}", typeof(T));
            }
#endif
            obj.Reset();
            mPools.Push(obj);
        }
        
        public void Return(object obj)
        {
            Return((T)obj);
        }
        
        public virtual void Clear()
        {
#if ENABLE_DEBUG
            int poolCount = mPools.Count;
            if (poolCount != mTotalCount) {
                SLogger.ErrorFormat("对象池中有未归还的对象:{0}, Count:{1}", typeof(T), mTotalCount - poolCount);
            }
#endif
            mTotalCount = 0;
            mPools.Clear();
        }
    }
}