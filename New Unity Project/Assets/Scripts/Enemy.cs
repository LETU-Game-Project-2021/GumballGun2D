using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int eHealth;
    [SerializeField] public int enemyType;
    [SerializeField] public int speed;

    public bool stuck = false;
    public bool fly = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (enemyType) {
            case 0:
                eHealth = 1;
                speed = 2;
                break;
            case 1:
                eHealth = 2;
                speed = 1;
                break;
            case 2:
                eHealth = 1;
                fly = true;
                break;
        }
    }

    void FixedUpdate()
    {
        
    }

    public void Damage() {
        eHealth--;
        if (eHealth <= 0) {
            stuck = true;
            Stuck();
        }
    }

    public void Stuck() {
        speed = 0;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        this.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.magenta);
    }
}
