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
            x_angle += transform.rotation.eulerAngles.x;
            //Automatically clamp angle...
            transform.localRotation = Quaternion.Euler(x_angle, 0,0);
        }
    }
}
