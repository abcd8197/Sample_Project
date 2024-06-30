namespace AddressableAssetSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Managers : MonoBehaviour
    {
        private static Managers sInstance;
        public static Managers Instance
        {
            get
            {
                if (IsApplicationQutting)
                    return null;

                return sInstance;
            }
        }

        #region Managers
        private ResourceManager _resource;
        private SceneTransitionManager _scene;

        public static ResourceManager Resource { get => Instance._resource; }
        public static SceneTransitionManager Scene { get => Instance._scene; }
        #endregion

        #region Fields
        private static bool IsApplicationQutting = false;
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "AddressableAssetSystem_1")
                return;

            if (sInstance == null)
            {
                sInstance = new GameObject("_Managers").AddComponent<Managers>();
                DontDestroyOnLoad(sInstance.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            IsApplicationQutting = true;
        }
    }
}