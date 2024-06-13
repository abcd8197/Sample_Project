namespace Player_MVC_FSM
{
    using UnityEngine;

    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData m_Data = null;
        private PlayerController m_Controller;

        public PlayerData Data { get => m_Data; }
        public void SetController(PlayerController controller) => m_Controller = controller;
    }
}