#if UNITY_EDITOR

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
            public static class ScriptGeneration
            {
                public enum Exposure
                {
                    Public,
                    Private
                }
                
                public enum Modifier
                {
                    None,
                    Static,
                    Abstract
                }

                [MenuItem("Tools/Test Script Generation")]
                public static void Test()
                {
                    string content = string.Empty;
                    string outsideContent = BuildEnum("MyEnumName", new[] { "Test1", "Test2" });
                    outsideContent += "\n\n";
                    outsideContent += BuildEnum("MyEnumName2", new[] { "Test1", "Test2" });
                    Generate(typeof(Framework.Core).ToString(), "MyClassName", Exposure.Public, Modifier.None, 
                        content, outsideContent, "");
                }
                
                public static void Generate(string desiredNamespace, string className, Exposure classExposure, Modifier classType, string content, string outsideContent, string path)
                {
                    string exposure = classExposure switch
                    {
                        Exposure.Public => "public",
                        Exposure.Private => "private",
                        _ => "public"
                    };
                    
                    string modifier = classType switch
                    {
                        Modifier.Static => "static",
                        Modifier.Abstract => "abstract",
                        _ => string.Empty
                    };

                    string text = $"namespace {desiredNamespace}" + "\n{\n"; //Namespace declaration
                    text += $"\t{exposure} {modifier} class {className}\n"; //Class declaration
                    text += "\t{\n"; //Class start

                    if (!string.IsNullOrEmpty(content))
                    {
                        string[] lines = content.Split('\n');
                        foreach (string line in lines) text += $"\t\t{line}\n";
                    }
                    
                    text += "\t}\n"; //Class end
                    
                    if (!string.IsNullOrEmpty(outsideContent))
                    {
                        text += "\n";
                        
                        string[] lines = outsideContent.Split('\n');
                        foreach (string line in lines) text += $"\t{line}\n";
                    }
                    
                    text += "}"; //Namespace end

                    Debug.Log($"{text}");
                    
                    //--- Generate

                    try
                    {
                        System.IO.File.WriteAllText(path, text);
                        Debug.Log("Enum script generated at: " + path);
                        AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();
        
                        UnityEditor.EditorUtility.RequestScriptReload();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Caught exception: {e.Message}");
                        throw;
                    }
                }

                public static string BuildEnum(string enumName, string[] enumContents)
                {
                    enumContents = enumContents.Distinct().ToArray();
                    string tmp = string.Empty;

                    tmp += $"public enum {enumName}\n";
                    tmp += "{\n";
                    foreach (var enumContent in enumContents) tmp += $"\t{enumContent},\n";
                    tmp += "}\n";

                    return tmp;
                }
            }
        }
    }
}

#endif