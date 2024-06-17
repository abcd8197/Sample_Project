namespace TPSPlayerController_Scene
{
    using UnityEngine;

    public interface IStateSetter
    {
        public void SetController(PlayerController controller);
    }
    public abstract class PlayerStateBase : IStateSetter
    {
        protected PlayerController m_controller;
        public PlayerStateBase() { }
        public void SetController(PlayerController controller)
        {
            m_controller = controller;
        }

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }

    public class PlayerState_Idle : PlayerStateBase
    {
        public override void Enter()
        {
            m_controller.m_View.StopMove();
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    public class PlayerState_Move : PlayerStateBase
    {
        private float m_RunWeight;
        private bool m_isRunning;

        public PlayerState_Move()
        {
            GlobalInputManager.OnLeftShiftDown += OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp += OnLeftShiftUp;
        }

        ~PlayerState_Move()
        {
            GlobalInputManager.OnLeftShiftDown -= OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp -= OnLeftShiftUp;
        }

        private void OnLeftShiftDown() => m_isRunning = true;
        private void OnLeftShiftUp() => m_isRunning = false;

        public override void Enter()
        {

        }

        public override void Execute()
        {
            Vector3 direction = GlobalInputManager.Instance.m_prevMoveDirection;

            if (direction.magnitude < 0.01)
            {
                m_RunWeight = 0;

                m_controller.m_View.Move(Vector3.zero, 0, 0);
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

                m_controller.m_View.Move(direction, m_controller.m_Model.Data.MoveSpeed * m_RunWeight, m_RunWeight);
            }
        }

        public override void Exit()
        {

        }
    }
}