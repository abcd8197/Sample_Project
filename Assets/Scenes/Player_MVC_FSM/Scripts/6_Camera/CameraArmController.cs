using UnityEngine;
using Player_MVC_FSM;

public class CameraArmController : MonoBehaviour
{
    [SerializeField] private float Sensitivity_X = 270;
    [SerializeField] private float Sensitivity_Y = 180;
    [SerializeField] private bool ReverseX = false;
    [SerializeField] private bool ReverseY = false;

    private Vector3 m_rotateAmount;

    private void Awake()
    {
        m_rotateAmount = Vector3.zero;
        this.transform.localEulerAngles = Vector2.right * 30;
        GlobalInputManager.OnMouseMove += RotateCameraArm;
    }

    private void OnDestroy()
    {
        GlobalInputManager.OnMouseMove -= RotateCameraArm;
    }

    private void RotateCameraArm(Vector3 delta)
    {
        m_rotateAmount.x += (ReverseY ? -delta.y : delta.y) * Sensitivity_Y * Time.deltaTime;
        m_rotateAmount.x = Mathf.Clamp(m_rotateAmount.x, 5, 80);
        m_rotateAmount.y += (ReverseX ? -delta.x : delta.x) * Sensitivity_X * Time.deltaTime;
        m_rotateAmount.z = 0f;

        this.transform.localEulerAngles = m_rotateAmount;
    }
}
