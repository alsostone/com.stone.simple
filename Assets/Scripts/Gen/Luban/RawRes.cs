
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using Luban;


namespace cfg
{
public sealed partial class RawRes : Luban.BeanBase
{
    public RawRes(ByteBuf _buf) 
    {
        Key = _buf.ReadInt();
        Path = _buf.ReadString();
    }

    public static RawRes DeserializeRawRes(ByteBuf _buf)
    {
        return new RawRes(_buf);
    }

    public readonly int Key;
    /// <summary>
    /// 路径
    /// </summary>
    public readonly string Path;
   
    public const int __ID__ = -1854168200;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(ConcurrentDictionary<Type, ITableSingleton> tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "key:" + Key + ","
        + "path:" + Path + ","
        + "}";
    }
}

}

