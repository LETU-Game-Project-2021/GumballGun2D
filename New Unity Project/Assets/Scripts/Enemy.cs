using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]public int eHealth;
    [SerializeField]public int enemyType;

    public bool stuck = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (enemyType) {
            case 0:
                eHealth = 1;
                break;
            case 1:
                eHealth = 2;
                break;
            case 2:
                eHealth = 1;
                //fly
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
