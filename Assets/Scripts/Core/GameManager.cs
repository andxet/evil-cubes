using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class GameManager : MonoBehaviour
    {
        //This is a singleton
        static GameManager _instance;

        [SerializeField]
        PlayerManager mPlayer;

        InputManager mInput;
        Crosshair mCrossHair;
        CameraManager mCameraManager;
        AppConfig mConfig;

        /////////////////////////////////////////////
        void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("GameManager: another GameManager exists!! This should be a singleton.");
                enabled = false;
                return;
            }
            _instance = this;
            mInput = GetComponent<InputManager>();
            mCrossHair = GetComponent<Crosshair>();
            mCameraManager = GetComponent<CameraManager>();
            mConfig = GetComponent<AppConfig>();
            if(mInput == null || mCrossHair == null || mCameraManager == null || mConfig == null)
            {
                Debug.LogError("GameManager: Failed initialization.");
                enabled = false;
                return;
            }
        }

        /////////////////////////////////////////////
        void Start()
        {
            if(mPlayer == null)
            {
                Debug.LogError("GameManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
        }

        /////////////////////////////////////////////
        public static GameManager GetInstance()
        {
            if (_instance == null)
                Debug.LogWarning("GameManager: trying to acces to the instance, but the instance doesn't exists!");
            return _instance;
        }

        /////////////////////////////////////////////
        public InputManager GetInputManager()
        {
            return mInput;
        }

        /////////////////////////////////////////////
        public Crosshair GetCrossHair()
        {
            return mCrossHair;
        }

        /////////////////////////////////////////////
        public CameraManager GetCameraManager()
        {
            return mCameraManager;
        }

        /////////////////////////////////////////////
        public AppConfig GetAppConfig()
        {
            return mConfig;
        }
    }
}
