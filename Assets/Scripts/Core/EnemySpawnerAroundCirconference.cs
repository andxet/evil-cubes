using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Enemy
{
    //Spawn and init an enemy
    public class EnemySpawnerAroundCirconference : IEnemySpawner
    {
        [SerializeField]
        int mMaxTries = 5;

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
            if (enemy == null)
            {
                Object.Destroy(enemyGameObject);
                return null;
            }

            //Set position
            Vector3 position = Vector3.zero;

            int tries = 0;
            bool positionUnvailable;
            enemyGameObject.SetActive(false);
            do
            {
                GetPosition(ref position);
                position.y = enemy.GetHeight() / 2;
                enemy.transform.position = position;

                //Set rotation
                Vector3 lookatPosition = mPlayerObject.transform.position;
                lookatPosition.y = enemy.transform.position.y;
                enemy.transform.LookAt(mPlayerObject.transform.position);
                tries++;
                positionUnvailable = PositionChecker.CheckArea(enemy.transform.position, enemy.transform.localScale, enemy.transform.rotation, 1 << LayerMask.NameToLayer("Enemy"));
            } while (positionUnvailable && tries < mMaxTries);

            //Max attempts reached
            if(positionUnvailable)
            {
                GameObject.Destroy(enemy);
                return null;
            }

            enemyGameObject.SetActive(true);
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
