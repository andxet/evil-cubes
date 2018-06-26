using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Appearance
{
    public class RotateRandomly : MonoBehaviour
    {
        private Vector3 mRotationPerSeconds;

        private void OnEnable()
        {
            mRotationPerSeconds = new Vector3(Random.Range(360, 720), Random.Range(360, 720), Random.Range(360, 720));
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(mRotationPerSeconds * Time.deltaTime);
        }
    }
}
