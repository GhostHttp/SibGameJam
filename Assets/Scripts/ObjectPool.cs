using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class Pool
    {
        private GameObject prefab;
        private int poolSize;
        private Transform parent;
        private List<GameObject> _objectPool = new List<GameObject>();

        public void CreatePool(GameObject prefab, int poolSize, Transform parent)
        {
            _objectPool.Clear();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.transform.parent = parent;
                obj.SetActive(false);
                _objectPool.Add(obj);
            }
        }
        public void DestroyPool()
        {
            foreach (GameObject obj in _objectPool)
            {
                obj.SetActive(true);
                GameObject.Destroy(obj);
            }
            _objectPool.Clear();
        }

        public GameObject GetObject()
        {
            foreach (GameObject obj in _objectPool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
            return null;
        }
    }
}
