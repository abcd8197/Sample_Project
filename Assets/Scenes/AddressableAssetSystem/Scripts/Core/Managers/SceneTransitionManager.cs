namespace AddressableAssetSystem
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneTransitionManager : MonoBehaviour
    {
        public enum ESceneType { BeforeInitialize = -1, Scene_1 = 0, Scene_2, Scene_3, Empty }

        #region ## Fields ##
        private const float MINIMUM_LOADTIME = 2f;
        private ESceneType m_CurrentSceneType = ESceneType.BeforeInitialize;
        #endregion

        #region Properties
        public ESceneType CurrentSceneType { get => m_CurrentSceneType; }
        #endregion

        private void Awake()
        {
            m_CurrentSceneType = (ESceneType)SceneManager.GetActiveScene().buildIndex;
        }

        public void LoadScene(ESceneType nextScene)
        {
            if (m_CurrentSceneType == nextScene)
                return;

            StartCoroutine(coroLoadScene(nextScene));
        }

        private IEnumerator coroLoadScene(ESceneType nextScene)
        {
            // 다음 씬에 필요한 리소스를 로드하기 전 Empty Scene을 불러온다.
            AsyncOperation async = SceneManager.LoadSceneAsync((int)ESceneType.Empty);
            async.allowSceneActivation = true;

            while (async.progress < 0.9f)
            {
                yield return null;
            }

            float loadTimeCount = 0;

        }
    }
}