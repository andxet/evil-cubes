using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class TitanCube : Enemy
    {
        [SerializeField]
        float mRestTime = 1.5f;

        enum TitanCubeState{
            END_STEP,
            REST
        }
        TitanCubeState mCurrentTitanState = TitanCubeState.REST;
        float mEndRest = 0;

        /////////////////////////////////////////////
        protected override void CalculateMove()
        {
            switch(mCurrentTitanState)
            {
                case TitanCubeState.END_STEP:
                    mEndRest = Time.timeSinceLevelLoad + mRestTime;
                    mCurrentTitanState = TitanCubeState.REST;
                    break;
                case TitanCubeState.REST:
                    if(Time.timeSinceLevelLoad > mEndRest)
                    {
                        mCurrentTitanState = TitanCubeState.END_STEP;
                        DoStepWhenPossible();
                    }
                    break;
            }
        }
    }
}
