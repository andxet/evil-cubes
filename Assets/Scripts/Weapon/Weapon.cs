using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;
using EvilCubes.Util;
using System;

namespace EvilCubes.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        GameObject mBulletPrefab;
        [SerializeField]
        GameObject mShootPoint;
        [SerializeField]
        float mCoolDownTime;
        float mLastShootTime;
        [SerializeField]
        float mImprecision;
        [SerializeField]
        float mBulletShooted = 1;

        InputManager mInputManager;
        int mPoolDimension;
        ObjectPool mBulletPool;
        Crosshair mCrossHair;

        /////////////////////////////////////////////
        protected void Awake()
        {
            if (mBulletPrefab == null
                || mBulletPrefab.GetComponent<Bullet>() == null
                || mShootPoint == null
                || mCoolDownTime < 0.1f
                || mImprecision < 0 || mImprecision > 360
                || mBulletShooted < 1)
            {
                Debug.LogError("Weapon: This weapon is not correctly initialized: " + name);
                enabled = false;
                return;
            }
            mPoolDimension = CalculatePoolDimension();
            mBulletPool = new ObjectPool(mPoolDimension, mBulletPrefab);
        }

        /////////////////////////////////////////////
        protected void Start()
        {
            mInputManager = GameManager.GetInstance().GetInputManager();
            mCrossHair = GameManager.GetInstance().GetCrossHair();
        }

        /////////////////////////////////////////////
        void Update()
        {
            if (Time.timeScale > 0)
            {
                if (mCrossHair != null)
                    transform.LookAt(mCrossHair.HitPoint);
                if (mInputManager.GetCommandState(InputManager.Command.SHOOT))
                {
                    if (Time.timeSinceLevelLoad > mLastShootTime + mCoolDownTime)
                    {
                        Shoot();
                        mLastShootTime = Time.timeSinceLevelLoad;
                    }
                }
            }
        }

        /////////////////////////////////////////////
        void Shoot()
        {
            if (!enabled)
                return;

            for (int i = 0; i < mBulletShooted; i++)
            {
                Bullet bullet = CreateBullet();
                if (bullet == null)
                    continue;
                bullet.transform.position = mShootPoint.transform.position;
                bullet.transform.rotation = mShootPoint.transform.rotation * Quaternion.FromToRotation(Vector3.forward, Util.Util.RandomInsideCone(mImprecision));
                bullet.Shoot();
            }
        }

        /////////////////////////////////////////////
        Bullet CreateBullet()
        {
            Bullet bullet = null;
            {
                PoolElement bulletEl = mBulletPool.Create();
                if (bulletEl != null)
                {
                    bullet = bulletEl.gameObject.GetComponent<Bullet>();
                }
            }
            if (bullet == null)
                Debug.LogWarning("Failed to shoot the projectile. Maybe the bullet frefab is wrong?");
            return bullet;
        }

        /////////////////////////////////////////////
        int CalculatePoolDimension()
        {
            Bullet bullet = mBulletPrefab.GetComponent<Bullet>();
            return (int)Mathf.Ceil(bullet.GetValidTime() / mCoolDownTime * (mBulletShooted + 1));
        }
    }
}
