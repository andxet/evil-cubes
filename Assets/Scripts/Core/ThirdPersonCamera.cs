using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Core
{
    public class ThirdPersonCamera : PlayerCamera
    {
        /////////////////////////////////////////////
        void Update()
        {
            if (!mInit)
                return;
            float x_delta_angle = mInput.GetMouseState(InputManager.MouseState.Y_DELTA) * mSensibility;
            float x_angle = x_delta_angle + transform.rotation.eulerAngles.x;
            float x_clamped_angle = Mathf.Clamp(x_angle, -90.0f, 90.0f);
            transform.rotation = Quaternion.Euler(x_clamped_angle, 0, 0);
        }
    }
}
