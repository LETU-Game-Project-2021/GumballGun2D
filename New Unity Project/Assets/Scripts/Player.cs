using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool fly = false;
    public bool drill = false;
    public bool upgradeT = false;
    public bool upgradeP = false;
    public GameObject nest;
    public GameObject drillObject;
    public GameObject currentDrill;
    public Upgrader upgraderTemp;
    public Upgrader upgraderPerm;
    public Text coinCounter;
    public int availableDrills = 1;
    public int totalJumps = 1;
    public int remainingJumps = 0;
    public bool jetpack = false;
    public bool drillPickup = false;
    public int coins;

    private void Start() {
        coinCounter = GameObject.Find("CoinCount").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
        if(Input.GetButton("Jump")) {
            fly = true;
        }
        if(Input.GetButtonDown("Use")) {
            playerUse();
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, remainingJumps, fly, jetpack);
        jump = false;
        fly = false;
    }

    private void playerUse() {

        if(drill && availableDrills > 0) {
            Drill drill = (Instantiate(drillObject, nest.transform.position, Quaternion.identity) as GameObject).GetComponent<Drill>();
            FindObjectOfType<SoundManager>().Play("Drill Start");
            availableDrills--;
            nest.gameObject.GetComponent<Nest>().startDrill();
            StartCoroutine(drill.displayTimer(Nest.drillTime));
        }
        else if(upgradeP) {
            //bring up buy menu
            //but for now I'll pick a random one for testing and charge one coin
            string[] tempUpgradeList = { "doubleJump", "jetpack", "automatic", "spray", "burst", "extraDrill", "shotCount", "drillSpeedUp" };
            upgraderPerm.getPermanentEnhancement(tempUpgradeList[Random.Range(0, tempUpgradeList.Length)]);
        }
        else if(upgradeT && coins >= upgraderTemp.tempUpgradeCost) {
            if(!upgraderTemp.tempActive) {
                upgraderTemp.getTemporaryEnhancement();
                changeCoin(-upgraderTemp.tempUpgradeCost);
                FindObjectOfType<SoundManager>().Play("Gumball Machine");
            }
        }
        else if(drillPickup) {
            availableDrills++;
            Destroy(currentDrill);
            drillPickup = false;
        }
    }

    public void changeCoin(int diff) {
        coins += diff;
        coinCounter.text = coins.ToString();
    }
}
