namespace Player_MVC_FSM
{
    using UnityEngine;

    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData m_Data = null;
        public PlayerData Data { get => m_Data; }
    }
}