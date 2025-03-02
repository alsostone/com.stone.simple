using System;
using System.Collections.Generic;
using System.Reflection;

namespace ST.Common
{
    public class EventMgr : AEventManager<EventMgr> { }
    
    public abstract class AEventManager<T> : Singleton<T> where T : new()
    {
        private readonly Dictionary<int, List<EventListener>> mEventListeners = new Dictionary<int, List<EventListener>>();
        private readonly Dictionary<int, List<EventListener>> mWaitRemoveListener = new Dictionary<int, List<EventListener>>();
        private readonly List<AEventPostWrapper> mEventPostWrappers = new List<AEventPostWrapper>(32);
        private int mDispatchRef;
        
        private void AddEventListener(int key, Delegate callback)
        {
            var evt = ClassPoolMgr.Instance.Get<EventListener>();
            evt.Action = callback;
            evt.Removed = false;

            if (!mEventListeners.TryGetValue(key, out var listener)) {
                listener = new List<EventListener>();
                mEventListeners.Add(key, listener);
            }

            listener.Add(evt);
        }
        private void RemoveEventListener(int key, Delegate callback)
        {
            if (!mEventListeners.TryGetValue(key, out var events)) { return; }

            for (var i = 0; i < events.Count; i++) {
                var evt = events[i];
                if (!evt.Removed && evt.Action == callback) {
                    if (mDispatchRef <= 0) {
                        events.RemoveAt(i);
                        ClassPoolMgr.Instance.Return(evt);
                    }
                    else {
                        evt.Removed = true;
                        if (!mWaitRemoveListener.TryGetValue(key, out var listener)) {
                            listener = new List<EventListener>();
                            mWaitRemoveListener.Add(key, listener);
                        }
                        listener.Add(evt);
                    }
                    break;
                }
            }
        }

        #region add & remove
        public void AddEventListener(int key, Action callBack) { AddEventListener(key, (Delegate)callBack); }
        public void AddEventListener<T1>(int key, Action<T1> callBack) { AddEventListener(key, (Delegate)callBack); }
        public void AddEventListener<T1, T2>(int key, Action<T1, T2> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3>(int key, Action<T1, T2, T3> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4>(int key, Action<T1, T2, T3, T4> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5>(int key, Action<T1, T2, T3, T4, T5> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5, T6>(int key, Action<T1, T2, T3, T4, T5, T6> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void AddEventListener<T1, T2, T3, T4, T5, T6, T7>(int key, Action<T1, T2, T3, T4, T5, T6, T7> callBack)
        {
            AddEventListener(key, (Delegate)callBack);
        }
        public void RemoveListener(int key, Action callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1>(int key, Action<T1> callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1, T2>(int key, Action<T1, T2> callBack) { RemoveEventListener(key, callBack); }
        public void RemoveListener<T1, T2, T3>(int key, Action<T1, T2, T3> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4>(int key, Action<T1, T2, T3, T4> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4, T5>(int key, Action<T1, T2, T3, T4, T5> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        public void RemoveListener<T1, T2, T3, T4, T5, T6>(int key, Action<T1, T2, T3, T4, T5, T6> callBack)
        {
            RemoveEventListener(key, callBack);
        }
        #endregion
        
        #region send 实时广播
        public void SendEvent<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4, T5, T6> action) {
                            action(arg1, arg2, arg3, arg4, arg5, arg6);
                        }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4, T5> action) { action(arg1, arg2, arg3, arg4, arg5); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3, T4> action) { action(arg1, arg2, arg3, arg4); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2, T3> action) { action(arg1, arg2, arg3); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent<T1, T2>(int key, T1 arg1, T2 arg2)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1, T2> action) { action(arg1, arg2); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent<T1>(int key, T1 arg1)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action<T1> action) { action(arg1); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }

                mDispatchRef--;
            }
        }
        public void SendEvent(int key)
        {
            if (mEventListeners.TryGetValue(key, out var events)) {
                mDispatchRef++;
                for (var i = 0; i < events.Count; i++) {
                    var evt = events[i];
                    if (!evt.Removed) {
                        if (evt.Action is Action action) { action(); }
                        else { SLogger.Error($"Listener Method{evt.Action.GetMethodInfo()} Not Match Event:{key} "); }
                    }
                }
                mDispatchRef--;
            }
        }
        #endregion
        
        #region post 延迟广播
        public void PostEvent<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1, T2, T3, T4, T5, T6>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            wrapper.Arg2 = arg2;
            wrapper.Arg3 = arg3;
            wrapper.Arg4 = arg4;
            wrapper.Arg5 = arg5;
            wrapper.Arg6 = arg6;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1, T2, T3, T4, T5>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            wrapper.Arg2 = arg2;
            wrapper.Arg3 = arg3;
            wrapper.Arg4 = arg4;
            wrapper.Arg5 = arg5;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1, T2, T3, T4>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            wrapper.Arg2 = arg2;
            wrapper.Arg3 = arg3;
            wrapper.Arg4 = arg4;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1, T2, T3>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            wrapper.Arg2 = arg2;
            wrapper.Arg3 = arg3;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent<T1, T2>(int key, T1 arg1, T2 arg2)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1, T2>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            wrapper.Arg2 = arg2;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent<T1>(int key, T1 arg1)
        {
            var wrapper = ClassPoolMgr.Instance.Get<EventPostWrapper<T1>>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            wrapper.Arg1 = arg1;
            mEventPostWrappers.Add(wrapper);
        }
        public void PostEvent(int key)
        {
            var wrapper = ClassPoolMgr.Instance.Get<AEventPostWrapper>();
            wrapper.Key = key;
            wrapper.FrameCount = UnityEngine.Time.frameCount;
            mEventPostWrappers.Add(wrapper);
        }
        #endregion
        
        public virtual void Update()
        {
            for (var i = mEventPostWrappers.Count - 1; i >= 0; i--)
            {
                var wrapper = mEventPostWrappers[i];
                if (UnityEngine.Time.frameCount > wrapper.FrameCount)
                {
                    wrapper.SendEvent(this);
                    mEventPostWrappers.RemoveAt(i);
                    ClassPoolMgr.Instance.Return(wrapper);
                }
            }
        }
        
        public virtual void LateUpdate()
        {
            foreach (var pair in mWaitRemoveListener) {
                if (mEventListeners.TryGetValue(pair.Key, out var listeners)) {
                    foreach (var evt in pair.Value) {
                        listeners.Remove(evt);
                        ClassPoolMgr.Instance.Return(evt);
                    }
                }
            }
            mWaitRemoveListener.Clear();
        }

        public virtual void Release()
        {
            foreach (var wrapper in mEventPostWrappers)
            {
                ClassPoolMgr.Instance.Return(wrapper);
            }
            mEventPostWrappers.Clear();
            mWaitRemoveListener.Clear();
            foreach (var pair in mEventListeners)
            {
                foreach (var eventObject in pair.Value)
                {
#if UNITY_EDITOR
                    // 在销毁时，如果事件未由注册者主动清除，输出错误日志
                    SLogger.Error($"事件未清除：{pair.Key}:{eventObject.Action.GetMethodInfo()}");
#endif
                    ClassPoolMgr.Instance.Return(eventObject);
                }
            }
            mEventListeners.Clear();
            mDispatchRef = 0;
        }

    }
}