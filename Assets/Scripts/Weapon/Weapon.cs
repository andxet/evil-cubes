using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;
using EvilCubes.Util;

namespace EvilCubes.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected int mPoolDimension;
        [SerializeField]
        protected GameObject mBulletPrefab;

        protected InputManager mInputManager;
        protected ObjectPool mBulletPool;

        /////////////////////////////////////////////
        protected void Start()
        {
            mInputManager = GameManager.GetInstance().GetInputManager();
        }

        /////////////////////////////////////////////
        protected void Awake()
        {
            if (mBulletPrefab == null)
            {
                Debug.LogError("Weapon: This weapon is not correctly initialized.");
                enabled = false;
                return;
            }
            mBulletPool = new ObjectPool(mPoolDimension, mBulletPrefab);
        }

        /////////////////////////////////////////////
        public abstract void Shoot();
    }
}
