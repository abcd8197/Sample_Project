using UnityEngine;

public class Sphere : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (GlobalEventSystem.EventManager.Instance != null)
        {
            GlobalEventSystem.EventManager.Instance.Subscribe(GlobalEventSystem.EEventKey.UI_OnClickPlayerJumpButton, this.Jump);
        }
    }

    private void OnDestroy()
    {
        if (!GlobalEventSystem.EventManager.IsApplicationQuitting && GlobalEventSystem.EventManager.Instance != null)
        {
            GlobalEventSystem.EventManager.Instance.Unsubscribe(GlobalEventSystem.EEventKey.UI_OnClickPlayerJumpButton, this.Jump);
        }

    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }
}
