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
        int mMaxConsecutiveSteps = 5;

        bool mIsRotating = false;
        int mStepsRemaining;
        Vector3 mDestination = Vector3.zero;

        enum Direction
        {
            INIT,
            DIRECTION_LEFT,
            DIRECTION_FORWARD,
            DIRECTION_RIGHT
        }
        Direction currentDirection = Direction.INIT;

        new void Start()
        {
            base.Start();
            GameManager gameManager = GameManager.GetInstance();
            if (gameManager != null)
            {
                PlayerManager player = gameManager.GetPlayer();
                if (player != null)
                    mDestination = player.transform.position;
            }
            mDestination.y = transform.position.y;
            mStepsRemaining = GenerateRandomSteps();
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
                    if (Vector3.Distance(transform.position, mDestination) > mMaxDistance)
                    {//Here we are too much distant from the player
                        if (currentDirection != Direction.DIRECTION_FORWARD)
                            mStepsRemaining = 0;//force to turn
                        else if (mStepsRemaining == 0 && currentDirection == Direction.DIRECTION_FORWARD)
                            mStepsRemaining = GenerateRandomSteps();
                    }

                    if (mStepsRemaining == 0)
                    {
                        //We need to turn...
                        mStepsRemaining = GenerateRandomSteps();
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
                                break;
                            case Direction.DIRECTION_LEFT:
                            case Direction.DIRECTION_RIGHT:
                            case Direction.INIT:
                                currentDirection = Direction.DIRECTION_FORWARD;
                                LookAtDestination();
                                break;
                        }
                    }
                    else
                    {
                        DoStepWhenPossible();
                        mStepsRemaining--;
                    }
                }
            }
        }

        /////////////////////////////////////////////
        int GenerateRandomSteps()
        {
            return Random.Range(1, mMaxConsecutiveSteps + 1);
        }

        /////////////////////////////////////////////
        protected override Vector3 NextStepPosition()
        {
            return transform.position + mMovementComponent.GetDirectionVector();
        }

        /////////////////////////////////////////////
        void LookAtDestination()
        {
            Quaternion rotation = new Quaternion();
            Vector3 from = mMovementComponent.GetDirectionVector();
            Vector3 to = mDestination - transform.position;
            to.Normalize();
            to.y = from.y;
            rotation.SetFromToRotation(from, to);
            StartRotationCoroutine(rotation);
        }

        /////////////////////////////////////////////
        void TurnLeft()
        {
            Quaternion rotation = Quaternion.AngleAxis(-90, Vector3.up);
            StartRotationCoroutine(rotation);
        }

        /////////////////////////////////////////////
        void TurnRight()
        {
            Quaternion rotation = Quaternion.AngleAxis(90, Vector3.up);
            StartRotationCoroutine(rotation);
        }

        /////////////////////////////////////////////
        void StartRotationCoroutine(Quaternion rotation)
        {
            IEnumerator rotationCoroutine = Rotate(rotation);
            StartCoroutine(rotationCoroutine);
        }

        /////////////////////////////////////////////
        IEnumerator Rotate(Quaternion rotation)
        {
            mIsRotating = true;

            Quaternion startRotation = transform.rotation;
            Quaternion destRotation = rotation * startRotation;
            Debug.Log("Starting rotation coroutine");
            float progress = 0;
            float startTime = Time.timeSinceLevelLoad;
            while (progress < 1)
            {
                transform.rotation = Quaternion.Lerp(startRotation, destRotation, progress);
                yield return null;
                progress = (Time.timeSinceLevelLoad - startTime) / mRotationVelocity;
            }
            transform.rotation = destRotation;

            mIsRotating = false;
        }
    }
}
