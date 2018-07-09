using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game namespaces
using EvilCubes.Core;
using EvilCubes.Util;

namespace EvilCubes.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        int mAttack = 10;
        [SerializeField]
        float mSpawnProbability = 30;
        [SerializeField]
        float mDieAnimationLength = 0.5f;
        [SerializeField]
        protected LifeComponent mLifeComponent;
        [SerializeField]
        protected WalkRotating mMovementComponent;
        [SerializeField]
        protected PositionChecker mStepChecker;

        enum State
        {
            CALCULATE_MOVE,
            CHECK_SPACE_FOR_STEP,
            CHILD_LOGIC,
            MOVING
        };
        State mCurrentState;

        protected bool mDied;
        protected Vector3 mDestination = Vector3.zero;

        public delegate void EnemyDieEvent(Enemy enemy);
        EnemyDieEvent mDieAction;

        /////////////////////////////////////////////
        protected void Awake()
        {
            if (mDieAnimationLength <= 0 || mLifeComponent == null || mMovementComponent == null)
            {
                Debug.LogError("Enemy: This component is not correctly initialized.");
                enabled = false;
                return;
            }
            mDied = false;
        }


        /////////////////////////////////////////////
        protected void Start()
        {
            if (mStepChecker == null)
            {
                Debug.LogError("SimpleCube: This component is not correctly initialized.");
                enabled = false;
                return;
            }

            GameManager gameManager = GameManager.GetInstance();
            if (gameManager != null)
            {
                PlayerManager player = gameManager.GetPlayer();
                if (player != null)
                    mDestination = player.transform.position;
            }
            mDestination.y = transform.position.y;
            mLifeComponent.RegisterDieAction(StartDie);
            mCurrentState = State.CALCULATE_MOVE;
            mStepChecker.RegisterStepAllowedAction(StartStep);
        }

        /////////////////////////////////////////////
        void Update()
        {
            switch (mCurrentState)
            {
                case State.CALCULATE_MOVE:
                    CalculateMove();
                    break;
                case State.CHILD_LOGIC:
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
        protected abstract void CalculateMove();

        /////////////////////////////////////////////
        protected void StartDie()
        {
            if (mDied)
                return;
            mDied = true;

            //Stop moving
            mMovementComponent.enabled = false;

            //Play a die animation
            StartCoroutine("DieAnimation");
        }

        /////////////////////////////////////////////
        protected void Die()
        {
            //If this object has a PoolElement, use the pool, otherwise destroy definitively
            //PoolElement poolElement = GetComponent<PoolElement>();
            //if (poolElement == null || !poolElement.Destroy())
            Destroy(gameObject);
            if (mDieAction != null)
                mDieAction(this);
        }

        /////////////////////////////////////////////
        IEnumerator DieAnimation()
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
            Vector3 originalScale = transform.localScale;
            Vector3 destScale = Vector3.zero;
            float elapsedTime = 0;
            //Wait next frame...
            yield return null;
            while(transform.localScale.x > 0 || transform.localScale.y > 0 || transform.localScale.z > 0)
            {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, destScale, elapsedTime / mDieAnimationLength);
                yield return null;
            }
            Die();
        }

        /////////////////////////////////////////////
        public void Hit(int damage)
        {
            mLifeComponent.Hit(damage);
        }

        /////////////////////////////////////////////
        public float GetSpawnPobability()
        {
            return mSpawnProbability;
        }

        /////////////////////////////////////////////
        public void RegisterDieAction(EnemyDieEvent ev)
        {
            mDieAction += ev;
        }

        /////////////////////////////////////////////
        public float GetHeight()
        {
            //Calculate the height
            BoxCollider collider = GetComponent<BoxCollider>();
            if(collider != null)
                return collider.size.y * transform.localScale.y;
            //Other collider options goes here
            return 0;
        }

        /////////////////////////////////////////////
        void OnTriggerEnter(Collider col)
        {
            PlayerManager player = col.GetComponent<PlayerManager>();
            LifeComponent life = col.GetComponent<LifeComponent>();
            if (player != null)
            {
                if(life != null)
                    life.Hit(mAttack);
                Hit(mLifeComponent.GetMaxLife());
            }
        }

        /////////////////////////////////////////////
        protected void DoStepWhenPossible()
        {
            mCurrentState = State.CHECK_SPACE_FOR_STEP;
            Vector3 position = NextStepPosition();
            mStepChecker.CheckPosition(position, transform.localScale, transform.rotation);
        }

        /////////////////////////////////////////////
        protected bool CheckStep()
        {
            //Check the step without do or book it
            mCurrentState = State.CHECK_SPACE_FOR_STEP;
            Vector3 position = NextStepPosition();
            return mStepChecker.IsAreaFree(position, transform.localScale, transform.rotation);
        }

        /////////////////////////////////////////////
        protected void StartStep()
        {
            //Begin the step coroutine without check
            mMovementComponent.Step();
            mCurrentState = State.MOVING;
        }

        /////////////////////////////////////////////
        protected virtual Vector3 NextStepPosition()
        {
            //Get the position of the next step (in front of the cube)
            return transform.position + mMovementComponent.GetDirectionVector() * transform.localScale.z;
        }

        /////////////////////////////////////////////
        protected bool IsNearPlayer()
        {
            return PositionChecker.CheckAreaAvailability(NextStepPosition(), transform.localScale, transform.rotation, 1 << LayerMask.NameToLayer("Player"));
        }

        /////////////////////////////////////////////
        protected float DistanceFromObjective()
        {
            //the distance from the current objective
            return Vector3.Distance(transform.position, mDestination);
        }
    }
}