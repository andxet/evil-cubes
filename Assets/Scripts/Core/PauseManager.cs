using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.UI;

namespace EvilCubes.Core
{
    public class PauseManager : MonoBehaviour
    {
        UIManager mUI;
        InputManager mInput;
        bool mCurrentPaused;

        /////////////////////////////////////////////
        void Start()
        {
            mUI = GameManager.GetInstance().GetUIManager();
            mInput = GameManager.GetInstance().GetInputManager();

            if (mUI == null || mInput == null)
            {
                Debug.LogError("PauseManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
            mCurrentPaused = false;
            Unpause();
        }

        /////////////////////////////////////////////
        void Update()
        {
            if(mInput.GetCommandState(InputManager.Command.PAUSE))
            {
                if (!mCurrentPaused)
                    Pause();
                else
                    Unpause();
                
            }
                
        }

        /////////////////////////////////////////////
        public void Pause()
        {
            if (GameManager.GetInstance().GameEnded())
                return;
            mUI.ShowPause();
            mCurrentPaused = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;

        }

        /////////////////////////////////////////////
        public void Unpause()
        {
            mUI.ExitPause();
            mCurrentPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;
        }
    }
}
