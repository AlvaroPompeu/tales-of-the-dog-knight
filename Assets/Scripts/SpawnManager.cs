using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab, powerUpPrefab;
    public int enemyCount = 0;
    private int wave = 0;
    private float boundaries = 20f;

    // Update is called once per frame
    void Update()
    {
        // Check if there are enemies alive and spawn the next wave if dont
        if (enemyCount == 0)
        {
            wave++;
            StartWave(wave);
        }
    }

    void StartWave(int wave)
    {
        // Spawn the desired number of enemies on random locations
        for (int i = 0; i < wave; i++)
        {
            Spawn(enemyPrefab);
        }

        // Spawn one power up
        Spawn(powerUpPrefab);

        enemyCount = wave;
    }

    void Spawn(GameObject prefab)
    {
        // Generate the random location inside the walls
        Vector3 randomPos = new Vector3(Random.Range(-boundaries, boundaries), prefab.transform.position.y, Random.Range(-boundaries, boundaries));

        // Instantiate the prefab
        Instantiate(prefab, randomPos, prefab.transform.rotation);
    }
}
