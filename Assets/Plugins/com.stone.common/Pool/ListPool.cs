using System;
using System.Collections;
using System.Collections.Generic;

namespace ST.Common
{
    public class ListPool<T> : IPool where T : class, IList
    {
        private readonly Stack<T> mPools;
        private int mTotalCount;

        public ListPool()
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
            if (obj.Count > 20) {
                SLogger.WarningFormat("对象池中的容器元素数量大于20:{0}", typeof(T));
            }
#endif
            obj.Clear();
            mPools.Push(obj);
        }
        
        public void Return(object obj)
        {
            Return((T)obj);
        }
        
        public void Clear()
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