namespace UnityEngine
{
    public static class SKSDebug
    {

        private static bool canShowLogs = false;

        public static void Log(object msg)
        {
            if (CanShowLogs)
                Debug.Log(msg);
        }
        public static void Log(object msg, Object context)
        {
            if (CanShowLogs)
                Debug.Log(msg, context);
        }

        public static void LogError(object msg)
        {
            if (CanShowLogs)
                Debug.LogError(msg);
        }
        public static void LogError(object msg, Object context)
        {
            if (CanShowLogs)
                Debug.LogError(msg, context);
        }

        public static void LogWarning(object msg)
        {
            if (CanShowLogs)
                Debug.LogWarning(msg);
        }
        public static void LogWarning(object msg, Object context)
        {
            if (CanShowLogs)
                Debug.LogWarning(msg, context);
        }

        public static void LogWarningFormat(string msg)
        {
            if (CanShowLogs)
                Debug.LogWarningFormat(msg);
        }
        public static void LogWarningFormat(string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogWarningFormat(msg, args);
        }
        public static void LogWarningFormat(Object context, string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogWarningFormat(context, msg, args);
        }

        public static void LogErrorFormat(string msg)
        {
            if (CanShowLogs)
                Debug.LogErrorFormat(msg);
        }
        public static void LogErrorFormat(string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogErrorFormat(msg, args);
        }
        public static void LogErrorFormat(Object context, string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogErrorFormat(context, msg, args);
        }

        public static void LogException(System.Exception msg)
        {
            if (CanShowLogs)
                Debug.LogException(msg);
        }
        public static void LogException(System.Exception msg, Object context)
        {
            if (CanShowLogs)
                Debug.LogException(msg, context);
        }

        public static void LogFormat(string msg)
        {
            if (CanShowLogs)
                Debug.LogFormat(msg);
        }
        public static void LogFormat(string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogFormat(msg, args);
        }
        public static void LogFormat(Object obj, string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogFormat(obj, msg, args);
        }
        public static void LogFormat(LogType logType, LogOption logOption, Object obj, string msg, params object[] args)
        {
            if (CanShowLogs)
                Debug.LogFormat(logType, logOption, obj, msg, args);
        }

        public static bool CanShowLogs
        {
            set
            {
                canShowLogs = value;
            }
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return canShowLogs;
#endif
            }
        }
    }
}