using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public Codejay.Enum.EEventKey EventKey;

    public void Trigger()
    {
        EventManager.Instance.Publish(EventKey);
    }
}
