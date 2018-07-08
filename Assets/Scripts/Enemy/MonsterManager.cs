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
        GameObject mBossEnemyPrefab;
        [SerializeField]
        int mNumMonsterToSpawnForBoss = 20;
        [SerializeField]
        int mSecondsBeforeStart = 2;
        [SerializeField]
        int mSecondsBeforeBoss = 2;
        [SerializeField]
        int minumumSecondsBetweenSpawn = 0;
        [SerializeField]
        int maximumSecondsBetweenSpawn = 5;



        PlayerManager mPlayer;
        Dictionary<float, GameObject> mEnemyWithProbability = new Dictionary<float, GameObject>();
        int mEnemiesCreated = 0;
        List<Enemy> mEnemyList = new List<Enemy>();
        float mMaxProbability;

        public delegate void WinAction();
        WinAction mWinAction;
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
            foreach (GameObject go in mEnemiesPrefabs)
            {
                Enemy enemy = go.GetComponent<Enemy>();
                if (enemy == null)
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
        IEnumerator SpawnMonsters()
        {
            yield return new WaitForSeconds(mSecondsBeforeStart);

            while (mEnemiesCreated < mNumMonsterToSpawnForBoss)
            {
                //If not in pause
                yield return new WaitUntil(() => Time.timeScale > 0);
                if (ChooseEnemyAndSpawn())
                    mEnemiesCreated++;
                //Generate the next enemy after some seconds
                yield return new WaitForSeconds(Random.Range(minumumSecondsBetweenSpawn, maximumSecondsBetweenSpawn));
            }

            yield return new WaitUntil(() => mEnemyList.Count == 0);

            if (mBossEnemyPrefab != null)
            {
                yield return new WaitForSeconds(mSecondsBeforeBoss);

                SpawnBoss();

                if (mWinAction != null)
                {
                    yield return new WaitUntil(() => mEnemyList.Count == 0);
                    mWinAction();
                }
            }
        }

        /////////////////////////////////////////////
        bool ChooseEnemyAndSpawn()
        {
            float probability = Random.Range(0, mMaxProbability);
            GameObject enemyToSpawn = null;
            foreach (KeyValuePair<float, GameObject> fe in mEnemyWithProbability)
            {
                if (probability <= fe.Key)
                {
                    enemyToSpawn = fe.Value;
                    break;
                }
            }
#if DEBUG
            if (enemyToSpawn == null)
            {
                Debug.LogError("MonsterManager: Failed to choose an enemy to spawn");
                return false;
            }
#endif //DEBUG

            return SpawnEnemy(enemyToSpawn);
        }

        /////////////////////////////////////////////
        void SpawnBoss()
        {
            if (mBossEnemyPrefab != null)
            {
                SpawnEnemy(mBossEnemyPrefab);
            }
        }

        /////////////////////////////////////////////
        bool SpawnEnemy(GameObject enemyToInstantiate)
        {
            Enemy enemy = enemySpawner.SpawnEnemy(enemyToInstantiate);
            if (enemy == null)
            {
                Debug.LogWarning("MonsterManager: Failed to spawn enemy");
                return false;
            }
            enemy.RegisterDieAction(DestroyEnemy);
            mEnemyList.Add(enemy);
            return true;
        }

        /////////////////////////////////////////////
        public void RegisterWinAction(WinAction action)
        {
            mWinAction = action;
        }

        /////////////////////////////////////////////
        void DestroyEnemy(Enemy enemy)
        {
            if (!mEnemyList.Remove(enemy))
            {
                Debug.LogWarning("Trying to remove non registered enemy " + enemy.name);
            }
        }
    }
}
