using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public float spawnRate = 5;
    public GameObject enemy;
    public GameObject gameManger;
    public bool currentDrill = false;
    private float lastSpawn = 0f;
    public bool destroyed = false;
    private float timeSinceDrill;
    public float drillTime;

    private void Start()
    {
        gameManger.gameObject.GetComponent<Gamemanager>().totalNests++;
    }

    private void Update()
    {
        lastSpawn += Time.deltaTime;

        if (!destroyed && lastSpawn > spawnRate)
        {

            lastSpawn = 0f;
            spawnEnemy();

        }

        if (currentDrill) {
            timeSinceDrill += Time.deltaTime;
            if (timeSinceDrill > drillTime && !destroyed) {
                destroyed = true;
                gameManger.gameObject.GetComponent<Gamemanager>().nestsDestroyed++;
                this.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.magenta);
            }
        }
    }

    void spawnEnemy()
    {

        Instantiate(enemy, transform.position, Quaternion.identity);

    }

    public void startDrill() {

        currentDrill = true;
        timeSinceDrill = Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!destroyed)
            {
                collision.gameObject.GetComponent<Player>().drill = true;
            }

            
            collision.gameObject.GetComponent<Player>().nest = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().drill = false;
        }

    }
}
