using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class PlayerCamera : MonoBehaviour
    {
        protected InputManager mInput;
        protected bool mInit = false;
        protected float mSensibility;

        /////////////////////////////////////////////
        public void Init(InputManager mgr, float sensibility)
        {
            mInput = mgr;
            mSensibility = sensibility;
            mInit = true;
        }
    }
}