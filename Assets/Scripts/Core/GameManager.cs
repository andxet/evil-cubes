﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.UI;
using UnityEngine.SceneManagement;

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
        UIManager mUI;
        bool mGameEnded = false;

        /////////////////////////////////////////////
        public void ExitApplication()
        {
            Application.Quit();
        }

        /////////////////////////////////////////////
        public void TitleScreen()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
            SceneManager.LoadScene("TitleScene");
        }

        /////////////////////////////////////////////
        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }

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
            mUI = GetComponent<UIManager>();
            if (mInput == null || mCrossHair == null || mCameraManager == null || mConfig == null || mUI == null)
            {
                Debug.LogWarning("GameManager: Some component not found.");
                //enabled = false;
                //return;
            }
        }

        /////////////////////////////////////////////
        void Start()
        {
            if (mPlayer == null)
            {
                Debug.LogError("GameManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
            LifeComponent life = mPlayer.GetComponent<LifeComponent>();
            if (life == null)
            {
                Debug.LogError("GameManager: The player does not have a life component.");
                enabled = false;
                return;
            }
            life.RegisterDieAction(Lose);
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

        /////////////////////////////////////////////
        public UIManager GetUIManager()
        {
            return mUI;
        }

        /////////////////////////////////////////////
        public void Win()
        {
            mGameEnded = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            mUI.Win();
        }

        /////////////////////////////////////////////
        public void Lose()
        {
            mGameEnded = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            mUI.Lose();
        }

        /////////////////////////////////////////////
        public bool GameEnded()
        {
            return mGameEnded;
        }

        /////////////////////////////////////////////
        public PlayerManager GetPlayer()
        {
            return mPlayer;
        }
    }
}
