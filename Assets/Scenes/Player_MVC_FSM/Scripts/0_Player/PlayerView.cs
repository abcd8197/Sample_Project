namespace Player_MVC_FSM
{
    using UnityEngine;

    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Transform _cameraArmTransform;

        private Vector3 m_LastDirection = Vector3.zero;
        private Coroutine m_StopCoroutine = null;
        private float m_StopWeight = 2f;

        private void OnDestroy()
        {
            StopAllCoroutines();
            m_StopCoroutine = null;
        }

        public void Move(Vector3 direction, float speed, float weight)
        {
            if (m_StopCoroutine != null)
                StopCoroutine(m_StopCoroutine);

            if (weight <= float.Epsilon)
            {
                m_StopCoroutine = StartCoroutine(coroStopMove());
            }
            else
            {
                _animator.SetFloat("MoveSpeed", weight);

                Vector3 cameraForward = new Vector3(_cameraArmTransform.forward.x, 0, _cameraArmTransform.forward.z);
                Vector3 cameraRight = new Vector3(_cameraArmTransform.right.x, 0, _cameraArmTransform.right.z);
                Vector3 resultDir = direction.x * cameraRight + direction.z * cameraForward;
                resultDir = resultDir.normalized;

                m_LastDirection = resultDir;

                if (_rigidBody.velocity.magnitude < speed)
                    _rigidBody.AddForce(resultDir * speed * weight, ForceMode.Acceleration);
                else
                    _rigidBody.velocity = resultDir * speed * weight;
            }
        }

        public void Jump(Vector3 direction, float jumpForce)
        {
            _rigidBody.AddForce(direction * jumpForce, ForceMode.Impulse);
        }

        private System.Collections.IEnumerator coroStopMove()
        {
            float moveSpeedParam = _animator.GetFloat("MoveSpeed");

            while (moveSpeedParam > 0)
            {
                moveSpeedParam -= m_StopWeight * Time.deltaTime;
                _animator.SetFloat("MoveSpeed", moveSpeedParam);

                if (_rigidBody.velocity.magnitude < moveSpeedParam)
                    _rigidBody.AddForce(m_LastDirection * moveSpeedParam, ForceMode.Acceleration);
                else
                    _rigidBody.velocity = m_LastDirection * moveSpeedParam;

                yield return null;
            }

            _animator.SetFloat("MoveSpeed", 0);

            m_StopCoroutine = null;
        }
    }
}