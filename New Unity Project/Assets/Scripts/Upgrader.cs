using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    string[] upgradeList;
    // Start is called before the first frame update
    void Start()
    {
        upgradeList = new string[] { };
    }

    public void getEnhancement() {
        //choose thing from list
        //tell player chosen item
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Player>().upgrade = true;
            collision.gameObject.GetComponent<Player>().upgrader = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Player>().upgrade = false;
        }

    }
}
