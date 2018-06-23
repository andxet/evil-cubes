using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game namespaces
using EvilCubes.Core;
using EvilCubes.Util;

namespace EvilCubes.Enemy
{
    [RequireComponent(typeof(LifeComponent))]
    public abstract class Enemy : MonoBehaviour
    {
        LifeComponent mLifeComponent;

        /////////////////////////////////////////////
        void Awake()
        {
            mLifeComponent = GetComponent<LifeComponent>();
        }


        /////////////////////////////////////////////
        void Start()
        {

        }

        /////////////////////////////////////////////
        void Update()
        {

        }

        /////////////////////////////////////////////
        /// <summary>
        /// If this object has a PoolElement, use the pool, otherwise destroy definitively
        /// </summary>
        void Die()
        {
            PoolElement poolElement = GetComponent<PoolElement>();
            if (poolElement == null || !poolElement.Destroy())
                Destroy(gameObject);
        }
    }
}