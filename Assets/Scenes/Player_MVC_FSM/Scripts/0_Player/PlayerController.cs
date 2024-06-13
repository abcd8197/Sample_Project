namespace Player_MVC_FSM
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerModel), typeof(PlayerView))]
    public class PlayerController : MonoBehaviour, IMoveable, IJumpable
    {
        #region Inspector Properties
        [SerializeField] private Rigidbody _rigidBody;
        #endregion

        #region Private Fields
        private PlayerModel m_Model;
        private PlayerView m_View;

        private IMoveable m_IMoveable;
        private IJumpable m_IJumpable;

        private float m_RunWeight = 1;
        #endregion

        private void Awake()
        {
            m_Model = this.GetComponent<PlayerModel>();
            m_View = this.GetComponent<PlayerView>();
        }

        private void FixedUpdate()
        {
            if (GlobalInputManager.Instance != null)
            {
                this.Move(GlobalInputManager.Instance.MoveDirection);
            }
        }

        public void Jump()
        {

        }

        public void Move(Vector3 direction)
        {
            if (GlobalInputManager.Instance.IsDown_LShift)
            {
                if (m_RunWeight < 2)
                    m_RunWeight += Time.fixedDeltaTime;
                else
                    m_RunWeight = 2;
            }
            else
            {
                if (m_RunWeight > 2)
                    m_RunWeight -= Time.fixedDeltaTime;
                else
                    m_RunWeight = 1;
            }

            if (m_Model != null && m_Model.Data != null)
                _rigidBody.MovePosition(this.transform.position + (direction.normalized * m_Model.Data.MoveSpeed * m_RunWeight * Time.fixedDeltaTime));

            if (m_View != null)
                m_View.SetMoveSpeed(Mathf.Clamp01(direction.magnitude) * m_RunWeight);
        }
    }
}