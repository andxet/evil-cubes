using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game namespaces
using EvilCubes.Core;
using EvilCubes.Util;

namespace EvilCubes.Enemy
{
    [RequireComponent(typeof(LifeComponent))]
    [RequireComponent(typeof(WalkRotating))]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        int mAttack = 10;
        [SerializeField]
        float mSpawnProbability = 30;
        [SerializeField]
        float mDieAnimationLength = 0.5f;

        protected LifeComponent mLifeComponent;
        protected WalkRotating mMovementComponent;
        protected bool mDied;

        public delegate void EnemyDieEvent(Enemy enemy);
        EnemyDieEvent mDieAction;

        /////////////////////////////////////////////
        protected void Awake()
        {
            if (mDieAnimationLength <= 0)
            {
                Debug.LogError("Enemy: This component is not correctly initialized.");
                enabled = false;
                return;
            }
            mLifeComponent = GetComponent<LifeComponent>();
            mMovementComponent = GetComponent<WalkRotating>();
            mDied = false;
        }


        /////////////////////////////////////////////
        protected void Start()
        {
            mLifeComponent.RegisterDieAction(StartDie);
        }

        /////////////////////////////////////////////
        void Update()
        {

        }

        /////////////////////////////////////////////
        protected void StartDie()
        {
            if (mDied)
                return;
            mDied = true;
            Debug.Log("die callback");

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
            Debug.Log("Collision enemy");
            PlayerManager player = col.GetComponent<PlayerManager>();
            LifeComponent life = col.GetComponent<LifeComponent>();
            if (player != null && life != null)
            {
                life.Hit(mAttack);
                Hit(mLifeComponent.GetMaxLife());
            }
        }
    }
}