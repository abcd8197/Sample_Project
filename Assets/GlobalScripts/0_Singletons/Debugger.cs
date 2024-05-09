namespace Codejay
{
    namespace Debug
    {
        using UnityEngine;


        public class Debugger : MonoBehaviour
        {
            private const string ERROR_LABEL = "[DEVELOPER ERROR]\n";
            private static System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();

            public static void LoggingCustomException(System.Exception e, string message = "", EDebugLogType logType = EDebugLogType.Normal)
            {
                _stringBuilder.Clear();
                _stringBuilder.Append(ERROR_LABEL);
                _stringBuilder.Append(e.Message);
                _stringBuilder.Append('\n');

                switch (logType)
                {
                    case EDebugLogType.Normal:
                        Debug.Log(_stringBuilder.ToString());
                        break;
                    case EDebugLogType.Warning:
                        Debug.LogWarning(_stringBuilder.ToString());
                        break;
                    case EDebugLogType.Error:
                        Debug.LogError(_stringBuilder.ToString());
                        break;
                }
            }
        }

        public enum EDebugLogType
        {
            Normal, Warning, Error
        }
    }
}