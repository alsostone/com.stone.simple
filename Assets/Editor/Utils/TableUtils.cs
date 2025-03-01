using System;
using System.IO;
using UnityEngine;
using Luban;

namespace ST.Editor
{
    internal static class TableUtils
    {
        private static readonly string PathFormat = Application.dataPath + "/res/datas/bytes/{0}.bytes";
        
        internal static T LoadTable<T>()
        {
            var bytes = File.ReadAllBytes(string.Format(PathFormat, typeof(T).Name));
            var instance = (T)Activator.CreateInstance(typeof(T), new ByteBuf(bytes));
            return instance;
        }
        
    }
}