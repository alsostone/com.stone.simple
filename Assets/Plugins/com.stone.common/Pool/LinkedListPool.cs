using System;
using System.Collections.Generic;


namespace ST.Common
{
    public class LinkedListPool<T> : IPool
    {
        private readonly Stack<LinkedList<T>> mPools;
        private int mTotalCount;

        public LinkedListPool()
        {
            mPools = new Stack<LinkedList<T>>(128);
        }
        
        public int CurCount => mPools.Count;

        public int TotalCount => mTotalCount;

        public LinkedList<T> Get()
        {
            if (mPools.Count > 0) {
                var obj = mPools.Pop();
                return obj;
            } else {
                var obj = Activator.CreateInstance<LinkedList<T>>();
                mTotalCount++;
                return obj;
            }
        }
        
        private void Return(LinkedList<T> obj)
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
            Return((LinkedList<T>)obj);
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