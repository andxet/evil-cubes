using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class WalkRotating : MonoBehaviour
    {
        [Tooltip("Time in second to complete a step (90°)")]
        [SerializeField]
        public float rotationTime = 1.0f;

        //Rotation remaining to complete this step
        float rotationRemaining;
        //A dictionary containing the local position of the pins
        Dictionary<int, Vector3> rotationPins = new Dictionary<int, Vector3>();
        //Index in the dictionary of the pin
        int currentPin;
        //position of the point around which rotate
        Vector3 pivot = Vector3.zero;

        /////////////////////////////////////////////
        void Start()
        {
            //Multiply with local scale to allow a non standard scale
            rotationPins.Add(0, Vector3.Scale(new Vector3( 0.0f, -0.5f,  0.5f), transform.localScale));
            rotationPins.Add(1, Vector3.Scale(new Vector3( 0.0f,  0.5f,  0.5f), transform.localScale));
            rotationPins.Add(2, Vector3.Scale(new Vector3( 0.0f,  0.5f, -0.5f), transform.localScale));
            rotationPins.Add(3, Vector3.Scale(new Vector3( 0.0f, -0.5f, -0.5f), transform.localScale));
            currentPin = 3;
            rotationRemaining = 0;
        }

        /////////////////////////////////////////////
        void Update()
        {
            if (rotationRemaining <= 0)
                return;
            
            //Pivot in world space
            pivot = transform.position + transform.rotation * rotationPins[currentPin];
            //rotate around the axis located at pivot
            float rotationDelta = Time.deltaTime * 90 / rotationTime;
            if (rotationDelta > rotationRemaining)
                rotationDelta = rotationRemaining;
            transform.RotateAround(pivot, transform.right, rotationDelta);
            rotationRemaining -= rotationDelta;
        }

        /////////////////////////////////////////////
        public void Step()
        {
            if (rotationRemaining > 0)
                return;
            
            rotationRemaining = 90;
            currentPin++;
            currentPin %= 4;
        }

        /////////////////////////////////////////////
        public bool IsMoving()
        {
            return rotationRemaining > 0;
        }

#if UNITY_EDITOR
        /////////////////////////////////////////////
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, transform.position - Vector3.Cross(Vector3.up, transform.right));
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(pivot, new Vector3(0.1f, 0.1f, 0.1f));
        }
#endif
    }
}