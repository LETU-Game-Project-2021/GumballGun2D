using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    public bool permanent;
    public static float timer = 15;
    public bool tempActive = false;

    Weapon gun;
    Player player;

    private Camera cam;
    private Image timerMeter;
    private Vector2 timerBarSize;
    private float indicatorTime = 1.5f;
    private float indicatorRiseSpeed = 10;
    public int tempUpgradeCost;
    private List<PowerUp> powerups = new List<PowerUp>();

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
        float runSpeed = player.runSpeed;
        float jamLimit = gun.currentMod.stuckLimit;
        float fireRate = gun.currentMod.rate;
        bool splash = gun.currentMod.splash;
        switch(Random.Range(0,4)) {
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
        StartCoroutine(enhancementTimer(runSpeed, jamLimit, fireRate, splash));
        StartCoroutine(indicatorFloat(msg));
    }

    private void createPowerups() {
        powerups.Add(new PowerUp("doubleJump",   1, Resources.Load<Image>("PUp images/doubleJumpMenu.png"),     Resources.Load<Image>("PUp images/doubleJumpActive.png"),   "Allows you to perform an additional jump mid-air", this));
        powerups.Add(new PowerUp("jetpack",      1, Resources.Load<Image>("PUp images/jetpackMenu.png"),        Resources.Load<Image>("PUp images/jetpackActive.png"),      "Allows you to fly through the sky (hold jump key)", this));
        powerups.Add(new PowerUp("automatic",    1, Resources.Load<Image>("PUp images/automaticMenu.png"),      Resources.Load<Image>("PUp images/automaticActive.png"),    "Fire a continuous stream of gumballs (click and hold)", this));
        powerups.Add(new PowerUp("spray",        1, Resources.Load<Image>("PUp images/sprayMenu.png"),          Resources.Load<Image>("PUp images/sprayActive.png"),        "Spray a cluster of gumballs with each shot", this));
        powerups.Add(new PowerUp("burst",        1, Resources.Load<Image>("PUp images/burstMenu.png"),          Resources.Load<Image>("PUp images/burstActive.png"),        "Rapidly fire multiple gumballs with each shot", this));
        powerups.Add(new PowerUp("extraDrill",   1, Resources.Load<Image>("PUp images/extraDrillMenu.png"),     Resources.Load<Image>("PUp images/extraDrillActive.png"),   "Allows an additional drill to be placed down", this));
        powerups.Add(new PowerUp("shotCount",    1, Resources.Load<Image>("PUp images/shotCountMenu.png"),      Resources.Load<Image>("PUp images/shotCountActive.png"),    "Increases the number of gumballs in \"spray\" and \"burst\" modes", this));
        powerups.Add(new PowerUp("drillSpeedUp", 1, Resources.Load<Image>("PUp images/drillSpeedUpMenu.png"),   Resources.Load<Image>("PUp images/drillSpeedUpActive.png"), "Decreases the time required to drill a nest", this));
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

    IEnumerator enhancementTimer(string status) {
        float startTime = Time.time;
        while(Time.time - startTime < timer) {
            float width = (timer - (Time.time - startTime)) / timer * timerBarSize.x;
            timerMeter.rectTransform.sizeDelta = new Vector2(width, timerBarSize.y);
            yield return 0;
        }
        timerMeter.rectTransform.sizeDelta = new Vector2(0, timerBarSize.y);
        switch(status) {
            case "speed":
                player.runSpeed /= runSpeedScale;
                break;
            case "stuck":
                gun.alterMod("stuckLimit", gun.currentMod.stuckLimit / stuckRateScale, false);
                break;
            case "rate":
                gun.alterMod("rate", gun.currentMod.rate * fireRateScale, false);
                break;
            case "splash":
                gun.alterMod("splash", false, false);
                break;
        }
    }

    IEnumerator enhancementTimer(float runSpeed, float jamLimit, float fireRate, bool splash) {
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
    }

    IEnumerator indicatorFloat(Text msg) {
        float startTime = Time.time;
        while(Time.time - startTime < indicatorTime) {
            msg.transform.position = msg.transform.position + new Vector3(0, Time.deltaTime * indicatorRiseSpeed, 0);
            yield return 0;
        }
        Destroy(msg.gameObject);
    }
}
