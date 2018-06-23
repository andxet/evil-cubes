using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Util
{
    /// <summary>
    /// Provide an object pool. Use this class to avoid the creation of 
    /// GameObjects during realtime. All the GO are created when a pool is created.
    /// </summary>
    public class ObjectPool
    {
        List<PoolElement> mActiveObjects;
        Queue<PoolElement> mAvailableObjects;

        /////////////////////////////////////////////
        public ObjectPool(int elements, GameObject go)
        {
            mActiveObjects = new List<PoolElement>();
            mAvailableObjects = new Queue<PoolElement>();
            for (int i = 0; i < elements; i++)
            {
                GameObject newOb = Object.Instantiate(go);
                PoolElement component = newOb.GetComponent<PoolElement>();
                if(component == null)
                {
                    Debug.LogError("ObjectPool: the GameObject " + go + " doesn't have a T component.");
                }
                mActiveObjects.Add(component);
            }
        }

        /////////////////////////////////////////////
        public PoolElement Create()
        {
            if(mAvailableObjects.Count == 0)
            {
                Debug.LogWarning("ObjectPool: no available objects.");
                return null;
            }
            PoolElement element = mAvailableObjects.Dequeue();
            mActiveObjects.Add(element);
            element.Reset();
            Debug.Log("ObjectPool: resuming object " + element.gameObject);
            return element;
        }

        //Set an object to disabled
        /////////////////////////////////////////////
        public void Destroy(PoolElement element)
        {
            if(!mActiveObjects.Contains(element))
            {
                Debug.LogWarning("ObjectPool: object is not active " + element + ".");
                return;
            }
            Debug.Log("ObjectPool: deactivating object " + element.gameObject);
            element.Deactivate();
            mActiveObjects.Remove(element);
            mAvailableObjects.Enqueue(element);
        }
    }
}
