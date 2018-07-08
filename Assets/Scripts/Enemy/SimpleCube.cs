using System.Collections;
using System.Collections.Generic;
using EvilCubes.Core;
using UnityEngine;

namespace EvilCubes.Enemy
{
    public class SimpleCube : Enemy
    {
        protected override void CalculateMove()
        {
            if (IsNearPlayer())
                StartStep();
            else
                DoStepWhenPossible();
        }
    }
}