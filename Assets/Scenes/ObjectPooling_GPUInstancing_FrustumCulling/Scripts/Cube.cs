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
    private Plane[] _planes;
    private Vector3 _cameraPrevPos;
    private Vector3 _cameraPrevRot;

    private void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mpb = new MaterialPropertyBlock();
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;

        _vertices = _meshFilter.sharedMesh.vertices;
        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        _cameraPrevPos = _camera.transform.position;
        _cameraPrevRot = Camera.main.transform.localEulerAngles;
    }

    private void OnEnable()
    {
        this.transform.localPosition = Vector3.zero;
        _isCollided = false;

        _meshRenderer.GetPropertyBlock(_mpb);
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

        UnityEngine.Profiling.Profiler.BeginSample("Frustum Culling 1");
        FrustumCulling_1();
        UnityEngine.Profiling.Profiler.EndSample();
        //UnityEngine.Profiling.Profiler.BeginSample("Frustum Culling 2");
        //FrustumCulling_2();
        //UnityEngine.Profiling.Profiler.EndSample();
    }

    #region Frustum Culling 1
    /// <summary> Dynamic Occulusion Culling </summary>
    private void FrustumCulling_1()
    {
        this.PlaneUpdate();
        bool isVisible = GeometryUtility.TestPlanesAABB(_planes, _meshRenderer.bounds);

        if (isVisible && _meshRenderer.enabled == false)
            _meshRenderer.enabled = true;
        else if (!isVisible && _meshRenderer.enabled)
            _meshRenderer.enabled = false;
    }

    /// <summary> For Optimize GC Allocation</summary>
    private void PlaneUpdate()
    {
        if(_cameraPrevPos != _camera.transform.position ||
            _cameraPrevRot != _camera.transform.localEulerAngles)
        {
            _cameraPrevPos = _camera.transform.position;
            _cameraPrevRot = _camera.transform.localEulerAngles;
            _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        }
    }
    #endregion

    #region Frustum Culling 2
    private void FrustumCulling_2()
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
            {
                isInViewport = true;
                break;
            }
        }

        if (_meshRenderer.enabled != isInViewport)
            _meshRenderer.enabled = isInViewport;
    }
    #endregion

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
