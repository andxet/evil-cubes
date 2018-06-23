using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class PlayerCamera : MonoBehaviour
    {
        //protected Camera mCam;
        protected InputManager mInput;
        protected bool mInit = false;
        protected float mSensibility;

        /////////////////////////////////////////////
        void Start()
        {
            //mCam = GetComponent<Camera>();
        }

        /////////////////////////////////////////////
        public void Init(InputManager mgr, float sensivity)
        {
            mInput = mgr;
            mSensibility = sensivity;
            mInit = true;
        }
    }
}