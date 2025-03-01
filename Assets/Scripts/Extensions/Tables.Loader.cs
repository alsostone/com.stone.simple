using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ET;
using ST.Common;
using UnityEngine;
using YooAsset;

namespace Luban
{
    public class TablesLoader : Singleton<TablesLoader>
    {
        private readonly ConcurrentDictionary<Type, ITableSingleton> tableMapping = new();
        
        public async ETTask Reload(Type type)
        {
            tableMapping.TryGetValue(type, out var table);
            table?.Destroy();
            
            using var handle = YooAssets.LoadAssetAsync<TextAsset>(type.Name);
            await handle.Task;
            
            var byteBuf = new ByteBuf((handle.AssetObject as TextAsset)?.bytes);
            table = (ITableSingleton)Activator.CreateInstance(type, byteBuf);
            table.Register();

            tableMapping[type] = table;
        }
		
        public async ETTask LoadAsync()
        {
            tableMapping.Clear();

            var tasks = ListPoolMgr.Instance.Get<List<Task>>();
            var typeBufMapping = await LoadTablesAssetAsync();
            foreach (var type in typeBufMapping.Keys)
            {
                var byteBuf = typeBufMapping[type];
                var task = Task.Run(() =>
                {
                    var table = (ITableSingleton)Activator.CreateInstance(type, byteBuf);
                    table.Register();
                    
                    tableMapping[type] = table;
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            ListPoolMgr.Instance.Return(tasks);
            
            foreach (var table in tableMapping.Values)
            {
                table.ResolveRef(tableMapping);
            }
        }
        
        private async ETTask<Dictionary<Type, ByteBuf>> LoadTablesAssetAsync()
        {
            var result = new Dictionary<Type, ByteBuf>();
            foreach (var type in cfg.Tables.TableTypes)
            {
                using var handle = YooAssets.LoadAssetAsync<TextAsset>(type.Name);
                await handle.Task;
                result[type] = new ByteBuf((handle.AssetObject as TextAsset)?.bytes);
            }
            return result;
        }
    }
}