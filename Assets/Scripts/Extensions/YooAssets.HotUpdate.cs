using ST.Common;
using UnityEngine;
using YooAsset;

namespace ET
{
    public static partial class YooAssetHelper
    {
        public static async ETTask<bool> HotUpdateAsync(string packageName)
        {
            var package = YooAssets.GetPackage(packageName);
            
            // 请求资源版本
            var versionOperation = package.RequestPackageVersionAsync();
            await versionOperation.Task;
            
            if (versionOperation.Status != EOperationStatus.Succeed)
            {
                SLogger.Error(versionOperation.Error);
                return false;
            }
            SLogger.Info($"Request package version : {versionOperation.PackageVersion}");
            
            // 更新资源清单
            var manifestOperation = package.UpdatePackageManifestAsync(versionOperation.PackageVersion);
            await manifestOperation.Task;

            if (manifestOperation.Status != EOperationStatus.Succeed)
            {
                SLogger.Error(manifestOperation.Error);
                return false;
            }
            
            // 创建资源下载器
            var downloader = package.CreateResourceDownloader(10, 3);
            if (downloader.TotalDownloadCount == 0) {
                SLogger.Info("Not found any download files !");
            }
            else
            {
                var sizeMb = downloader.TotalDownloadBytes / 1048576f;
                sizeMb = Mathf.Clamp(sizeMb, 0.1f, float.MaxValue);
                SLogger.Info($"Found update patch files, Total count {downloader.TotalDownloadCount} Total size {sizeMb:f1}MB");
                
                // 开始下载资源文件
                downloader.DownloadUpdateCallback = data =>
                {
                    EventMgr.Instance.SendEvent(EventDefine.DownloadUpdate, data.TotalDownloadCount, data.CurrentDownloadCount, data.TotalDownloadBytes, data.CurrentDownloadBytes);
                };
                downloader.BeginDownload();
                await downloader.Task;
                
                if (downloader.Status != EOperationStatus.Succeed)
                {
                    SLogger.Error(downloader.Error);
                    return false;
                }
                
                // 清理未使用的缓存文件
                var clearOperation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
                await clearOperation.Task;
                
                if (clearOperation.Status != EOperationStatus.Succeed)
                {
                    SLogger.Error(clearOperation.Error);
                    return false;
                }
            }
            return true;
        }
    }
}