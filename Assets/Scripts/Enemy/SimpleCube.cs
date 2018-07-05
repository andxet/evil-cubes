using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class SimpleCube : Enemy
    {
        [SerializeField]
        StepChecker mStepChecker;

        enum State
        {
            CALCULATE_MOVE,
            CHECK_SPACE_FOR_STEP,
            MOVING
        };
        State mCurrentState;

        /////////////////////////////////////////////
        new void Start()
        {
            base.Start();
            if (mStepChecker == null)
            {
                Debug.LogError("SimpleCube: This component is not correctly initialized.");
                enabled = false;
                return;
            }
            mCurrentState = State.CALCULATE_MOVE;
            mStepChecker.RegisterStepAllowedAction(StartStep);
        }

        /////////////////////////////////////////////
        void Update()
        {
            switch(mCurrentState)
            {
                case State.CALCULATE_MOVE:
                    mStepChecker.CheckStep();
                    mCurrentState = State.CHECK_SPACE_FOR_STEP;
                    break;

                case State.CHECK_SPACE_FOR_STEP:
                    break;
                    
                case State.MOVING:
                    if (!mMovementComponent.IsMoving())
                    {
                        if(mMovementComponent.transform.IsChildOf(transform))
                        {
                            transform.position = mMovementComponent.transform.position;
                            mMovementComponent.transform.localPosition = Vector3.zero;
                            mCurrentState = State.CALCULATE_MOVE;
                        }
                    }
                    break;
            }
        }

        /////////////////////////////////////////////
        protected void StartStep()
        {
            mMovementComponent.Step();
            mCurrentState = State.MOVING;
        }

        /////////////////////////////////////////////
        new void Die()
        {
            Debug.Log("die callback child");
            base.Die();
        }
    }
}