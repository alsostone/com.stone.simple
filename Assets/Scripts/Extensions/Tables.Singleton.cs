using System;
using System.Collections.Concurrent;
using ST.Common;
using UnityEngine;
using YooAsset;

namespace Luban
{
    public interface ITable
    {
        void ResolveRef(ConcurrentDictionary<Type, ITableSingleton> tables);
    }

    public interface ITableSingleton: ITable
    {
        void Register();
        void Destroy();
        bool IsDisposed();
    }
    
    // 1.在未被外部初始化时获取单例 会使用YooAssets加载二进制解析为实例（同步模式）
    // 2.也可以在使用单例前 通过TablesLoader初始化表的单例（异步模式）
    public abstract class TableSingleton<T>: ITableSingleton where T: TableSingleton<T>
    {
        private bool isDisposed;

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    using var handle = YooAssets.LoadAssetSync<TextAsset>(typeof(T).Name);
                    var bytes = (handle.AssetObject as TextAsset)?.bytes;
                    instance = Activator.CreateInstance(typeof(T), new ByteBuf(bytes)) as T;
                }
                return instance;
            }
        }

        void ITableSingleton.Register()
        {
            if (instance != null)
            {
                SLogger.Error($"singleton register twice! {typeof (T).Name}");
            }
            instance = (T)this;
        }

        void ITableSingleton.Destroy()
        {
            if (this.isDisposed)
            {
                return;
            }
            this.isDisposed = true;
            instance = null;
        }

        bool ITableSingleton.IsDisposed()
        {
            return this.isDisposed;
        }

        public abstract void ResolveRef(ConcurrentDictionary<Type, ITableSingleton> tables);
    }
}