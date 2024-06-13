namespace Player_MVC_FSM
{
    using UnityEngine;

    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;

        private PlayerController m_Controller;
        public void SetController(PlayerController controller) => m_Controller = controller;

        public void SetMoveSpeed(float paramValue)
        {
            m_Animator.SetFloat("MoveSpeed", paramValue);
        }

    }
}