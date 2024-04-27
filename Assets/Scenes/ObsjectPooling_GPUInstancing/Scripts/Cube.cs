using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour, System.IDisposable
{
    public System.Action<Cube> Die;

    private ParticleSystem _particle;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private MaterialPropertyBlock _mpb;
    private Rigidbody _rb;
    private Camera _camera;

    private bool _isCollided = false;
    private Vector3[] _vertices;

    private void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mpb = new MaterialPropertyBlock();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;

        _vertices = _meshFilter.mesh.vertices;
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
        rndForce.y = Random.Range(30, 50);
        rndForce.z = Random.Range(-0.5f, 0.5f);
        _rb.AddForce(rndForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (_isCollided && _particle.isEmitting == false)
            Die?.Invoke(this);

        OcculusionCullingUpdate();
    }

    /// <summary> Dynamic Occulusion Culling </summary>
    private void OcculusionCullingUpdate()
    {
        bool isInViewport = false;

        for (int i = 0; i < _vertices.Length; i++)
        {
            // Calcuate World Point -> Viewport Point
            Vector3 worldPoint = transform.TransformPoint(_vertices[i]);
            Vector3 viewpotPoint = _camera.WorldToViewportPoint(worldPoint);

            if (viewpotPoint.x >= 0 && viewpotPoint.x <= 1 &&
                viewpotPoint.y >= 0 && viewpotPoint.y <= 1 &&
                viewpotPoint.z > 0)
                isInViewport = true;
        }

        if (_meshRenderer.enabled != isInViewport)
            _meshRenderer.enabled = isInViewport;
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
