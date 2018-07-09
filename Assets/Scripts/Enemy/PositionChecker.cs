using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class PositionChecker : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Will be added PlacementTest layer by default")]
        LayerMask mIntersectLayer;

        public delegate void StepAllowed();
        event StepAllowed mStepAllowedAction;

        GameObject mTestObject;
        Collider mColliderToIgnore;

        static readonly float POLL_MAX_INTERVAL = 0.1f;

        /////////////////////////////////////////////
        void Start()
        {
            mTestObject = new GameObject();
            mTestObject.AddComponent<BoxCollider>();
            mTestObject.layer = LayerMask.NameToLayer("PlacementTest");
            mTestObject.SetActive(false);
            mIntersectLayer |= (1 << LayerMask.NameToLayer("PlacementTest"));
            //Doesn't matter if doesn't exists. Can be a problem if the collider is not in the same GO
            mColliderToIgnore = GetComponent<Collider>();
        }

        /////////////////////////////////////////////
        private void OnDestroy()
        {
            GameObject.Destroy(mTestObject);
        }

        /////////////////////////////////////////////
        public void RegisterStepAllowedAction(StepAllowed callback)
        {
            mStepAllowedAction += callback;
        }

        /////////////////////////////////////////////
        public void CheckPosition(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            IEnumerator coroutine = CheckPositionCoroutine(position, scale, rotation);
            StartCoroutine(coroutine);
        }

        /////////////////////////////////////////////
        IEnumerator CheckPositionCoroutine(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            //this not work when the rotation side is not a square
            //The collider scale must be Vector3.one
            mTestObject.SetActive(false);

            yield return new WaitForFixedUpdate();

            if (mColliderToIgnore != null)
                mColliderToIgnore.enabled = false;
            while (!BookPosition(position, scale, rotation))
            {
                Debug.Log("COLLISION");
                if (mColliderToIgnore != null)
                    mColliderToIgnore.enabled = true;
                yield return new WaitForSeconds(Random.value * POLL_MAX_INTERVAL);
                yield return new WaitForFixedUpdate();
                if (mColliderToIgnore != null)
                    mColliderToIgnore.enabled = false;
            }

            if (mColliderToIgnore != null)
                mColliderToIgnore.enabled = true;

            if (mStepAllowedAction != null)
                mStepAllowedAction();
        }

        /////////////////////////////////////////////
        public bool BookPosition(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            if (IsAreaFree(position, scale, rotation))
            {
                //We take our space
                mTestObject.transform.position = position;
                mTestObject.transform.rotation = rotation;
                mTestObject.transform.localScale = scale;
                mTestObject.SetActive(true);
                return true;
            }
            return false;
        }

#if DEBUG
        Vector3 mPosition;
        Vector3 mScale;
        Quaternion mRotation;
#endif

        /////////////////////////////////////////////
        public bool IsAreaFree(Vector3 position, Vector3 scale, Quaternion rotation)
        {
#if DEBUG
            mPosition = position;
            mScale = scale;
            mRotation = rotation;
#endif
            return !Physics.CheckBox(position, scale, rotation, mIntersectLayer);
        }

        /////////////////////////////////////////////
        public static bool CheckAreaAvailability(Vector3 position, Vector3 scale, Quaternion rotation, LayerMask layer)
        {
            return Physics.CheckBox(position, scale, rotation, layer);
        }

#if DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(mPosition, mScale);
        }
#endif
    }
}