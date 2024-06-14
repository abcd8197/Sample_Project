namespace Player_MVC_FSM
{
    using UnityEngine;
    public class GlobalInputManager : MonoBehaviour
    {
        public static GlobalInputManager Instance = null;

        public static UnityEngine.Events.UnityAction<Vector3> OnMove;
        public static UnityEngine.Events.UnityAction OnLeftShiftDown;
        public static UnityEngine.Events.UnityAction OnLeftShiftUp;
        public static UnityEngine.Events.UnityAction OnSpaceBarDown;

        private Vector3 m_prevMoveDirection = Vector3.zero;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {

            if (Instance == null)
            {
                Instance = new GameObject("_InputManager").AddComponent<GlobalInputManager>();
                DontDestroyOnLoad(Instance.gameObject);
            }
        }

        private void Update()
        {
            if (Input.anyKey || Input.anyKeyDown)
            {
                Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                dir = dir.normalized;
                OnMove?.Invoke(dir);
                m_prevMoveDirection = dir;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                    OnLeftShiftDown?.Invoke();
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                    OnLeftShiftUp?.Invoke();
            }
            else if (m_prevMoveDirection != Vector3.zero)
            {
                OnMove?.Invoke(Vector3.zero);
                m_prevMoveDirection = Vector3.zero;
            }
        }
    }
}