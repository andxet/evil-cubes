using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    [RequireComponent(typeof(InputManager))]
    public class GameManager : MonoBehaviour
    {
        //This is a singleton
        public static GameManager _instance { get; private set; }

        [Header("Input configurations")]
        [SerializeField] float mTurnSensibility = 1.0f;

        [SerializeField]
        PlayerManager mPlayer;
        InputManager mInput;

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
        void Start()
        {
            if(mPlayer == null)
            {
                Debug.LogError("GameManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }
            mInput = GetComponent<InputManager>();
            mPlayer.Init(mInput, mTurnSensibility);
        }

        /////////////////////////////////////////////
        void Update()
        {

        }
    }
}
