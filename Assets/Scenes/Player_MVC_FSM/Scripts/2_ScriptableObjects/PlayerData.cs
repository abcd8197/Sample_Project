namespace TPSPlayerController_Scene
{
    using UnityEngine;
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private System.Collections.Generic.List<PlayerAnimationData> m_AnimationDatas;
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_JumpPower;

        public System.Collections.Generic.List<PlayerAnimationData> AnimationDatas { get => m_AnimationDatas; }
        public float MoveSpeed { get => m_MoveSpeed; }
        public float JumpPower { get => m_JumpPower; }
    }
}