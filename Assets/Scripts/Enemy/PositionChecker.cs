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
            while (CheckArea(position, scale, rotation, mIntersectLayer))
            {
                Debug.Log("COLLISION");
                if (mColliderToIgnore != null)
                    mColliderToIgnore.enabled = true;
                yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
                if (mColliderToIgnore != null)
                    mColliderToIgnore.enabled = false;
            }

            if (mColliderToIgnore != null)
                mColliderToIgnore.enabled = true;
            
            //We take our space
            mTestObject.transform.position = position;
            mTestObject.transform.rotation = rotation;
            mTestObject.transform.localScale = scale;
            mTestObject.SetActive(true);

            if (mStepAllowedAction != null)
                mStepAllowedAction();
        }


        public static bool CheckArea(Vector3 position, Vector3 scale, Quaternion rotation, LayerMask layer)
        {
            return Physics.CheckBox(position, scale, rotation, layer);
        }
    }
}