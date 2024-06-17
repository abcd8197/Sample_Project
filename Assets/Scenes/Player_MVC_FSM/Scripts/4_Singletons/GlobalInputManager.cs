namespace TPSPlayerController_Scene
{
    using UnityEngine;
    public class GlobalInputManager : MonoBehaviour
    {
        public static GlobalInputManager Instance = null;

        public static UnityEngine.Events.UnityAction<Vector3> OnMove;
        public static UnityEngine.Events.UnityAction OnMoveStart;
        public static UnityEngine.Events.UnityAction OnMoveEnd;
        public static UnityEngine.Events.UnityAction<Vector3> OnMouseMove;
        public static UnityEngine.Events.UnityAction OnLeftShiftDown;
        public static UnityEngine.Events.UnityAction OnLeftShiftUp;
        public static UnityEngine.Events.UnityAction OnSpaceBarDown;

        private bool m_isMove = false;
        public Vector3 m_prevMoveDirection { get; private set; } = Vector3.zero;
        public Vector2 m_prevMouseAxis { get; private set; } = Vector2.zero;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TPSPlayerController")
                return;
            if (Instance == null)
            {
                Instance = new GameObject("_InputManager").AddComponent<GlobalInputManager>();
                DontDestroyOnLoad(Instance.gameObject);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
#if UNITY_STANDALONE
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (mouse != m_prevMouseAxis)
            {
                m_prevMouseAxis = mouse.normalized;
                OnMouseMove?.Invoke(mouse.normalized);
            }
#endif

            if (Input.anyKey || Input.anyKeyDown)
            {
                if (!m_isMove)
                {
                    m_isMove = true;
                    OnMoveStart?.Invoke();
                }

                Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                dir = dir.normalized;
                OnMove?.Invoke(dir);
                m_prevMoveDirection = dir;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                    OnLeftShiftDown?.Invoke();
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                    OnLeftShiftUp?.Invoke();
            }
            else if (m_prevMoveDirection != Vector3.zero && m_isMove)
            {
                m_isMove = false;
                OnMove?.Invoke(Vector3.zero);
                OnMoveEnd?.Invoke();
                m_prevMoveDirection = Vector3.zero;
            }
        }
    }
}