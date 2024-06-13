namespace Player_MVC_FSM
{
    using UnityEngine;
    public class GlobalInputManager : MonoBehaviour
    {
        public static GlobalInputManager Instance = null;

        public Vector3 MoveDirection { get; private set; }
        public bool IsDown_LShift { get; private set; }

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
                MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                IsDown_LShift = Input.GetKey(KeyCode.LeftShift);
            }
            else
                MoveDirection = Vector2.zero;
        }
    }
}