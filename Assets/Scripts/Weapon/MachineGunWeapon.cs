using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;
using EvilCubes.Util;

namespace EvilCubes.Weapon
{
    public class MachineGunWeapon : Weapon
    {
        //Remember to call base.Awake if Awake function is needed
        [SerializeField]
        GameObject mShootPoint;
        [SerializeField]
        float mCoolDownTime = 0.5f;

        float mLastShootTime = -1.0f;

        /////////////////////////////////////////////
        new void Awake()
        {
            if(mShootPoint == null)
            {
                Debug.LogError("MachineGunWeapon: This weapon is not correctly initialized.");
                enabled = false;
                return;
            }
            base.Awake();
        }

        /////////////////////////////////////////////
        void Update()
        {
            if(mInputManager.GetCommandState(InputManager.Command.SHOOT))
            {
                if (Time.timeSinceLevelLoad > mLastShootTime + mCoolDownTime)
                {
                    Shoot();
                    mLastShootTime = Time.timeSinceLevelLoad;
                }
            }
        }

        /////////////////////////////////////////////
        public override void Shoot()
        {
            if (!enabled)
                return;
            Bullet bullet = null;
            {
                PoolElement bulletEl = mBulletPool.Create();
                if(bulletEl != null)
                {
                    bullet = bulletEl.gameObject.GetComponent<Bullet>();   
                }
            }
            if (bullet == null)
            {
                Debug.LogWarning("Failed to shoot the projectile. Maybe the bullet frefab is wrong?");
                return;
            }
            bullet.transform.position = mShootPoint.transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.Shoot();
        }
    }
}