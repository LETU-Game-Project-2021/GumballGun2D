using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public float spawnRate = 5;
    public GameObject enemy;
    private float lastSpawn = 0f;

    private void Update()
    {
        lastSpawn += Time.deltaTime;

        if (lastSpawn > spawnRate)
        {

            lastSpawn = 0f;
            spawnEnemy();

        }
    }

    void spawnEnemy()
    {

        Instantiate(enemy, transform.position, Quaternion.identity);

    }
}
