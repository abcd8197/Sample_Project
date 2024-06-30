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
        private AddressableAssetSystem.AddressableAssetManager _addressable;
        private AddressableAssetSystem.SceneTransitionManager _scene;
        private AddressableAssetSystem.ResourceManager _resource;


        public static AddressableAssetSystem.AddressableAssetManager Addressable { get => Instance._addressable; }
        public static AddressableAssetSystem.SceneTransitionManager Scene { get => Instance._scene; }
        public static AddressableAssetSystem.ResourceManager Resource { get => Instance._resource; }
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