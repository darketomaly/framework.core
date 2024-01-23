#if UNITY_EDITOR

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

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

            /// <summary>
            /// Gets the path of a given class type
            /// </summary>
            /// <param name="classTypeOf">Use typeof() to grab class type.</param>
            /// <returns></returns>
            public static bool GetScriptPath(System.Type classTypeOf, out string path)
            {
                return DoGetScriptPath(classTypeOf, out path);
            }

            /// <summary>
            /// Gets the path of a given class. For static classes, use `GetScriptPath(System.Type, out string)`
            /// </summary>
            public static bool GetScriptPath<T>(out string path)
            {
                return DoGetScriptPath(typeof(T), out path);
            }

            private static bool DoGetScriptPath(System.Type scriptTypeOf, out string path)
            {
                path = string.Empty;

                string targetAssemblyName = string.Empty;

                string[] guids = AssetDatabase.FindAssets($"t:script");
                
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type == scriptTypeOf)
                        {
                            targetAssemblyName = type.Assembly.GetName().Name;
                            break;
                        }
                    }
                }
                
                foreach (var guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    string txt = File.ReadAllText(assetPath);
                    
                    var splits = scriptTypeOf.FullName.Split('+', '.');
                    string tmp = string.Empty;
                    int totalSuccess = splits.Length;
                    int success = 0;
                    
                    foreach (var split in splits)
                    {
                        string pattern = @"\b" + split + @"\b";

                        if (MatchPattern(txt, pattern))
                        {
                            success++;
                        }
                    }
                    
                    if (success == totalSuccess)
                    {
                        string assemblyName = Path.GetFileName(CompilationPipeline.GetAssemblyNameFromScriptPath(assetPath));
                        assemblyName = assemblyName.Replace(".dll", string.Empty);

                        if (targetAssemblyName == assemblyName)
                        {
                            path = assetPath;
                            return true;
                        }
                    }
                }
                
                return false;

                bool MatchPattern(string input, string pattern)
                {
                    Match matchNamespace = Regex.Match(input, pattern);
                    return matchNamespace.Success;
                }
            }

            public static UnityEditor.Build.NamedBuildTarget GetCurrentNamedBuildTarget()
            {
                BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
                BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                return UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            }
            
            public static void RemoveFromDefineSymbols(params string[] symbolsToRemove)
            {
                string currentSymbols = PlayerSettings.GetScriptingDefineSymbols(GetCurrentNamedBuildTarget());
                string updatedSymbols = currentSymbols;

                foreach (string symbolToRemove in symbolsToRemove)
                {
                    updatedSymbols = updatedSymbols.Replace(symbolToRemove + ";", "");
                    updatedSymbols = updatedSymbols.Replace(symbolToRemove, "");
                }
                
                Debug.Log($"Symbols should now be: {updatedSymbols}");
            
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, updatedSymbols);
                AssetDatabase.Refresh();
                UnityEditor.EditorUtility.RequestScriptReload();
            }
        }
    }
}

#endif