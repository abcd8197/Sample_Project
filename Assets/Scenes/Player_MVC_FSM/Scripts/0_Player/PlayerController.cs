namespace Player_MVC_FSM
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerModel), typeof(PlayerView))]
    public class PlayerController : MonoBehaviour, IMoveable, IJumpable
    {
        #region Private Fields
        private PlayerModel m_Model;
        private PlayerView m_View;

        private float m_RunWeight = 1;
        private bool m_isRunning = false;
        #endregion

        private void Awake()
        {
            m_Model = this.GetComponent<PlayerModel>();
            m_View = this.GetComponent<PlayerView>();

            GlobalInputManager.OnMove += this.Move;
            GlobalInputManager.OnSpaceBarDown += this.OnSpaceBarDown;
            GlobalInputManager.OnLeftShiftDown += this.OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp += this.OnLeftShiftUp;
        }

        private void OnDestroy()
        {
            GlobalInputManager.OnMove -= this.Move;
            GlobalInputManager.OnSpaceBarDown -= this.OnSpaceBarDown;
            GlobalInputManager.OnLeftShiftDown -= this.OnLeftShiftDown;
            GlobalInputManager.OnLeftShiftUp -= this.OnLeftShiftUp;
        }

        public void Move(Vector3 direction)
        {
            if (direction.magnitude < 0.01)
            {
                m_RunWeight = 0;

                if (m_View != null)
                    m_View.Move(Vector3.zero, 0, 0);
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

                if (m_View != null)
                    m_View.Move(direction, m_Model.Data.MoveSpeed * m_RunWeight, m_RunWeight);
            }
        }

        public void Jump(float power)
        {
            if (m_View != null)
            {
                m_View.Jump(Vector3.up, power);
            }
        }

        #region Events
        private void OnLeftShiftDown() => m_isRunning = true;
        private void OnLeftShiftUp() => m_isRunning = false;

        private void OnSpaceBarDown() => this.Jump(m_Model.Data.JumpPower);
        #endregion
    }
}