﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvilCubes.Core;

namespace EvilCubes.Enemy
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> mEnemiesPrefabs = new List<GameObject>();
        [SerializeField]
        GameObject mBossEnemyPrefab;
        [SerializeField]
        int mNumMonsterToSpawnForBoss = 20;
        [SerializeField]
        int mSecondsBeforeStart = 2;


        PlayerManager mPlayer;
        Dictionary<float, GameObject> mEnemyWithProbability = new Dictionary<float, GameObject>();
        int mEnemiesCreated = 0;
        List<Enemy> mEnemyList = new List<Enemy>();
        float mMaxProbability;
        IEnemySpawner enemySpawner;

        /////////////////////////////////////////////
        void Start()
        {
            if (mEnemiesPrefabs.Count == 0 || mNumMonsterToSpawnForBoss < 0)
            {
                Debug.LogError("MonsterManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }

            mPlayer = GameManager.GetInstance().GetPlayer();
            mMaxProbability = 0;

            //Populate the dictionary with cumulative probability
            foreach(GameObject go in mEnemiesPrefabs)
            {
                Enemy enemy = go.GetComponent<Enemy>();
                if(enemy == null)
                {
                    Debug.LogWarning("EnemyManager: Added a prefab that isn't an enemy!");
                    continue;
                }
                mMaxProbability += enemy.GetSpawnPobability();
                mEnemyWithProbability.Add(mMaxProbability, go);
            }
            Enemy en = mBossEnemyPrefab.GetComponent<Enemy>();
            if (en == null)
            {
                Debug.LogWarning("EnemyManager: Boss preffab prefab isn't an enemy!");
                mBossEnemyPrefab = null;
            }

            enemySpawner = new EnemySpawnerAroundCirconference();

            StartCoroutine("SpawnMonsters");
        }


        /////////////////////////////////////////////
        void Update()
        {

        }

        /////////////////////////////////////////////
        IEnumerator SpawnMonsters()
        {
            yield return new WaitForSeconds(mSecondsBeforeStart);

            while(mEnemiesCreated < mNumMonsterToSpawnForBoss)
            {
                SpawnMonster();
                //Generate the next after some seconds
                yield return new WaitForSeconds(Random.Range(0, 5));
            }
            while (mEnemyList.Count != 0)
                yield return null;

            SpawnBoss();
        }

        /////////////////////////////////////////////
        void SpawnMonster()
        {
            float probability = Random.Range(0, mMaxProbability);
            GameObject enemyToSpawn = null;
            foreach(KeyValuePair<float, GameObject> fe in mEnemyWithProbability)
            {
                if (probability <= fe.Key)
                {
                    enemyToSpawn = fe.Value;
                    break;
                }
            }
#if DEBUG
            if(enemyToSpawn == null)
            {
                Debug.LogError("MonsterManager: Failed to choose an enemy to spawn");
                return;
            }
#endif //DEBUG

            SpawnEnemy(enemyToSpawn);
        }  

        /////////////////////////////////////////////
        void SpawnBoss()
        {
            if(mBossEnemyPrefab != null)
            {
                SpawnEnemy(mBossEnemyPrefab);
            }
        }

        /////////////////////////////////////////////
        void SpawnEnemy(GameObject enemyToInstantiate)
        {
            Enemy enemy = enemySpawner.SpawnEnemy(enemyToInstantiate);
            if(enemy == null)
            {
                Debug.LogWarning("MonsterManager: Trying to spawn a non enemy object");
                return;
            }
            enemy.RegisterDieAction(DestroyEnemy);
            mEnemyList.Add(enemy);
        }

        /////////////////////////////////////////////
        void DestroyEnemy(Enemy enemy)
        {
            if(!mEnemyList.Remove(enemy))
            {
                Debug.LogWarning("Trying to remove non registered enemy " + enemy.name);
            }
        }
    }
}