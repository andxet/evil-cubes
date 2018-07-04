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

            //Set position
            Vector3 position = Vector3.zero;
            GetPosition(ref position);
            position.y = enemy.GetHeight() / 2;
            enemy.transform.position = position;

            //Set rotation
            Vector3 lookatPosition = mPlayerObject.transform.position;
            lookatPosition.y = enemy.transform.position.y;
            enemy.transform.LookAt(mPlayerObject.transform.position);
            return enemy;
        }

        /////////////////////////////////////////////
        void GetPosition(ref Vector3 position)
        {
            //casual angle, get sin and cos
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            //Sum the player position, so that the player will be in the center of the circle
            Vector3 playerPosition = mPlayerObject.transform.position;
            position.x = randomPosition.x + playerPosition.x;
            position.z = randomPosition.y + playerPosition.z;
            position *= DISTANCE;
        }
    }
}
