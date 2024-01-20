using UnityEngine;

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
        
        #endregion
    }
}