using UnityEngine;
using TPSPlayerController_Scene;

public class CameraArmController : MonoBehaviour
{
    [SerializeField] private float Sensitivity_X = 270;
    [SerializeField] private float Sensitivity_Y = 230;
    [SerializeField] private bool ReverseX = false;
    [SerializeField] private bool ReverseY = true;
    [SerializeField] private float Smoothing = 0.1f;

    private Vector3 m_rotateAmount;
    private Vector3 m_currentRotation;
    private Vector3 m_rotationVelocity;

    private void Awake()
    {
        this.transform.localEulerAngles = Vector2.right * 30;

        m_rotateAmount = Vector3.zero;
        m_currentRotation = this.transform.localEulerAngles;

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

        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, m_rotationVelocity, ref m_rotationVelocity, Smoothing);
        this.transform.localEulerAngles = m_rotateAmount;
    }
}
