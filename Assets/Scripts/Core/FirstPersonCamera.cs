using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    [RequireComponent(typeof(Camera))]
    public class FirstPersonCamera : PlayerCamera
    {
        /////////////////////////////////////////////
        void Update()
        {
            if (!mInit)
                return;
            float x_angle = -mInput.GetMouseState(InputManager.MouseState.Y_DELTA) * mSensibility;
            Debug.Log(x_angle);
            x_angle += transform.rotation.eulerAngles.x;
            Debug.Log(x_angle);
            //Automatically clamp angle...
            transform.localRotation = Quaternion.Euler(x_angle, 0,0);
        }
    }
}
