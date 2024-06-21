namespace TPSPlayerController_Scene
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerModel), typeof(PlayerView))]
    public class PlayerController : MonoBehaviour, IMoveable, IJumpable
    {
        #region Private Fields

        private float m_RunWeight = 1;
        private bool m_isRunning = false;
        private System.Collections.Generic.Dictionary<System.Type, PlayerStateBase> _dicStateCaches;
        private PlayerStateBase m_currentState;
        #endregion

        #region Properties
        public PlayerModel Model { get; private set; }
        public PlayerView View { get; private set; }
        public bool IsGround { get => View.IsGround; }
        #endregion

        private void Awake()
        {
            Model = this.GetComponent<PlayerModel>();
            View = this.GetComponent<PlayerView>();

            GlobalInputManager.OnMoveStart += this.OnMoveStart;
            GlobalInputManager.OnSpaceBarDown += this.OnSpaceBarDown;
            GlobalInputManager.OnLeftShiftDown += this.OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp += this.OnLeftShiftUp;
            GlobalInputManager.OnSpaceBarDown += this.OnSpaceBarDown;

            _dicStateCaches = new System.Collections.Generic.Dictionary<System.Type, PlayerStateBase>();
            ChangeState<PlayerState_Idle>();
        }

        private void OnDestroy()
        {
            GlobalInputManager.OnSpaceBarDown -= this.OnSpaceBarDown;
            GlobalInputManager.OnLeftShiftDown -= this.OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp -= this.OnLeftShiftUp;
        }

        private void Update()
        {
            m_currentState?.Execute();
        }

        public void ChangeState<T>() where T : PlayerStateBase, new()
        {
            System.Type stateType = typeof(T);

            if (!_dicStateCaches.TryGetValue(stateType, out PlayerStateBase state))
            {
                state = new T();
                if (state is IStateSetter setter)
                {
                    setter.SetController(this);
                }

                _dicStateCaches[stateType] = state;
            }

            m_currentState?.Exit();
            m_currentState = state;
            m_currentState?.Enter();
        }

        public void OnMoveStart() => ChangeState<PlayerState_Move>();
        public void OnMoveEnd() => ChangeState<PlayerState_Idle>();

        public void Move(Vector3 direction)
        {
            if (direction.magnitude < 0.01)
            {
                m_RunWeight = 0;

                if (View != null)
                    View.Move(Vector3.zero, 0, 0);
            }
            else
            {
                if (m_isRunning)
                {
                    m_RunWeight = Mathf.Clamp(m_RunWeight + Time.fixedDeltaTime, 1, 2);
                }
                else if (m_RunWeight > 1 + Time.fixedDeltaTime + float.Epsilon)
                {
                    m_RunWeight -= Time.fixedDeltaTime;
                }
                else if (m_RunWeight < 1 - Time.fixedDeltaTime - float.Epsilon)
                {
                    m_RunWeight += Time.fixedDeltaTime;
                }
                else
                {
                    m_RunWeight = 1;
                }

                if (View != null)
                    View.Move(direction, Model.Data.MoveSpeed * m_RunWeight, m_RunWeight);
            }
        }

        public void Jump(float power)
        {
            if (!IsGround)
            {
                ChangeState<PlayerState_Jump>();
            }
        }

        #region Events
        private void OnLeftShiftDown() => m_isRunning = true;
        private void OnLeftShiftUp() => m_isRunning = false;

        private void OnSpaceBarDown() => this.Jump(Model.Data.JumpPower);
        #endregion
    }
}