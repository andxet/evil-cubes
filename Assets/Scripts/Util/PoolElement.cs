using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Util
{
    public class PoolElement : MonoBehaviour
    {
        ObjectPool mProprietaryPool;

        /////////////////////////////////////////////
        public void Reset()
        {
            gameObject.SetActive(true);
        }

        /////////////////////////////////////////////
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        /////////////////////////////////////////////
        public bool Destroy()
        {
            bool success = mProprietaryPool != null;
            if(success)
                mProprietaryPool.Destroy(this);
            return success;
        }
    }
}

