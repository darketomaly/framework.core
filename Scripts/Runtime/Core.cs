using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{
    public static partial class Core
    {
        #region Extensions

        #region Logs

        private static string GetNameOrFullName(this System.Type type)
        {
            #if UNITY_EDITOR

            if (EditorPrefs.GetBool(Prefs.Key.FullTypePath, false))
            {
                return type.ToString();
            }
            
            #endif

            return type.Name;
        }

        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void Log<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.LogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogWarning<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.WarningLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogImportant<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.ImportantLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogError<T>(this T contextObject, string message)
        {
            Debug.LogError($"<color=#{Prefs.GetColorString(Prefs.Key.ErrorLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
        }
        
        #endregion
        
        /// <returns>
        /// Returns a default (which can be null for reference types, or the default for value types)
        /// if the object could not be casted or the index is out of bounds.
        /// </returns>
        public static T SafeGetAt<T>(this object[] objs, int index)
        {
            if (objs != null && objs.Length > index && index >= 0)
            {
                object obj = objs[index];
                
                if(obj is T obj1)
                {
                    return obj1;
                }
            }
            
            return default;
        }
        
        #endregion

        public static class Prefs
        {
            [Flags]
            public enum Logging
            {
                None = 0,
                Everything = ~0,
                Utility = 1 << 1,
                EditorUtility = 1 << 2,
                UnityExtensions = 1 << 3,
                Mathematics = 1 << 4
            }
            
            private static readonly Dictionary<string, string> Keys = new Dictionary<string, string>()
            {
                {Key.LogColor, "627bc4"},
                {Key.ImportantLogColor, "b988d1"},
                {Key.WarningLogColor, "ab953c"},
                {Key.ErrorLogColor, "d9766f"}
            };
            
            public class Key
            {
                public const string LogColor = "LogColor";
                public const string ImportantLogColor = "ImportantLogColor";
                public const string WarningLogColor = "WarningLogColor";
                public const string ErrorLogColor = "ErrorLogColor";
                
                public const string FullTypePath = "FullTypeName";
                
                public const string Logs = "Logs";
            }

            #if UNITY_EDITOR
            
            [MenuItem("Tools/Framework/Clear Preferences")]
            public static void Clear()
            {
                EditorPrefs.DeleteKey(Key.LogColor);
                EditorPrefs.DeleteKey(Key.ImportantLogColor);
                EditorPrefs.DeleteKey(Key.WarningLogColor);
                EditorPrefs.DeleteKey(Key.ErrorLogColor);
                EditorPrefs.DeleteKey(Key.Logs);
            }
            
            public static bool LogsEnabled(Logging logType)
            {
                var flags = EditorPrefs.GetInt(Prefs.Key.Logs, 0);
                var logging = (Logging)flags;
                return logging.HasFlag(logType);
            }
            
            #endif
            
            public static string GetColorString(string key)
            {
                #if !UNITY_EDITOR

                return Keys[key];
                
                #else
                
                return EditorPrefs.GetString(key, Keys[key]);
                
                #endif
            }

            public static Color GetColor(string key)
            {
                var colorString = Keys[key];
                
                #if UNITY_EDITOR

                colorString = EditorPrefs.GetString(key, Keys[key]);
                
                #endif
                
                ColorUtility.TryParseHtmlString($"#{colorString}", out Color parsedColor);
                
                return parsedColor;
            }
        }
    }
}