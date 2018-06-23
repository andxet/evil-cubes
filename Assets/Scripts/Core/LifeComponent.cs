using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EvilCubes.Core
{
    public class LifeComponent : MonoBehaviour
    {
        //publics
        public int MaxLife;
        public int Life{
            get; private set;
        }

        //privates
        public delegate void DieAction();
        event DieAction dieAction;

        /////////////////////////////////////////////
        void Awake()
        {
            Life = MaxLife;
        }

        void Start()
        {

        }
        /////////////////////////////////////////////
        void Update()
        {
            if (Life <= 0)
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
            Life -= amount;
        }

        /////////////////////////////////////////////
        public void Die()
        {
            if (dieAction != null)
                dieAction();
        }

        /////////////////////////////////////////////
        public void RegisterDieAction(DieAction call)
        {
            dieAction += call;
        }
    }
}
