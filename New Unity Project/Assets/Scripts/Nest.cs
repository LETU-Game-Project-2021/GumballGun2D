using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public float spawnRate = 5;
    public GameObject enemy;
    private float lastSpawn = 0f;
    private bool destroyed = false;

    private void Update()
    {
        lastSpawn += Time.deltaTime;

        if (!destroyed && lastSpawn > spawnRate)
        {

            lastSpawn = 0f;
            spawnEnemy();

        }
    }

    void spawnEnemy()
    {

        Instantiate(enemy, transform.position, Quaternion.identity);

    }

    public void destroy() {
        destroyed = true;
    }
}
