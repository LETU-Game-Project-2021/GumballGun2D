using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    public float timer;
    Weapon gun;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.FindObjectOfType<Weapon>();
        player = GameObject.FindObjectOfType<Player>();
    }

    //select and apply lasting upgrades
    public void getPermanentEnhancement() {
        switch(Random.Range(0, 7)) {
            case 0:// "doubleJump":
                player.totalJumps++;
                player.remainingJumps = player.totalJumps;
                player.jetpack = false;
                player.doubleJump = true;
                Debug.Log("Double jump");
                break;
            case 1:// "jetpack":
                player.totalJumps = 1;
                player.jetpack = true;
                Debug.Log("Jetpack");
                break;
            case 2:// "automatic":
                gun.applyMod("machinegun");
                Debug.Log("Automatic");
                break;
            case 3:// "spray":
                gun.applyMod("shotgun");
                Debug.Log("Shotgun");
                break;
            case 4:// "burst":
                gun.applyMod("burst");
                Debug.Log("Burst");
                break;
            case 5:// "extraDrill":
                player.availableDrills++;
                Debug.Log("Extra drill");
                break;
            case 6:// "shotCount":
                gun.alterMod("shots", gun.permanentMods.shots + 3, true);
                Debug.Log("Extra shots");
                break;
            case 7:// "drillSpeedUp":
                Nest.drillTime *= .7f;
                break;
        }
    }

    //select and apply timed upgrades
    public void getTemporaryEnhancement() {
        float runSpeed = player.runSpeed;
        float jamLimit = gun.currentMod.stuckLimit;
        //float enemySpeed;
        float fireRate = gun.currentMod.rate;
        bool splash = gun.currentMod.splash;
        switch(Random.Range(0,5)) {
            case 0:// "moveSpeedUp":
                player.runSpeed *= 1.7f;
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
        StartCoroutine(enhancementTimer(runSpeed, jamLimit, /*enemySpeed, */fireRate, splash));
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

    IEnumerator enhancementTimer(float runSpeed, float jamLimit, /*float enemySpeed, */float fireRate, bool splash) {
        float startTime = Time.time;
        while(Time.time - startTime < timer) {
            //update timer UI
            yield return 0;
        }
        player.runSpeed = runSpeed;
        gun.currentMod.stuckLimit = jamLimit;
        gun.currentMod.rate = fireRate;
        gun.currentMod.splash = splash;
    }
}
