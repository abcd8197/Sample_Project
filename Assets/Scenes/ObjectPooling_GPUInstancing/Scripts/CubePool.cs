using UnityEngine;

public class CubePool : MonoBehaviour
{
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private float FireInterval = 0.5f;

    private Codejay.ObjectPool<Cube> pool;
    private float _fireCounter = 0f;
    private Camera _mainCamera;

    private void Awake()
    {
        pool = new Codejay.ObjectPool<Cube>();
        pool.Initialize(CubePrefab, 512, transform);
    }

    private void OnDisable()
    {
        if (pool != null)
            pool.ReturnToPoolAll();
    }

    private void OnDestroy()
    {
        if (pool != null)
            pool.DestroyPool();
    }

    private void FixedUpdate()
    {
        if (_fireCounter < FireInterval)
            _fireCounter += Time.fixedDeltaTime;
        else
        {
            var cube = pool.GetPooledObject();
            cube.Die += this.ReturnToPool;
        }
    }

    private void ReturnToPool(Cube cube)
    {
        cube.Die -= this.ReturnToPool;
        pool.ReturnToPool(cube);
    }
}
