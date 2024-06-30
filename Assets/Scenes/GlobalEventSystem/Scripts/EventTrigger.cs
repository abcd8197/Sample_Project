namespace GlobalEventSystem
{
    using UnityEngine;

    public class EventTrigger : MonoBehaviour
    {
        public EEventKey EventKey;

        public void Trigger()
        {
            EventManager.Instance.Publish(EventKey);
        }
    }
}