namespace ST.Common
{
    public static class SLogger
    {
        public static void SetLogEnable(bool enable) => UnityEngine.Debug.unityLogger.logEnabled = enable;

        public static void Debug(string content)
        {
            content = "[debug]" + content;
            UnityEngine.Debug.Log((object) content);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            format = "[debug]" + format;
            UnityEngine.Debug.LogFormat(format, args);
        }

        public static void Info(string content)
        {
            content = "[info]" + content;
            UnityEngine.Debug.Log((object) content);
        }

        public static void InfoFormat(string format, params object[] param)
        {
            format = "[info]" + format;
            UnityEngine.Debug.LogFormat(format, param);
        }

        public static void Warning(string content)
        {
            content = "[warning]" + content;
            UnityEngine.Debug.LogWarning((object) content);
        }

        public static void WarningFormat(string format, params object[] param)
        {
            format = "[warning]" + format;
            UnityEngine.Debug.LogWarningFormat(format, param);
        }

        public static void Error(string content)
        {
            content = "[error]" + content;
            UnityEngine.Debug.LogError((object) content);
        }

        public static void ErrorFormat(string format, params object[] param)
        {
            format = "[error]" + format;
            UnityEngine.Debug.LogErrorFormat(format, param);
        }

        public static void Custom(string content)
        {
            content = "[custom]" + content;
            UnityEngine.Debug.Log((object) content);
        }

        public static void CustomFormat(string format, params object[] param)
        {
            format = "[custom]" + format;
            UnityEngine.Debug.LogFormat(format, param);
        }
    }
}