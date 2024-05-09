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
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(Codejay.Enum.EEventKey.UI_OnClickPlayerJumpButton, this.Jump);
        }
    }

    private void OnDestroy()
    {
        if (!EventManager.IsApplicationQuitting && EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(Codejay.Enum.EEventKey.UI_OnClickPlayerJumpButton, this.Jump);
        }

    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }
}
