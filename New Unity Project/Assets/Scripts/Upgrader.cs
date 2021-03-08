using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    //select and apply lasting upgrades
    public void getPermanentEnhancement() {
        switch(Random.Range(0, 6)) {
            case 0:// "doubleJump":
                //change player tag
                break;
            case 1:// "jetpack":
                //change player tag
                break;
            case 2:// "automatic":
                //weapon upgrade
                break;
            case 3:// "spray":
                //weapon upgrade
                break;
            case 4:// "burst":
                //weapon upgrade
                break;
            case 5:// "extraDrill":
                //increment player value
                break;
            case 6:// "shotCount":
                //weapon upgrade
                break;
        }
        //tell player chosen item
    }

    //select and apply timed upgrades
    public void getTemporaryEnhancement() {
        switch(Random.Range(0,5)) {
            case 0:// "drillSpeedUp":
                //umm
                break;
            case 1:// "moveSpeedUp":
                //change player value
                break;
            case 2:// "jamLimitUp":
                //weapon upgrade
                break;
            case 3:// "enemySpeedDown":
                //change nest/gamemananger value
                break;
            case 4:// "fireRateUp":
                //weapon upgrade
                break;
            case 5:// "splashOn":
                //weapon upgrade
                break;
        }
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
