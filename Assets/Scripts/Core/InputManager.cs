using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class InputManager : MonoBehaviour
    {
        public enum MouseState
        {
            X_DELTA,
            Y_DELTA
        }

        public enum Command
        {
            PAUSE,
            SHOOT_DOWN,
            SHOOT,
            NEXT_WEAPON,
            PREVIOUS_WEAPON,
            CHANGE_CAMERA
        }

        Dictionary<Command, bool> currentCommandState = new Dictionary<Command, bool>();
        Dictionary<MouseState, float> currentMouseState = new Dictionary<MouseState, float>();

        /////////////////////////////////////////////
        private void Awake()
        {
            currentMouseState.Add(MouseState.X_DELTA, .0f);
            currentMouseState.Add(MouseState.Y_DELTA, .0f);
            currentCommandState.Add(Command.PAUSE, false);
            currentCommandState.Add(Command.SHOOT_DOWN, false);
            currentCommandState.Add(Command.SHOOT, false);
            currentCommandState.Add(Command.NEXT_WEAPON, false);
            currentCommandState.Add(Command.PREVIOUS_WEAPON, false);
            currentCommandState.Add(Command.CHANGE_CAMERA, false);
        }

        /////////////////////////////////////////////
        void Start()
        {
            
        }

        /////////////////////////////////////////////
        void Update()
        {
            //Calculate the current states
            currentMouseState[MouseState.X_DELTA] = Input.GetAxis("Mouse X");
            currentMouseState[MouseState.Y_DELTA] = Input.GetAxis("Mouse Y");

            currentCommandState[Command.NEXT_WEAPON] = Input.GetAxis("Mouse ScrollWheel") > 0;
            currentCommandState[Command.PREVIOUS_WEAPON] = Input.GetAxis("Mouse ScrollWheel") < 0;
            currentCommandState[Command.SHOOT_DOWN] = Input.GetButtonDown("Fire1");
            currentCommandState[Command.SHOOT] = Input.GetButton("Fire1");
            currentCommandState[Command.PAUSE] = Input.GetButtonDown("Pause");
            currentCommandState[Command.CHANGE_CAMERA] = Input.GetButtonDown("Pause");

#if DEBUG
            if (currentCommandState[Command.NEXT_WEAPON])
                Debug.Log("Next Weapon");
            if (currentCommandState[Command.PREVIOUS_WEAPON])
                Debug.Log("Previous Weapon");
            if (currentCommandState[Command.PAUSE])
                Debug.Log("Pause");
            if (currentCommandState[Command.SHOOT_DOWN])
                Debug.Log("Shoot Down");
            //if (currentCommandState[Command.SHOOT])
            //    Debug.Log("Shoot");
#endif
        }

        /////////////////////////////////////////////
        public float GetMouseState(MouseState state)
        {
            return currentMouseState[state];
        }

        /////////////////////////////////////////////
        public bool GetCommandState(Command state)
        {
            return currentCommandState[state];
        }
    }
}
