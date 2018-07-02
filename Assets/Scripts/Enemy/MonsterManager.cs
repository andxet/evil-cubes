using System.Collections;
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
        Enemy mBossEnemyPrefab;
        [SerializeField]
        int mNumMonsterToSpawnForBoss = 20;


        PlayerManager mPlayer;
        Dictionary<float, Enemy> mEnemyWithProbability = new Dictionary<float, Enemy>();
        int mEnemiesCreated = 0;
        List<Enemy> mMonsterList = new List<Enemy>();
        float mMaxProbability;

        /////////////////////////////////////////////
        void Start()
        {
            if (mEnemiesPrefabs.Count == 0 || mNumMonsterToSpawnForBoss < 0)
            {
                Debug.LogError("PlayerManager: This manager is not correctly initialized.");
                enabled = false;
                return;
            }

            mPlayer = GameManager.GetInstance().GetPlayer();
            StartCoroutine("SpawnMonsters");
            float mMaxProbability = 0;
            foreach(GameObject go in mEnemiesPrefabs)
            {
                Enemy enemy = go.GetComponent<Enemy>();
                if(enemy == null)
                {
                    Debug.LogWarning("EnemyManager: Added a prefab that isn't an enemy!");
                    continue;
                }
                mMaxProbability += enemy.GetSpawnPobability();
                mEnemyWithProbability.Add(mMaxProbability, enemy);
            }
        }


        /////////////////////////////////////////////
        void Update()
        {

        }

        /////////////////////////////////////////////
        IEnumerator SpawnMonsters()
        {
            while(mEnemiesCreated < mNumMonsterToSpawnForBoss)
            {
                SpawnMonster();
                //Generate the next after some seconds
                yield return new WaitForSeconds(Random.Range(0, 5));
            }
            while (mMonsterList.Count != 0)
                yield return null;

            SpawnBoss();
        }

        /////////////////////////////////////////////
        void SpawnMonster()
        {
            float p = Random.Range(0, mMaxProbability);
            int i = 0;
            //while(p > mEnemyWithProbability.Keys[i])
                //TODO
        }  

        /////////////////////////////////////////////
        void SpawnBoss()
        {
            
        }  
    }
}
