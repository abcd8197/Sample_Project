namespace GlobalEventSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using GlobalEventSystem;
    using Codejay.Debug;

    
    public class EventManager : MonoBehaviour
    {
        #region Singleton
        private static EventManager sInstace;
        public static EventManager Instance
        {
            get
            {
                if (IsApplicationQuitting)
                    return null;

                return sInstace;
            }
        }

        public static bool IsApplicationQuitting = false;
        #endregion

        private Dictionary<EEventKey, Delegate> _dicEvents = new Dictionary<EEventKey, Delegate>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GlobalEventSystem")
                return;
            if (sInstace == null)
            {
                sInstace = new GameObject("_Event").AddComponent<EventManager>();
                DontDestroyOnLoad(sInstace.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            IsApplicationQuitting = true;
        }

        private void OnDestroy()
        {
            if (_dicEvents != null)
            {
                _dicEvents.Clear();
            }
        }

        #region None Parameter
        public void Subscribe(EEventKey eventType, Action listener)
        {
            if (_dicEvents.TryGetValue(eventType, out Delegate existingDelegate))
            {
                existingDelegate = Delegate.Combine(existingDelegate, listener);
                _dicEvents[eventType] = existingDelegate;
            }
            else
                _dicEvents.Add(eventType, listener);
        }

        public void Unsubscribe(EEventKey eventType, Action listener)
        {
            if (_dicEvents.TryGetValue(eventType, out var del))
            {
                del = Delegate.Remove(del, listener);

                if (del == null)
                {
                    _dicEvents.Remove(eventType);
                }
                else
                {
                    _dicEvents[eventType] = del;
                }
            }
        }
        public void Publish(EEventKey eventType)
        {
            if (_dicEvents.TryGetValue(eventType, out Delegate delegates))
            {
                try
                {
                    foreach (var individualDelegate in delegates.GetInvocationList())
                    {
                        Action action = (Action)individualDelegate;
                        action.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    Debugger.LoggingCustomException(ex, $"Failed to Publish Event: {eventType}\n");
                }
            }
        }
        #endregion

        #region Generic Parameter
        public void Subscribe<T>(EEventKey eventType, Action<T> listener)
        {
            if (_dicEvents.TryGetValue(eventType, out Delegate existingDelegate))
            {
                existingDelegate = Delegate.Combine(existingDelegate, listener);
                _dicEvents[eventType] = existingDelegate;
            }
            else
                _dicEvents.Add(eventType, listener);
        }

        public void Unsubscribe<T>(EEventKey eventType, Action<T> listener)
        {
            if (_dicEvents.TryGetValue(eventType, out var del))
            {
                del = Delegate.Remove(del, listener);

                if (del == null)
                {
                    _dicEvents.Remove(eventType);
                }
                else
                {
                    _dicEvents[eventType] = del;
                }
            }
        }
        public void Publish<T>(EEventKey eventType, T data)
        {
            if (_dicEvents.TryGetValue(eventType, out Delegate delegates))
            {
                try
                {
                    foreach (var individualDelegate in delegates.GetInvocationList())
                    {
                        Action<T> action = (Action<T>)individualDelegate;
                        action.Invoke(data);
                    }
                }
                catch (Exception ex)
                {
                    Debugger.LoggingCustomException(ex, $"Failed to Publish Event: {eventType}\n");
                }
            }
        }
        #endregion

#if UNITY_EDITOR
        public void ShowAllListnerCounts()
        {
            var strBuilder = new System.Text.StringBuilder("==== 모든 리스너의 개수 ====\n");
            foreach (var pair in _dicEvents)
            {
                strBuilder.AppendLine($"{pair.Key}: 개수: {((Delegate)pair.Value).GetInvocationList().Length}");
            }
            Debug.Log(strBuilder.ToString());
        }
#endif
    }
}