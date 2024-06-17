namespace TPSPlayerController_Scene
{
    using UnityEngine;
    [System.Serializable]
    public class PlayerAnimationData
    {
        [SerializeField] private EPlayerAnimationState m_state;
        [SerializeField] private float m_duration;
        [SerializeField] private float[] m_triggerMoments;

        public EPlayerAnimationState State { get => m_state; }
        public float Duration { get => m_duration; }
        public float[] TriggerMoments { get => m_triggerMoments; }
    }
}