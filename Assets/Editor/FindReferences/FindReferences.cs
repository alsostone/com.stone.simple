using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using cfg;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ST.Editor
{
    public static class FindReferences
    {
        private const string MetaExtension = ".meta";

        [MenuItem("Assets/Find References &f", false, 10)]
        public static void Find()
        {
            var gameObjects = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
            var table = TableUtils.LoadTable<TbRes>();
            foreach (var go in gameObjects) {
                FindOne(go, table);
            }
        }

        private static bool CheckResTable(Object selectedObject, in TbRes referenceTable)
        {
            var selectedAssetPath = AssetDatabase.GetAssetPath(selectedObject);
            foreach (var item in referenceTable.DataList)
            {
                if (selectedAssetPath.EndsWith(item.Path))
                {
                    return true;
                }
                // if (selectedAssetPath.EndsWith(item.PathHigh))
                // {
                //     return true;
                // }
            }
            return false;
        }

        private static void FindOne(Object selectedObject, in TbRes referenceTable)
        {
            var selectedAssetPath = AssetDatabase.GetAssetPath(selectedObject);
            if (AssetDatabase.IsValidFolder(selectedAssetPath)) {
                return;
            }

            bool isMacOS = Application.platform == RuntimePlatform.OSXEditor;
            int totalWaitMilliseconds = isMacOS ? 2 * 1000 : 300 * 1000;
            int cpuCount = Environment.ProcessorCount;
            string appDataPath = Application.dataPath;
            
            var selectedAssetGuid = AssetDatabase.AssetPathToGUID(selectedAssetPath);
            string selectedAssetMetaPath = selectedAssetPath + MetaExtension;

            var references = new List<string>();
            var output = new StringBuilder();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var psi = new ProcessStartInfo();
            if (isMacOS) {
                psi.FileName = "/usr/bin/mdfind";
                psi.Arguments = string.Format("-onlyin {0} {1}", appDataPath, selectedAssetGuid);
            } else {
                psi.FileName = Path.Combine(Environment.CurrentDirectory, "Assets/Editor/FindReferences/rg.exe");
                psi.Arguments = string.Format("--case-sensitive --follow --files-with-matches --no-text --fixed-strings " +
                                              "--ignore-file Assets/Editor/FindReferences/ignore.txt " +
                                              "--threads {0} --regexp {1} -- {2}",
                    cpuCount, selectedAssetGuid, appDataPath);
            }
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var process = new Process();
            process.StartInfo = psi;

            process.OutputDataReceived += (sender, e) => {
                if (string.IsNullOrEmpty(e.Data)) { return; }
                string relativePath = e.Data.Replace(appDataPath, "Assets").Replace("\\", "/");
                // skip the meta file of whatever we have selected
                if (relativePath == selectedAssetMetaPath) { return; }
                references.Add(relativePath);
            };
            process.ErrorDataReceived += (sender, e) => {
                if (string.IsNullOrEmpty(e.Data)){ return; }
                output.AppendLine("Error: " + e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            while (!process.HasExited) {
                if (stopwatch.ElapsedMilliseconds < totalWaitMilliseconds) {
                    float progress = (float)((double)stopwatch.ElapsedMilliseconds / totalWaitMilliseconds);
                    string info = $"Finding {stopwatch.ElapsedMilliseconds / 1000}/{totalWaitMilliseconds / 1000}s {progress:P2}";
                    bool canceled = EditorUtility.DisplayCancelableProgressBar("Find References in Project", info, progress);
                    if (canceled) {
                        process.Kill();
                        break;
                    }
                    Thread.Sleep(100);
                } else {
                    process.Kill();
                    break;
                }
            }
            foreach (var file in references) {
                var info = $"{AssetDatabase.AssetPathToGUID(file)} {file}";
                output.AppendLine(info);
                var assetPath = file;
                if (file.EndsWith(MetaExtension)) {
                    assetPath = file.Substring(0, file.Length - MetaExtension.Length);
                }
                UnityEngine.Debug.Log(info, AssetDatabase.LoadMainAssetAtPath(assetPath));
            }

            var referenceCount = references.Count;
            if (CheckResTable(selectedObject, referenceTable))
            {
                referenceCount += 1;
                output.AppendLine("from res table");
            }
            
            EditorUtility.ClearProgressBar();
            stopwatch.Stop();
            
            if (referenceCount > 0)
            {
                var content = string.Format("{0} {1} found for object: \"{2}\" path: \"{3}\" guid: \"{4}\" total time: {5}s\n\n{6}",
                    referenceCount, referenceCount > 2 ? "references" : "reference", selectedObject.name, selectedAssetPath,
                    selectedAssetGuid, stopwatch.ElapsedMilliseconds / 1000d, output);
                UnityEngine.Debug.LogWarning(content, selectedObject);
            }
            else {
                var content = string.Format("0 reference found for object. remove it: \"{0}\" path: \"{1}\" guid: \"{2}\" total time: {3}s\n\n{4}",
                     selectedObject.name, selectedAssetPath, selectedAssetGuid, stopwatch.ElapsedMilliseconds / 1000d, output);
                UnityEngine.Debug.LogError(content, selectedObject);
                AssetDatabase.DeleteAsset(selectedAssetPath);
            }
        }
        
    }
}
