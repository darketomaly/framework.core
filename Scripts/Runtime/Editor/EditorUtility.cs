using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    public static partial class Core
    {
        public static partial class EditorUtility
        {
            public static bool LoadUniquePrefab<T>(out T loadedObject) where T : UnityEngine.Object
            {
                loadedObject = null;
                string storedPath = EditorPrefs.GetString($"Framework-Path-{typeof(T)}");
            
                if (!string.IsNullOrEmpty(storedPath))
                {
                    T prefab = AssetDatabase.LoadAssetAtPath<T>(storedPath);

                    if (prefab)
                    {
                        prefab.Log("Found prefab with pre-stored path.");
                        loadedObject = prefab;
                        return true;
                    }
                }
            
                string[] guids = AssetDatabase.FindAssets("t:Prefab"); 

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    T prefab = AssetDatabase.LoadAssetAtPath<T>(path);

                    if (prefab)
                    {
                        loadedObject = prefab;
                        EditorPrefs.SetString($"Framework-Path-{typeof(T)}", path);
                        prefab.Log("Searched and found prefab.");
                        return true;
                    }   
                }

                return false;
            }

            public static bool GetScriptPath<T>(out string path)
            {
                path = string.Empty;

                string scriptType = typeof(T).ToString();
                int lastIndex = scriptType.LastIndexOf('.') + 1;
                string scriptName = scriptType[lastIndex..];
                
                string[] guids = AssetDatabase.FindAssets($"t:script {scriptName}");

                foreach (var guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    int lastSlashIndex = assetPath.LastIndexOf('/');
                    
                    var substring = assetPath[lastSlashIndex..];
            
                    if (substring == $"/{scriptName}.cs")
                    {
                        path = assetPath;
                        return true;
                    }
                }

                return false;
            }
        }
    }
}