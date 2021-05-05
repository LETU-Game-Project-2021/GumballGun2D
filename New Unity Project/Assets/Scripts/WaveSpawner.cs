using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Gamemanager gameManager;
    public GameObject[] enemy;
    public int nestsDestroyed;
    public float waveLength = 15f;
    public bool spawn = false;
    private float waveTime;
    private float spawnRate;
    

    private void Start()
    {
        //nestsDestroyed = gameManager.GetComponent<Gamemanager>().nestsDestroyed;
        gameManager = FindObjectOfType<Gamemanager>();
        spawnRate = Random.Range(1, 5);
    }

    private void FixedUpdate()
    {
        //if (nestsDestroyed != gameManager.GetComponent<Gamemanager>().nestsDestroyed) {
        //    nestsDestroyed = gameManager.GetComponent<Gamemanager>().nestsDestroyed;
        //    SpawnWave();
        //}

        if (spawn)
        {
            waveLength -= Time.deltaTime;
            waveTime += Time.deltaTime;
            if (waveTime > spawnRate) {
                waveTime = 0f;
                spawnRate = Random.Range(3, 7);
                SpawnWaveEnemy();
            }

            if (waveLength <= 0) {
                spawn = false;
                waveLength = 15f;
                waveTime = 0f;
            }
        }
    }

    public void SpawnWave() {
        
        FindObjectOfType<SoundManager>().Play("Wave Spawner");
        spawn = true;

    }

    private void SpawnWaveEnemy() {
        if (gameManager.roomNumber > 2) {
            return;
        }
        int rand = Random.Range(0, 1);
        enemy[rand].GetComponent<Enemy>().target = gameManager.gameObject.GetComponent<Gamemanager>().portalLocation.gameObject;
        Instantiate(enemy[rand], transform.position, Quaternion.identity);
    }
}
