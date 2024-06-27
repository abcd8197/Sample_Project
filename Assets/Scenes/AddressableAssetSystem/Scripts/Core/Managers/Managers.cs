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
            if (sInstance == null)
            {
                sInstance = new GameObject("_Managers").AddComponent<Managers>();
                DontDestroyOnLoad(sInstance.gameObject);
            }

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

    private void OnApplicationQuit()
    {
        if (!IsApplicationQutting)
            IsApplicationQutting = true;
    }
}