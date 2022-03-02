using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] foodPrefabs;
    [SerializeField] GameObject golemBossPrefab;

    public int enemyCount = 0;
    private int wave = 0;
    private float boundaries = 20f;
    private float foodSpawnChance = 0.6f;

    void Update()
    {
        // Check if there are enemies alive and spawn the next wave if dont
        if (enemyCount == 0)
        {
            wave++;

            // Start a boss fight after each 5 waves
            if (wave % 5 == 0)
            {
                StartBossWave();
            }
            else
            {
                StartWave(wave);
            }

            SpawnFood();
        }
    }

    private void StartWave(int wave)
    {
        // Spawn the desired number of enemies on random locations
        for (int i = 0; i < wave; i++)
        {
            // Pick one random enemy to spawn
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Spawn(enemyPrefabs[randomIndex]);
        }

        enemyCount = wave;
    }

    private void StartBossWave()
    {
        Spawn(golemBossPrefab);

        enemyCount = 1;
    }

    private void SpawnFood()
    {
        float foodSpawnCheck = Random.Range(0, 1f);

        if (foodSpawnCheck <= foodSpawnChance)
        {
            // Pick one random food to spawn
            int randomIndex = Random.Range(0, foodPrefabs.Length);
            Spawn(foodPrefabs[randomIndex]);
        }
    }

    private void Spawn(GameObject prefab)
    {
        // Generate the random location inside the environment
        Vector3 randomPos = new Vector3(Random.Range(-boundaries, boundaries), prefab.transform.position.y, Random.Range(-boundaries, boundaries));

        // Instantiate the prefab
        Instantiate(prefab, randomPos, prefab.transform.rotation);
    }
}
