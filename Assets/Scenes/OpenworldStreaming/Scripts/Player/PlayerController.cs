using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float JumpPower = 5;
    [SerializeField] private float Sensivity = 360;
    [SerializeField] private Transform CameraArmTransform;

    private float RotX_Min = -50;
    private float RotX_Max = 70;
    private Rigidbody _rb;
    private Vector2 _prevAxis;
    private Vector2 _axis;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _axis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (_axis != _prevAxis)
        {
            CameraArmTransform.Rotate(new Vector3(-_axis.y, _axis.x, 0) * Time.deltaTime * Sensivity);
            Vector3 rot = CameraArmTransform.localEulerAngles;

            if (rot.x > 180)
                rot.x -= 360;
            else if (rot.x < -180)
                rot.x += 360;
            rot.x = Mathf.Clamp(rot.x, RotX_Min, RotX_Max);
            rot.z = 0f;

            CameraArmTransform.localEulerAngles = rot;
            _prevAxis = _axis;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            _rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (Input.anyKey)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                float vertical = Input.GetAxis("Vertical");
                float horizontal = Input.GetAxis("Horizontal");

                Vector3 dir = CameraArmTransform.forward + (CameraArmTransform.forward * vertical) + (CameraArmTransform.right * horizontal);
                _rb.MovePosition(this.transform.position + dir.normalized * MoveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
