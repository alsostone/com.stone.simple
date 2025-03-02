using System;

namespace ST.Common
{
    public class EventListener : IPoolObject
    {
        public Delegate Action;
        public bool Removed;
        
        public void Reset()
        {
            Action = default;
            Removed = default;
        }
    }
}