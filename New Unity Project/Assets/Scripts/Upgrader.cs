using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader: MonoBehaviour
{
    public bool permanent;
    public static float timer = 15;
    public static bool menuIsOpen = false;
    public bool tempActive = false;
    public float openMenuTime;
    public Sprite[] powerUpMenuImages;
    public Sprite[] powerUpActiveImages;
    public static bool enhanceBreak;
    public int tempUpgradeCost;

    Weapon gun;
    Player player;

    private Camera cam;
    private Image timerMeter;
    private Vector2 timerBarSize;
    private float indicatorTime = 1.5f;
    private float indicatorRiseSpeed = 10;
    private GameObject hud;
    //private List<PowerUp> powerups = new List<PowerUp>();
    private GameObject menu;
    private List<TaggedImage> activeImages;
    private List<Image> activeImagesOnscreen;
    private Vector2 activeBasePosition = new Vector2(141.25f, -257.7f);
    private float activeOffset = 40;

    private float runSpeedScale = 1.7f;
    private float stuckRateScale = 2;
    private float fireRateScale = 1.5f;

    // Start is called before the first frame update
    void Start() {
        gun = GameObject.FindObjectOfType<Weapon>();
        player = GameObject.FindObjectOfType<Player>();
        cam = GameObject.FindObjectOfType<Camera>();
        hud = GameObject.Find("HudCanvas");
        timerMeter = GameObject.Find("UpgradeMeterInner").GetComponent<Image>();
        timerBarSize = timerMeter.rectTransform.sizeDelta;
        timerMeter.rectTransform.sizeDelta = new Vector2(0, timerBarSize.y);
        activeImages = new List<TaggedImage>();
        activeImagesOnscreen = new List<Image>();
        if(permanent) {
            menu = GameObject.Find("PowerUp Menu");
        }
        enhanceBreak = true;
    }

    //select and apply lasting upgrades
    public void getPermanentEnhancement(string selection, int cost) {
        if(player.coins < cost) {
            return;
        }
        string indicator = "";
        switch(selection) {
            case "doubleJump":
                player.totalJumps = 2;
                player.remainingJumps = player.totalJumps;
                player.jetpack = false;
                indicator = "Double jump";
                applyActives(new pupChange[] { new pupChange("doubleJump", 0, true), new pupChange("jetpack", 0, false) });
                break;
            case "jetpack":
                player.totalJumps = 1;
                player.jetpack = true;
                indicator = "Jetpack";
                applyActives(new pupChange[] { new pupChange("jetpack", 1, true), new pupChange("doubleJump", 1, false) });
                break;
            case "automatic":
                gun.applyMod("machinegun");
                indicator = "Automatic";
                applyActives(new pupChange[] { new pupChange("automatic", 2, true), new pupChange("shotgun", 2, false), new pupChange("burst", 2, false) });
                break;
            case "spray":
                gun.applyMod("shotgun");
                indicator = "Shotgun";
                applyActives(new pupChange[] { new pupChange("shotgun", 3, true), new pupChange("automatic", 3, false), new pupChange("burst", 3, false) });
                break;
            case "burst":
                gun.applyMod("burst");
                indicator = "Burst";
                applyActives(new pupChange[] { new pupChange("burst", 4, true), new pupChange("automatic", 4, false), new pupChange("shotgun", 4, false) });
                break;
            case "extraDrill":
                player.availableDrills = Mathf.Min(player.availableDrills+1,2);
                player.totalDrills = 2;
                indicator = "Extra drill";
                applyActives(new pupChange[] { new pupChange("extraDrill", 5, true) });
                break;
            case "shotCount":
                gun.alterMod("shots", 5, true);
                indicator = "Extra shots";
                applyActives(new pupChange[] { new pupChange("shots", 6, true) });
                break;
            case "drillSpeedUp":
                Nest.drillTime = 7;
                indicator = "Drill speed up";
                applyActives(new pupChange[] { new pupChange("drillSpeedUp", 7, true) });
                break;
        }
        player.changeCoin(-cost);
        Text msg = (Instantiate(Resources.Load("Upgrade Indicator Text"), cam.WorldToScreenPoint(transform.position/*+cam.transform.position*/), Quaternion.identity) as GameObject).GetComponent<Text>();
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
        Text msg = (Instantiate(Resources.Load("Upgrade Indicator Text"), cam.WorldToScreenPoint(transform.position/*+cam.transform.position*/), Quaternion.identity) as GameObject).GetComponent<Text>();
        msg.text = indicator;
        msg.transform.SetParent(GameObject.Find("HudCanvas").transform);
        StartCoroutine(enhancementTimer(rand, splashStore/*runSpeed, jamLimit, fireRate, splash*/));
        StartCoroutine(indicatorFloat(msg));
    }

    private void applyActives(pupChange[] changes) {
        foreach(pupChange change in changes) {
            if(change.activate) {
                for(int i = 0; i < activeImages.Count; i++) {
                    if(activeImages[i].tag == change.keyword) {
                        activeImages.RemoveAt(i);
                        Destroy(activeImagesOnscreen[i].gameObject);
                        activeImagesOnscreen.RemoveAt(i);
                        i--;
                    }
                }
                activeImages.Add(new TaggedImage(powerUpActiveImages[change.id], change.keyword));
            }
            else {
                for(int i = 0; i < activeImages.Count; i++) {
                    if(activeImages[i].tag == change.keyword) {
                        activeImages.RemoveAt(i);
                        Destroy(activeImagesOnscreen[i].gameObject);
                        activeImagesOnscreen.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        //clear existing images from hud, add new ones at activeBasePositon + (activeOffset * index, 0)
        for(int i = 0; i < activeImages.Count; i++) {
            Image temp = (Instantiate(Resources.Load("ActivePowerUpTemplate"), hud.transform) as GameObject).GetComponent<Image>();
            temp.sprite = activeImages[i].image;
            temp.rectTransform.anchoredPosition = activeBasePosition + new Vector2(activeOffset * i, 0);
            activeImagesOnscreen.Add(temp);
        }
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
        if(permanent) {
            menu.transform.position = transform.position;
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

    IEnumerator indicatorFloat(Text msg) {
        float startTime = Time.time;
        while(Time.time - startTime < indicatorTime) {
            msg.transform.position = msg.transform.position + new Vector3(0, Time.deltaTime * indicatorRiseSpeed, 0);
            yield return 0;
        }
        Destroy(msg.gameObject);
    }

    public IEnumerator openBuyMenu(bool opening) {
        if(!opening)
            Time.timeScale = 1;
        else
            menu.transform.GetChild(0).GetComponent<PowerUp>().select();
        float startTime = Time.time;
        float ratio = 0;
        if(opening) {
            player.stunned = true;
        }
        while(Time.time - startTime < openMenuTime) {
            ratio = (opening ? (Time.time - startTime) / openMenuTime : 1 - (Time.time - startTime) / openMenuTime);
            menu.transform.localScale = new Vector3(ratio, ratio, 1);
            yield return 0;
        }
        if(opening) {
            player.stunned = false;
        }
        menu.transform.localScale = (opening ? new Vector3(1, 1, 1) : new Vector3(0, 0, 1));
        menuIsOpen = opening;
        if(opening)
            Time.timeScale = 0;
    }
}

public struct TaggedImage {
    public Sprite image;
    public string tag;
    public TaggedImage(Sprite i, string t) {
        image = i;
        tag = t;
    }
}

public struct pupChange
{
    public string keyword;
    public int id;
    public bool activate;
    public pupChange(string keyword, int id, bool activate) {
        this.keyword = keyword;
        this.id = id;
        this.activate = activate;
    }
}