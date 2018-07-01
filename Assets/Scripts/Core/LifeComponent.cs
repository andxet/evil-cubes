using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EvilCubes.Core
{
    public class LifeComponent : MonoBehaviour
    {
        [SerializeField]
        int MaxLife;
        public int mLife { get; private set; }

        public delegate void DieAction();
        event DieAction dieAction;

        /////////////////////////////////////////////
        void Awake()
        {
            if(MaxLife <= 0)
            {
                Debug.LogError("LifeComponent: This component is not correctly initialized.");
                enabled = false;
                return;
            }
            mLife = MaxLife;
        }

        /////////////////////////////////////////////
        void Start()
        {

        }

        /////////////////////////////////////////////
        void Update()
        {
            if (mLife <= 0)
                Die();
        }

        /////////////////////////////////////////////
        public void Hit(int amount)
        {
            if(amount < 0)
            {
                Debug.LogError("Trying to set a negative damage.");
                return;
            }
            mLife -= amount;
        }

        /////////////////////////////////////////////
        public void Die()
        {
            Debug.Log(name + " is dead");
            if (dieAction != null)
                dieAction();
            else
                gameObject.SetActive(false);
        }

        /////////////////////////////////////////////
        public void RegisterDieAction(DieAction call)
        {
            dieAction += call;
        }

        /////////////////////////////////////////////
        public int GetMaxLife()
        {
            return MaxLife;
        }
    }
}
