using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Enemy
{
    //Spawn and init an enemy
    public class EnemySpawnerAroundCirconference : IEnemySpawner
    {
        readonly float DISTANCE = 10;
        GameObject mPlayerObject;

        List<Vector2> generated = new List<Vector2>();

        /////////////////////////////////////////////
        public EnemySpawnerAroundCirconference()
        {
            if (mPlayerObject == null)
                mPlayerObject = GameManager.GetInstance().GetPlayer().gameObject;
        }

        /////////////////////////////////////////////
        public Enemy SpawnEnemy(GameObject enemyPrefab)
        {
            //Instantiate, set position and rotation
            GameObject enemyGameObject = Object.Instantiate(enemyPrefab);
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            if(enemy == null)
            {
                Object.Destroy(enemyGameObject);
                return null;
            }
            enemy.transform.position = GetPosition();
            Vector3 lookatPosition = mPlayerObject.transform.position;
            lookatPosition.y = enemy.GetHeight();
            enemy.transform.LookAt(mPlayerObject.transform.position);
            return enemy;
        }

        /////////////////////////////////////////////
        Vector3 GetPosition()
        {
            //casual angle, get sin and cos
            Vector3 playerPosition = mPlayerObject.transform.position;
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector3 randomPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            randomPosition.x += playerPosition.x;
            randomPosition.z += playerPosition.z;
            randomPosition *= DISTANCE;
            return randomPosition;
        }
    }
}
