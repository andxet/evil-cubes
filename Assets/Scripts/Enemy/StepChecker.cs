using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class StepChecker : MonoBehaviour
    {
        public delegate void StepAllowed();
        event StepAllowed mStepAllowedAction;

        //bool currentInCollision;
        Collider mCollider;
        bool stepCheckRunning;
        int mCurrentTriggers;

        /////////////////////////////////////////////
        void Start()
        {
            mCollider = GetComponent<Collider>();
            mCollider.enabled = false;
            mCurrentTriggers = 0;
        }

        /////////////////////////////////////////////
        public void RegisterStepAllowedAction(StepAllowed callback)
        {
            mStepAllowedAction += callback;
        }

        /////////////////////////////////////////////
        void FixedUpdate()
        {
            if (stepCheckRunning)
                Debug.Log("FIXED UPDATE");
        }

        /////////////////////////////////////////////
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("COLLIDER enter!!" + name + " " + other.name + " " + mCurrentTriggers);
            mCurrentTriggers++;
        }

        /////////////////////////////////////////////
        void OnTriggerExit(Collider other)
        {
            Debug.Log("COLLIDER exit!!" + name + " " + other.name + " " + mCurrentTriggers);
            mCurrentTriggers--;
        }

        /////////////////////////////////////////////
        public void CheckStep()
        {
            StartCoroutine("CheckStepCoroutine");
        }

        /////////////////////////////////////////////
        IEnumerator CheckStepCoroutine()
        {
            //currentInCollision = false;
            mCollider.enabled = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => mCurrentTriggers == 0);

            if (mStepAllowedAction != null)
                mStepAllowedAction();
            stepCheckRunning = false;
        }


        /*IEnumerator CheckStepCoroutine()
        {
            bool checkNeeded = true;
            while (checkNeeded)
            {
                //currentInCollision = false;
                mCollider.enabled = true;
                yield return new WaitForFixedUpdate();
                if (currentInCollision)
                {
                    mCollider.enabled = false;
                    stepCheckRunning = false;
                    yield return new WaitForSeconds(0.1f);
                    stepCheckRunning = true;
                }
                else
                    checkNeeded = false;
                Debug.Log("cycle");
            }

            if (mStepAllowedAction != null)
                mStepAllowedAction();
            stepCheckRunning = false;
        }*/
    }
}