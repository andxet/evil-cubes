using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class TitanCube : Enemy
    {
        [SerializeField]
        float mRestTime = 1.5f;

        enum State{
            END_STEP,
            REST
        }
        State mCurrentState = State.REST;
        float mEndRest = 0;

        protected override void CalculateMove()
        {
            switch(mCurrentState)
            {
                case State.END_STEP:
                    mEndRest = Time.timeSinceLevelLoad + mRestTime;
                    mCurrentState = State.REST;
                    break;
                case State.REST:
                    if(Time.timeSinceLevelLoad > mEndRest)
                    {
                        mCurrentState = State.END_STEP;
                        DoStepWhenPossible();
                    }
                    break;
            }
        }
    }
}
