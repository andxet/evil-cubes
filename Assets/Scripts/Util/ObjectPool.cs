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
#if DEBUG
        int mMinimumAvailableElements;
#endif

        /////////////////////////////////////////////
        public ObjectPool(int numElements, GameObject go)
        {
            if(numElements < 1)
            {
                Debug.LogError("Failed to init the Object Pool");
                return;
            }

#if DEBUG
            mMinimumAvailableElements = numElements;
#endif

            mActiveObjects = new List<PoolElement>();
            mAvailableObjects = new Queue<PoolElement>();

            GameObject root = new GameObject("Pool_" + go.name);
            for (int i = 0; i < numElements; i++)
            {
                GameObject newOb = Object.Instantiate(go);
                newOb.transform.parent = root.transform;
                PoolElement component = newOb.GetComponent<PoolElement>();
                if(component == null)
                {
                    Debug.LogError("ObjectPool: the GameObject " + go + " doesn't have a PoolElement component.");
                }
                component.SetProprietaryPool(this);
                component.Deactivate();
                mAvailableObjects.Enqueue(component);
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
#if DEBUG
            if(mMinimumAvailableElements > mAvailableObjects.Count)
            {
                mMinimumAvailableElements = mAvailableObjects.Count;
                Debug.Log("ObjectPool: Minimum available elements reached: " + mMinimumAvailableElements);
            }
#endif
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
