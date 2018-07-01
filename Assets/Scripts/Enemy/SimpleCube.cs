using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class SimpleCube : Enemy
    {
        enum State
        {
            CALCULATE_MOVE,
            MOVING
        };
        State mCurrentState;

        /////////////////////////////////////////////
        void Start()
        {
            base.Start();
            mCurrentState = State.CALCULATE_MOVE;
        }

        /////////////////////////////////////////////
        void Update()
        {
            switch(mCurrentState)
            {
                case State.CALCULATE_MOVE:
                    mMovementComponent.Step();
                    mCurrentState = State.MOVING;
                    break;
                case State.MOVING:
                    if (!mMovementComponent.IsMoving())
                        mCurrentState = State.CALCULATE_MOVE;
                    break;
            }
        }

        /////////////////////////////////////////////
        public void OnTriggerEnter(Collider other)
        {

            Debug.Log("COLLISION CUBE");
        }

        /////////////////////////////////////////////
        protected void Die()
        {
            Debug.Log("die callback child");
            base.Die();
        }
    }
}