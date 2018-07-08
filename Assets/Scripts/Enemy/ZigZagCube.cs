using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Enemy
{
    public class ZigZagCube : Enemy
    {
        [SerializeField]
        float mMaxDistance = 10;
        [SerializeField]
        [Tooltip("Time to rotate 90 degrees")]
        float mRotationVelocity = 0.6f;
        [SerializeField]
        float mTurnProbability = 0.3f;

        bool mIsRotating = false;
        float mCurrentTurnProbability;
        Vector3 mDestination = Vector3.zero;

        enum Direction
        {
            DIRECTION_LEFT,
            DIRECTION_FORWARD,
            DIRECTION_RIGHT
        }
        Direction currentDirection = Direction.DIRECTION_FORWARD;

        new void Start()
        {
            base.Start();
            mCurrentTurnProbability = mTurnProbability;
            PlayerManager player = GameManager.GetInstance().GetPlayer();
            if (player != null)
                mDestination = player.transform.position;
            mDestination.y = transform.position.y;
        }
        /////////////////////////////////////////////
        protected override void CalculateMove()
        {
            
            if (!mIsRotating)
            {
                if (IsNearPlayer())
                    StartStep();
                else
                {
                    //Limit the distance from the player
                    if (currentDirection != Direction.DIRECTION_FORWARD && Vector3.Distance(transform.position, mDestination) > mMaxDistance)
                        mCurrentTurnProbability = 1;
                    if (Random.value < mCurrentTurnProbability)
                    {
                        //We need to turn...
                        switch (currentDirection)
                        {

                            case Direction.DIRECTION_FORWARD:
                                if (Random.value <= 0.5)
                                {
                                    currentDirection = Direction.DIRECTION_LEFT;
                                    TurnLeft();
                                }
                                else
                                {
                                    currentDirection = Direction.DIRECTION_RIGHT;
                                    TurnRight();
                                }
                                mCurrentTurnProbability = mTurnProbability;
                                break;
                            case Direction.DIRECTION_LEFT:
                                currentDirection = Direction.DIRECTION_FORWARD;
                                //LookAtDestination();
                                TurnRight();
                                mCurrentTurnProbability = mTurnProbability;
                                break;
                            case Direction.DIRECTION_RIGHT:
                                //Turn again to forward
                                currentDirection = Direction.DIRECTION_FORWARD;
                                //LookAtDestination();
                                TurnLeft();
                                mCurrentTurnProbability = mTurnProbability;
                                break;
                        }
                    }
                    else
                        DoStepWhenPossible();
                }
            }
        }

        /////////////////////////////////////////////
        protected override Vector3 NextStepPosition()
        {
            return transform.position + mMovementComponent.GetDirectionVector();
        }

        /////////////////////////////////////////////
        void LookAtDestination()
        {
            mIsRotating = true;
            //Rotate(//magic);
        }

        /////////////////////////////////////////////
        void TurnLeft()
        {
            mIsRotating = true;
            IEnumerator rotationCoroutine = Rotate(-90);
            StartCoroutine(rotationCoroutine);
        }

        /////////////////////////////////////////////
        void TurnRight()
        {
            mIsRotating = true;
            IEnumerator rotationCoroutine = Rotate(90);
            StartCoroutine(rotationCoroutine);
        }

        /////////////////////////////////////////////
        IEnumerator Rotate(float degrees)
        {
            Debug.Log("Starting rotation coroutine");
            float sign = Mathf.Sign(degrees);
            degrees = Mathf.Abs(degrees);
            do
            {
                float deltaRotation = 90 / mRotationVelocity * Time.deltaTime;
                if (deltaRotation > degrees)
                    deltaRotation = degrees;
                transform.Rotate(Vector3.up, sign * deltaRotation, Space.World);
                degrees -= deltaRotation;
                Debug.Log("remaining degrees: " + degrees);
                if (degrees > 0)
                    yield return null;
            } while (degrees > 0);

            mIsRotating = false;
        }
    }
}
