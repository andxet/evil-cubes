﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EvilCubes.Weapon;

namespace EvilCubes.Core
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> mWeaponPrefabList = new List<GameObject>();
        [SerializeField]
        Transform mHandPosition;
        [SerializeField]
        GameObject mPlayerModel;

        List<Weapon.Weapon> mWeaponList = new List<Weapon.Weapon>();
        int mCurrentWeapon = 0;
        InputManager mInput;
        float mRotationSensibility;

        /////////////////////////////////////////////
        void Awake()
        {
            //Check only prefabs internal objects, the other objects will be injected by the GameManager
            if (mHandPosition == null || mPlayerModel == null)
            {
                Debug.LogError("PlayerManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }

            //Init the weapons
            foreach (GameObject weaponPrefab in mWeaponPrefabList)
            {
                GameObject ob = Instantiate(weaponPrefab);
                Weapon.Weapon weapon = ob.GetComponent<Weapon.Weapon>();
                if (weapon != null)
                {
                    mWeaponList.Add(weapon);
                    weapon.transform.parent = mHandPosition;
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.gameObject.SetActive(false);
                }
                else
                    Debug.LogWarning("PlayerManager: found a non weapon object in prefabs list.");
            }

            //Does exists weapons?
            if (mWeaponList.Count == 0)
            {
                Debug.LogError("PlayerManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }

            mWeaponList[0].gameObject.SetActive(true);
        }

        /////////////////////////////////////////////
        void Start()
        {
            if (!enabled)
                return;
            
            mInput = GameManager.GetInstance().GetInputManager();
            mRotationSensibility = GameManager.GetInstance().GetAppConfig().XTurnRotation;
        }

        /////////////////////////////////////////////
        void Update()
        {
            if(Time.timeScale > 0 && mInput != null)
            {
                //Movement
                float mouse_x = mInput.GetMouseState(InputManager.MouseState.X_DELTA);
                transform.rotation *= Quaternion.AngleAxis(mouse_x * mRotationSensibility, new Vector3(0, 1, 0));

                //Weapon management
                if (mInput.GetCommandState(InputManager.Command.NEXT_WEAPON))
                    ChangeWeapon(mCurrentWeapon == mWeaponList.Count - 1 ? 0 : mCurrentWeapon + 1);
                
                if (mInput.GetCommandState(InputManager.Command.PREVIOUS_WEAPON))
                    ChangeWeapon(mCurrentWeapon == 0 ? mWeaponList.Count - 1 : mCurrentWeapon - 1);
            }
        }

        /////////////////////////////////////////////
        void ChangeWeapon(int newWeaponIndex)
        {
            if(newWeaponIndex < 0 || newWeaponIndex > mWeaponList.Count)
            {
                Debug.LogError("PlayerManager: change weapon not possible for weapon index " + newWeaponIndex);
                return;
            }

            mWeaponList[mCurrentWeapon].gameObject.SetActive(false);
            mWeaponList[newWeaponIndex].gameObject.SetActive(true);
            mCurrentWeapon = newWeaponIndex;
        }

        /////////////////////////////////////////////
        public void HidePlayer(bool visible)
        {
            mPlayerModel.SetActive(!visible);
        }
    }
}
