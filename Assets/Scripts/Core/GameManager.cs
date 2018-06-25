using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    [RequireComponent(typeof(InputManager))]
    public class GameManager : MonoBehaviour
    {
        //This is a singleton
        static GameManager _instance;

        [Header("Input configurations")]
        [SerializeField] 
        float mTurnSensibility = 1.0f;
        [SerializeField]
        PlayerManager mPlayer;
        InputManager mInput;
        Crosshair mCrossHair;
        CameraManager mCameraManager;

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
            mPlayer.Init(mInput, mTurnSensibility);
        }

        /////////////////////////////////////////////
        void Update()
        {

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
    }
}
