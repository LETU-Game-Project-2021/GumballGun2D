using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Gamemanager gameManager;
    public GameObject[] enemy;
    public int nestsDestroyed;

    private void Start()
    {
        nestsDestroyed = gameManager.GetComponent<Gamemanager>().nestsDestroyed;
        gameManager = FindObjectOfType<Gamemanager>();
    }

    private void FixedUpdate()
    {
        if (nestsDestroyed != gameManager.GetComponent<Gamemanager>().nestsDestroyed) {
            nestsDestroyed = gameManager.GetComponent<Gamemanager>().nestsDestroyed;
            SpawnWave();
        }
       
    }

    private void SpawnWave() {
        int rand = Random.Range(0, 1);
        Debug.Log("spawn");
        enemy[rand].GetComponent<Enemy>().target = gameManager.gameObject.GetComponent<Gamemanager>().portalLocation.gameObject;
        Instantiate(enemy[rand], transform.position, Quaternion.identity);
    }
}
