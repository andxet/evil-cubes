using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class JumpingCube : Enemy
    {
        [SerializeField]
        float mJumpDuration = 0.8f;
        [SerializeField]
        float mJumpProbability = 0.3f;

        bool mIsJumping = false;

        //Selected a function with this jump length.
        //Start from 0, go high to certain value, return to 0 after mJumpDistance
        float mJumpDistance = 3;
        Vector2 JumpValue(float norm_x)
        {
            float x = Mathf.Lerp(0, mJumpDistance, norm_x);
            float y = (float)3.552714e-15 + 4 * x - 1.333333f * x * x;
            return new Vector2(x, y);
        }


        protected override void CalculateMove()
        {
            if (!mIsJumping)
            {
                if (IsNearPlayer())
                    StartStep();
                else
                {
                    if (DistanceFromObjective() < mJumpDistance * transform.localScale.z || Random.value > mJumpProbability)
                        DoStepWhenPossible();
                    else if(JumpPossible())
                        Jump();
                }
            }
        }

        void Jump()
        {
            StartCoroutine("JumpCoroutine");
        }

        IEnumerator JumpCoroutine()
        {
            mIsJumping = true;
            Vector3 startPosition = transform.position;
            float jumpStartTime = Time.timeSinceLevelLoad;
            float jumpEndTime = jumpStartTime + mJumpDuration;
            while (Time.timeSinceLevelLoad < jumpEndTime)
            {
                Vector2 position = JumpValue((Time.timeSinceLevelLoad - jumpStartTime) / mJumpDuration);
                //Get a vector 3 like if we jump in front
                Vector3 position3 = new Vector3(0, position.y, position.x);
                //Scale the jumpto the actual scale
                position3 *= transform.localScale.z;
                //Rotate the jump to the current front
                Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, mMovementComponent.GetDirectionVector());
                transform.position = startPosition + rotation * position3;
                yield return null;
            }
            mIsJumping = false;
        }

        /////////////////////////////////////////////
        protected bool JumpPossible()
        {
            Vector3 position = transform.position + mMovementComponent.GetDirectionVector() * mJumpDistance * transform.localScale.z;
            return mStepChecker.BookPosition(position, transform.localScale, transform.rotation);
        }
    }
}
