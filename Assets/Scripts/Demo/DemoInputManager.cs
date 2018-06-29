using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Demo
{
    public class DemoInputManager : InputManager
    {
        /////////////////////////////////////////////
        void Update()
        {
            //Calculate the current states
            currentMouseState[MouseState.X_DELTA] = 0.5f;
            currentMouseState[MouseState.Y_DELTA] = 0.0f;

            currentCommandState[Command.NEXT_WEAPON] = false;
            currentCommandState[Command.PREVIOUS_WEAPON] = false;
            currentCommandState[Command.SHOOT_DOWN] = true;
            currentCommandState[Command.SHOOT] = true;
            currentCommandState[Command.PAUSE] = false;
            currentCommandState[Command.CHANGE_CAMERA] = false;
        }

        /////////////////////////////////////////////
        new public float GetMouseState(MouseState state)
        {
            return currentMouseState[state];
        }

        /////////////////////////////////////////////
        new public bool GetCommandState(Command state)
        {
            return currentCommandState[state];
        }
    }
}