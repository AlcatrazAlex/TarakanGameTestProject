using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TaranaGame.Pooling
{
    public class SimplePool<T> where T : MonoBehaviour
    {
        private ObjectPool<T> _objectPool;
        
        private T _prefab;
        private Transform _inActiveRoot;
        private Transform _activeRoot;
        
        private List<T> _activeObjects = new List<T>();

        public SimplePool(Transform activeRoot, T prefab, int maxPoolSize = 100, int initPoolSize = 0)
        {
            _prefab = prefab;
            _activeRoot = activeRoot;
            
            var poolRoot = new GameObject($" [{typeof(T)}] PoolRoot");
            _inActiveRoot = poolRoot.transform;

            _objectPool = new ObjectPool<T>(OnCreateObject, OnGetObject, OnObjectReleased, defaultCapacity: initPoolSize, maxSize: maxPoolSize);
        }

        private T OnCreateObject()
        {
            var inst = GameObject.Instantiate(_prefab, _inActiveRoot);
            inst.gameObject.SetActive(false);
            return inst;
        }

        private void OnGetObject(T obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(_activeRoot);
            if(_activeRoot != null)
                obj.transform.localPosition = Vector3.zero;
            _activeObjects.Add(obj);
        }

        private void OnObjectReleased(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_inActiveRoot);
            _activeObjects.Remove(obj);
        }

        public T Get()
        {
            return _objectPool.Get();
        }

        public void ReleaseAll()
        {
            foreach (var o in _activeObjects)
            {
                ReleaseObject(o);
            }
        }

        public void ReleaseObject(T obj)
        {
            _objectPool.Release(obj);
        }
    }
}