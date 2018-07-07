using System.Collections;
using System.Collections.Generic;
using EvilCubes.Core;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class SimpleCube : Enemy
    {
        [SerializeField]
        PositionChecker mStepChecker;

        enum State
        {
            CALCULATE_MOVE,
            CHECK_SPACE_FOR_STEP,
            MOVING
        };
        State mCurrentState;
        GameObject mPlayer;

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
            //This could be injected in other ways...
            mPlayer = GameManager.GetInstance().GetPlayer().gameObject;
        }

        /////////////////////////////////////////////
        void Update()
        {
            switch (mCurrentState)
            {
                case State.CALCULATE_MOVE:
                    if (IsNearPlayer())
                        StartStep();
                    else
                        CheckStep();
                    break;

                case State.CHECK_SPACE_FOR_STEP:
                    break;

                case State.MOVING:
                    if (!mMovementComponent.IsMoving())
                    {
                        mCurrentState = State.CALCULATE_MOVE;
                    }
                    break;
            }
        }

        /////////////////////////////////////////////
        protected void CheckStep()
        {
            mCurrentState = State.CHECK_SPACE_FOR_STEP;
            Vector3 position = transform.position + mMovementComponent.GetDirectionVector();
            mStepChecker.CheckPosition(position, transform.localScale, transform.rotation);
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
        
        /////////////////////////////////////////////
        Vector3 NextStepPosition()
        {
            return transform.position + mMovementComponent.GetDirectionVector();
        }

        /////////////////////////////////////////////
        bool IsNearPlayer()
        {
            return PositionChecker.CheckArea(NextStepPosition(), transform.localScale, transform.rotation, 1 << LayerMask.NameToLayer("Player"));
        }
    }
}