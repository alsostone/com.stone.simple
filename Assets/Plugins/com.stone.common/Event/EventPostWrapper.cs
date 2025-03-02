namespace ST.Common
{
    public abstract class AEventPostWrapper : IPoolObject
    {
        public int Key;
        public int FrameCount;

        public virtual void Reset()
        {
            Key = default;
            FrameCount = default;
        }

        public abstract void SendEvent<T>(AEventManager<T> manager) where T : new();
    }
    
    public class EventPostWrapper<T1, T2, T3, T4, T5, T6> : AEventPostWrapper
    {
        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;
        public T4 Arg4;
        public T5 Arg5;
        public T6 Arg6;
        
        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
            Arg2 = default;
            Arg3 = default;
            Arg4 = default;
            Arg5 = default;
            Arg6 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        }
    }
    
    public class EventPostWrapper<T1, T2, T3, T4, T5> : AEventPostWrapper
    {
        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;
        public T4 Arg4;
        public T5 Arg5;

        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
            Arg2 = default;
            Arg3 = default;
            Arg4 = default;
            Arg5 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1, Arg2, Arg3, Arg4, Arg5);
        }
    }
    
    public class EventPostWrapper<T1, T2, T3, T4> : AEventPostWrapper
    {
        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;
        public T4 Arg4;

        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
            Arg2 = default;
            Arg3 = default;
            Arg4 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1, Arg2, Arg3, Arg4);
        }
    }
    
    public class EventPostWrapper<T1, T2, T3> : AEventPostWrapper
    {
        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;

        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
            Arg2 = default;
            Arg3 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1, Arg2, Arg3);
        }
    }
    
    public class EventPostWrapper<T1, T2> : AEventPostWrapper
    {
        public T1 Arg1;
        public T2 Arg2;

        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
            Arg2 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1, Arg2);
        }
    }
    
    public class EventPostWrapper<T1> : AEventPostWrapper
    {
        public T1 Arg1;
        
        public override void Reset()
        {
            base.Reset();
            Arg1 = default;
        }
        
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key, Arg1);
        }
    }
    
    public class EventPostWrapper : AEventPostWrapper
    {
        public override void SendEvent<T>(AEventManager<T> manager)
        {
            manager.SendEvent(Key);
        }
    }

}