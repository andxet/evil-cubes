﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Util
{
    public class Util
    {
        /////////////////////////////////////////////
        public static Vector3 RandomInsideCone(float radius)
        {
            //(sqrt(1 - z^2) * cosϕ, sqrt(1 - z^2) * sinϕ, z)
            //Get angle radians
            float radradius = radius * Mathf.PI / 360;

            //Random circonference in the part of sphere inside the cone (angle alfa, get cos)
            float z = Random.Range(Mathf.Cos(radradius), 1);

            //Random point on the circonference
            float t = Random.Range(0, Mathf.PI * 2);

            //Get the coordinates of the point in the circonference
            //sin alfa * cos of the point in the circonference
            //sin alfa * sin of the point in the circonference
            //z
            return new Vector3(Mathf.Sqrt(1 - z * z) * Mathf.Cos(t), Mathf.Sqrt(1 - z * z) * Mathf.Sin(t), z);
        }
    }
}
