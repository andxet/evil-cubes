using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class GameManager : MonoBehaviour
    {
        //This is a singleton
        public static GameManager _instance { get; private set; }

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
        }

        /////////////////////////////////////////////
        void Update()
        {

        }
    }
}
