using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour, System.IDisposable
{
    public System.Action<Cube> Die;

    private ParticleSystem _particle;
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _mpb;
    private Rigidbody _rb;

    private bool _isCollided = false;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
        _particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        this.transform.localPosition = Vector3.zero;
        _isCollided = false;

        _mpb.SetColor("_Color", Random.ColorHSV());
        _mpb.SetFloat("_Metalic", Random.Range(0, 1));
        _meshRenderer.SetPropertyBlock(_mpb);
        _rb.mass = Random.Range(2, 5);

        transform.rotation = Random.rotation;

        Vector3 rndForce;
        rndForce.x = Random.Range(-0.25f, 0.25f);
        rndForce.y = Random.Range(30,50);
        rndForce.z = Random.Range(-0.5f, 0.5f);
        _rb.AddForce(rndForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (_isCollided && _particle.isEmitting == false)
            Die?.Invoke(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _particle.Play(true);
            _isCollided = true;
        }
    }

    public void Dispose()
    {
        if (Die != null)
            Die = null;
    }
}
