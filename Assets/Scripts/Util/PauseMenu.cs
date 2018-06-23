using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Util
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseMenuGUI;
        bool paused;

        /////////////////////////////////////////////
        void Start()
        {
            if (PauseMenuGUI)
            {
                Debug.LogError("PauseMenu: PauseMenuGUI not found.");
                enabled = false;
                return;
            }
            paused = false;
        }

        /////////////////////////////////////////////
        public void Pause()
        {
            paused = true;
            Time.timeScale = 0;
            PauseMenuGUI.SetActive(true);
            //TODO: Show mouse
        }

        /////////////////////////////////////////////
        public void Unpause()
        {
            paused = false;
            Time.timeScale = 1;
            PauseMenuGUI.SetActive(false);
            //TODO: Show mouse
        }
    }
}
