using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Enemy
{
    public class EnemySpawnerAroundCirconference : IEnemySpawner
    {
        readonly float DISTANCE = 5;
        GameObject mPlayerObject;

        /////////////////////////////////////////////
        public Vector3 GetPosition()
        {
            if (mPlayerObject == null)
                mPlayerObject = GameManager.GetInstance().GetPlayer().gameObject;
            Vector3 playerPosition = mPlayerObject.transform.position;
            Vector2 randomPosition = Random.insideUnitCircle * DISTANCE;
            return new Vector3(randomPosition.x, 0, randomPosition.y);
        }
    }
}
