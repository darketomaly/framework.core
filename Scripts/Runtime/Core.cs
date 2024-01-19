using System;
using System.Collections;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Framework
{
    public static partial class Core
    {
        #region Extensions

        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void Log<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#627bc4>[{contextObject.GetType()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogWarning<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#ab953c>[{contextObject.GetType()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogImportant<T>(this T contextObject, string message)
        {
            Debug.Log($"<color=#b988d1>[{contextObject.GetType()}] </color>{message}");
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogError<T>(this T contextObject, string message)
        {
            Debug.LogError($"<color=#d9766f>[{contextObject.GetType()}] </color>{message}");
        }

        /// <summary>
        /// Maps a value given from `a` space to `b`.
        /// </summary>
        public static float MapValue(this float x, float a0, float a1, float b0, float b1)
        {
            float value = b0 + (b1 - b0) * ((x - a0) / (a1 - a0));
            return Mathf.Clamp(value, Mathf.Min(b0, b1), Mathf.Max(b0, b1));
        }
        
        #endregion
    }
}