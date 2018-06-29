using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Demo
{
    public class DemoCamera : PlayerCamera
    {
        [SerializeField]
        float mVelocity = 10;

        // Update is called once per frame
        void Update()
        {

            transform.localRotation *= Quaternion.AngleAxis(mVelocity * Time.deltaTime, Vector3.up);
        }
    }
}