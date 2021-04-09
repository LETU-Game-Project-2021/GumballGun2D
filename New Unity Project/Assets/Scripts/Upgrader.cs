using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    public bool permanent;
    public static float timer = 15;
    public bool tempActive = false;
    public float openMenuTime;
    public Sprite[] powerUpMenuImages;
    public Sprite[] powerUpActiveImages;

    Weapon gun;
    Player player;

    private Camera cam;
    private Image timerMeter;
    private Vector2 timerBarSize;
    private float indicatorTime = 1.5f;
    private float indicatorRiseSpeed = 10;
    public int tempUpgradeCost;
    private List<PowerUp> powerups = new List<PowerUp>();
    private GameObject menu;

    private float runSpeedScale = 1.7f;
    private float stuckRateScale = 2;
    private float fireRateScale = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.FindObjectOfType<Weapon>();
        player = GameObject.FindObjectOfType<Player>();
        cam = GameObject.FindObjectOfType<Camera>();
        timerMeter = GameObject.Find("UpgradeMeterInner").GetComponent<Image>();
        timerBarSize = timerMeter.rectTransform.sizeDelta;
        timerMeter.rectTransform.sizeDelta = new Vector2(0, timerBarSize.y);
        if(permanent) {
            createPowerups();
            menu = GameObject.Find("PowerUp Menu");
            loadMenu();
        }
    }

    //select and apply lasting upgrades
    public void getPermanentEnhancement(string selection, int cost) {
        if(player.coins < cost) {
            return;
        }
        string indicator = "";
        switch(selection) {
            case "doubleJump":
                player.totalJumps++;
                player.remainingJumps = player.totalJumps;
                player.jetpack = false;
                indicator = "Double jump";
                break;
            case "jetpack":
                player.totalJumps = 1;
                player.jetpack = true;
                indicator = "Jetpack";
                break;
            case "automatic":
                gun.applyMod("machinegun");
                indicator = "Automatic";
                break;
            case "spray":
                gun.applyMod("shotgun");
                indicator = "Shotgun";
                break;
            case "burst":
                gun.applyMod("burst");
                indicator = "Burst";
                break;
            case "extraDrill":
                player.availableDrills++;
                indicator = "Extra drill";
                break;
            case "shotCount":
                gun.alterMod("shots", gun.permanentMods.shots + 3, true);
                indicator = "Extra shots";
                break;
            case "drillSpeedUp":
                Nest.drillTime *= .7f;
                indicator = "Drill speed up";
                break;
        }
        player.changeCoin(-cost);
        Text msg = (Instantiate(Resources.Load("Upgrade Indicator Text"), cam.WorldToScreenPoint(transform.position), Quaternion.identity) as GameObject).GetComponent<Text>();
        msg.text = indicator;
        msg.transform.SetParent(GameObject.Find("HudCanvas").transform);
        StartCoroutine(indicatorFloat(msg));
    }

    //select and apply timed upgrades
    public void getTemporaryEnhancement() {
        tempActive = true;
        string indicator = "";
        /*float runSpeed = player.runSpeed;
        float jamLimit = gun.currentMod.stuckLimit;
        float fireRate = gun.currentMod.rate;*/
        bool splashStore = gun.currentMod.splash;
        int rand = Random.Range(0, 4);
        switch(rand) {
            case 0:// "moveSpeedUp":
                player.runSpeed *= runSpeedScale;
                indicator = "Speed up";
                break;
            case 1:// "jamLimitUp":
                gun.alterMod("stuckLimit", gun.currentMod.stuckLimit * stuckRateScale, false);
                indicator = "Jam limit up";
                break;
            case 2:// "fireRateUp":
                gun.alterMod("rate", gun.currentMod.rate / fireRateScale, false);
                indicator = "Fire rate up";
                break;
            case 3:// "splashOn":
                gun.alterMod("splash", true, false);
                indicator = "Splash";
                break;
        }
        Text msg = (Instantiate(Resources.Load("Upgrade Indicator Text"), cam.WorldToScreenPoint(transform.position), Quaternion.identity) as GameObject).GetComponent<Text>();
        msg.text = indicator;
        msg.transform.SetParent(GameObject.Find("HudCanvas").transform);
        StartCoroutine(enhancementTimer(rand, splashStore/*runSpeed, jamLimit, fireRate, splash*/));
        StartCoroutine(indicatorFloat(msg));
    }

    private void createPowerups() {
        powerups.Add(new PowerUp("doubleJump",   1, powerUpMenuImages[0], powerUpActiveImages[0], "Allows you to perform an additional jump mid-air", this));
        powerups.Add(new PowerUp("jetpack",      1, powerUpMenuImages[1], powerUpActiveImages[1], "Allows you to fly through the sky (hold jump key)", this));
        powerups.Add(new PowerUp("automatic",    1, powerUpMenuImages[2], powerUpActiveImages[2], "Fire a continuous stream of gumballs (click and hold)", this));
        powerups.Add(new PowerUp("spray",        1, powerUpMenuImages[3], powerUpActiveImages[3], "Spray a cluster of gumballs with each shot", this));
        powerups.Add(new PowerUp("burst",        1, powerUpMenuImages[4], powerUpActiveImages[4], "Rapidly fire multiple gumballs with each shot", this));
        powerups.Add(new PowerUp("extraDrill",   1, powerUpMenuImages[5], powerUpActiveImages[5], "Allows an additional drill to be placed down", this));
        powerups.Add(new PowerUp("shotCount",    1, powerUpMenuImages[6], powerUpActiveImages[6], "Increases the number of gumballs in \"spray\" and \"burst\" modes", this));
        powerups.Add(new PowerUp("drillSpeedUp", 1, powerUpMenuImages[7], powerUpActiveImages[7], "Decreases the time required to drill a nest", this));
    }

    private void loadMenu() {
        //int children = menu.transform.childCount-2;
        for(int i=0;i<powerups.Count;i++) {
            menu.transform.GetChild(2 * i).GetComponent<Image>().sprite = powerups[i].menuImg;
            menu.transform.GetChild(2 * i + 1).GetComponent<Text>().text = "Cost: " + powerups[i].cost;
        }
        menu.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(permanent) {
                collision.gameObject.GetComponent<Player>().upgradeP = true;
                collision.gameObject.GetComponent<Player>().upgraderPerm = this;
            }
            else {
                collision.gameObject.GetComponent<Player>().upgradeT = true;
                collision.gameObject.GetComponent<Player>().upgraderTemp = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(permanent) {
                collision.gameObject.GetComponent<Player>().upgradeP = false;
            }
            else {
                collision.gameObject.GetComponent<Player>().upgradeT = false;
            }
        }
    }

    public bool enhanceBreak = true;//yeah this is weird, I know
    IEnumerator enhancementTimer(int status, bool prevSplash) {
        float startTime = Time.time;
        PowerUp.waiting = true;
        while(enhanceBreak && Time.time - startTime < timer) {
            float width = (timer - (Time.time - startTime)) / timer * timerBarSize.x;
            timerMeter.rectTransform.sizeDelta = new Vector2(width, timerBarSize.y);
            yield return 0;
        }
        timerMeter.rectTransform.sizeDelta = new Vector2(0, timerBarSize.y);
        switch(status) {
            case 0:// "speed":
                player.runSpeed /= runSpeedScale;
                break;
            case 1:// "stuck":
                gun.alterMod("stuckLimit", gun.currentMod.stuckLimit / stuckRateScale, false);
                break;
            case 2:// "rate":
                gun.alterMod("rate", gun.currentMod.rate * fireRateScale, false);
                break;
            case 3:// "splash":
                gun.alterMod("splash", prevSplash, false);
                break;
        }
        PowerUp.waiting = false;
    }

    /*IEnumerator enhancementTimer(float runSpeed, float jamLimit, float fireRate, bool splash) {
        float startTime = Time.time;
        while(Time.time - startTime < timer) {
            float width = (timer-(Time.time-startTime)) / timer * timerBarSize.x;
            timerMeter.rectTransform.sizeDelta = new Vector2(width, timerBarSize.y);
            yield return 0;
        }
        timerMeter.rectTransform.sizeDelta = new Vector2(0, timerBarSize.y);
        player.runSpeed = runSpeed;
        gun.restoreFraction(jamLimit);
        gun.currentMod.rate = fireRate;
        gun.currentMod.splash = splash;
        tempActive = false;
    }*/

    IEnumerator indicatorFloat(Text msg) {
        float startTime = Time.time;
        while(Time.time - startTime < indicatorTime) {
            msg.transform.position = msg.transform.position + new Vector3(0, Time.deltaTime * indicatorRiseSpeed, 0);
            yield return 0;
        }
        Destroy(msg.gameObject);
    }

    public IEnumerator openBuyMenu(bool opening) {
        float startTime = Time.time;
        float ratio = 0;
        while(Time.time - startTime < openMenuTime) {
            ratio = (opening ? (Time.time-startTime)/openMenuTime : (1 - Time.time + startTime)/openMenuTime);
            menu.transform.localScale = new Vector3(ratio, ratio, 1);
            yield return 0;
        }
        menu.transform.localScale = (opening ? new Vector3(1, 1, 1) : new Vector3(0, 0, 1));
    }
}
