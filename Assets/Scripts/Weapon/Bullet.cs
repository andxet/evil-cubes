using System.Collections;
using System.Collections.Generic;
using EvilCubes.Util;
using UnityEngine;

namespace EvilCubes.Weapon
{
    public class Bullet : PoolElement
    {
        [SerializeField]
        int mDamage;
        [SerializeField]
        float mVelocity;
        [SerializeField]
        float mValidtime;
        float mShootedTime = -1f;

        //////////////////////////////////////////////
        void Start()
        {

        }

        /////////////////////////////////////////////
        private void Awake()
        {
            mShootedTime = Time.timeSinceLevelLoad;
        }

        /////////////////////////////////////////////
        void FixedUpdate()
        {
            if (mShootedTime > 0)
            {
                transform.position += transform.forward * mVelocity * Time.deltaTime;
                if (Time.timeSinceLevelLoad > mShootedTime + mValidtime)
                    Destroy();
            }
                    
        }

        /////////////////////////////////////////////
        public void Shoot()
        {
            mShootedTime = Time.timeSinceLevelLoad;
        }

        /////////////////////////////////////////////
        new void Reset()
        {
            base.Reset();
            mShootedTime = -1;
        }

        /////////////////////////////////////////////
        void OnCollisionEnter(Collision col)
        {
            Enemy.Enemy enemy = col.gameObject.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                enemy.Hit(mDamage);
                Destroy();
            }

        }

        public float GetValidTime()
        {
            return mValidtime;
        }
    }
}
