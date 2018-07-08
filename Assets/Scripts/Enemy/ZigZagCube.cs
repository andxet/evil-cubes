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
                                LookAtDestination();
                                mCurrentTurnProbability = mTurnProbability;
                                break;
                            case Direction.DIRECTION_RIGHT:
                                //Turn again to forward
                                currentDirection = Direction.DIRECTION_FORWARD;
                                LookAtDestination();
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
            while (progress < 1)
            {
                transform.rotation = Quaternion.Lerp(startRotation, destRotation, progress);
                yield return null;
                progress += Time.deltaTime;
            }
            transform.rotation = destRotation;

            mIsRotating = false;
        }
    }
}
