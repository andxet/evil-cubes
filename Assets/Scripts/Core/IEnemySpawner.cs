using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCubes.Enemy{
    public interface IEnemySpawner
    {
        Vector3 GetPosition();
    }
}