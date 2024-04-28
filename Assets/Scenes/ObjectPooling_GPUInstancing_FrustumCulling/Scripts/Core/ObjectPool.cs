namespace Codejay
{
    using UnityEngine;
    /// <summary>Generic Pool For MonoBehaviour</summary>
    public class ObjectPool<T> where T : MonoBehaviour, System.IDisposable
    {
        private class PoolData
        {
            public T Data;
            public float ActivationMoment;
        }

        /// <summary> Create Target</summary>
        private GameObject Prefab;
        /// <summary> Set Parent Target</summary>
        private Transform ParentTransform;
        /// <summary> Pool Container</summary>
        private System.Collections.Generic.List<PoolData> _lstPool;

        /// <summary> Pool System Capacity</summary>
        public int Capacity { get; set; } = 32;

        /// <summary>
        /// Initialize For use Pooling System.
        /// </summary>
        /// <param name="prefab">Create Target</param>
        /// <param name="capacity">Pool System Capacity</param>
        /// <param name="parent">Set Parent Target</param>
        public void Initialize(GameObject prefab, int capacity = 32, Transform parent = null)
        {
            // Exception Handling
            if (prefab == null)
                throw new System.ArgumentException($"[Developer Error] {prefab} is Never Be null");

            Prefab = prefab;
            Capacity = capacity;
            ParentTransform = parent;

            if (_lstPool == null)
                _lstPool = new System.Collections.Generic.List<PoolData>(Capacity);
            else
            {
                for (int i = 0; i < _lstPool.Count; i++)
                {
                    if (_lstPool[i].Data != null && _lstPool[i].Data.gameObject != null)
                    {
                        _lstPool[i].Data.Dispose();
                        MonoBehaviour.Destroy(_lstPool[i].Data.gameObject);
                    }
                }

                _lstPool.Clear();
            }

            for (int i = 0; i < Capacity; i++)
            {
                PoolData data = new PoolData();
                data.Data = MonoBehaviour.Instantiate(Prefab, ParentTransform != null ? ParentTransform : null).GetComponent<T>();
                data.ActivationMoment = 0;

                _lstPool.Add(data);
                _lstPool[i].Data.gameObject.SetActive(false);
            }

        }

        public T GetPooledObject()
        {
            for (int i = 0; i < _lstPool.Count; i++)
            {
                if (_lstPool[i].Data.gameObject.activeInHierarchy == false)
                {
                    _lstPool[i].ActivationMoment = Time.realtimeSinceStartup;
                    _lstPool[i].Data.gameObject.SetActive(true);
                    return _lstPool[i].Data;
                }
            }

            if (_lstPool.Capacity < Capacity)
            {
                int temp = _lstPool.Capacity * 2;

                if (temp >= Capacity)
                    temp = Capacity;

                _lstPool.Capacity = temp;

                PoolData data = new PoolData();
                data.Data = MonoBehaviour.Instantiate(Prefab, ParentTransform != null ? ParentTransform : null).GetComponent<T>();
                data.ActivationMoment = Time.realtimeSinceStartup;

                _lstPool.Add(data);
                _lstPool[_lstPool.Count - 1].Data.gameObject.SetActive(true);

                return data.Data;
            }
            else
            {
                PoolData poolData = FindOldestActivatedObject();

                if (poolData == null)
                {
                    // Exception Handling
                    throw new System.IndexOutOfRangeException("[Developer Error] All of Pooled Object Already Activated!\n please Increase Capacity.\n");
                }
                else
                {
                    poolData.ActivationMoment = Time.realtimeSinceStartup;
                    poolData.Data.gameObject.SetActive(true);
                    return poolData.Data;
                }
            }
        }

        public void ReturnToPool(T t)
        {
            t.gameObject.SetActive(false);
        }

        public void ReturnToPoolAll()
        {
            for (int i = 0; i < _lstPool.Count; i++)
            {
                if (_lstPool[i].Data.gameObject.activeInHierarchy)
                {
                    _lstPool[i].Data.gameObject.SetActive(false);
                }
            }
        }

        public void DestroyPool()
        {
            for (int i = 0; i < _lstPool.Count; i++)
            {
                if (_lstPool[i].Data != null && _lstPool[i].Data.gameObject != null)
                {
                    _lstPool[i].Data.Dispose();
                    MonoBehaviour.Destroy(_lstPool[i].Data.gameObject);
                }
            }

            Prefab = null;
            ParentTransform = null;
            _lstPool.Clear();
        }

        private PoolData FindOldestActivatedObject()
        {
            float temp = float.MaxValue;
            int index = -1;
            for (int i = 0; i < _lstPool.Count; i++)
            {
                if (_lstPool[i].Data.gameObject.activeInHierarchy)
                {
                    if (_lstPool[i].ActivationMoment < temp)
                    {
                        index = i;
                        temp = _lstPool[i].ActivationMoment;
                    }
                }
            }

            return index >= 0 ? _lstPool[index] : null;
        }
    }
}