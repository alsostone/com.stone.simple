using UnityEngine;

namespace ST.Common
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour, IClear where T : MonoBehaviour
    {
        private static T _instance;

        protected bool IsInit { set; get; }
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance != null)
                {
                    return _instance;
                }

                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public abstract void Clear();
    }
}