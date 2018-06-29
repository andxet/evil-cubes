using System.Collections;
using System.Collections.Generic;
using EvilCubes.Core;
using UnityEngine;

namespace EvilCubes.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        GameObject mGameUI;
        [SerializeField]
        GameObject mPauseUI;

        InputManager mInput;

        /////////////////////////////////////////////
        void Start()
        {
            mInput = GameManager.GetInstance().GetInputManager();
            if(mInput == null || mGameUI == null || mPauseUI == null)
            {
                Debug.LogError("UIManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
            mGameUI.gameObject.SetActive(true);
            mPauseUI.gameObject.SetActive(false);
        }

        /////////////////////////////////////////////
        void Update()
        {

        }

        /////////////////////////////////////////////
        public void ShowPause()
        {
            mGameUI.gameObject.SetActive(false);
            mPauseUI.gameObject.SetActive(true);
        }

        /////////////////////////////////////////////
        public void ExitPause()
        {
            mGameUI.gameObject.SetActive(true);
            mPauseUI.gameObject.SetActive(false);
        }
    }
}