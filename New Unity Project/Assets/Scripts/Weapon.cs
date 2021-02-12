using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon: MonoBehaviour
{
    public Camera cam;
    public Rigidbody2D player;
    public Transform firePoint;

    private gunMod currentMod, permanentMods;
    private Rigidbody2D rb;
    private Dictionary<string, gunMod> modList;
    private float timeSinceFired = 0;
    private float burstDelay = 0.1f;
    private Vector2 playerPos, mousePos;

    // Start is called before the first frame update
    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        createMods();
        permanentMods = new gunMod(1, 1, 1, 1, 1, 0, false, false, false, false, false);
        applyMod("default");
    }

    // Update is called once per frame
    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerPos = player.position;
        //reduce stuck value and meter
        if(currentMod.automatic) {
            if(Input.GetButton("Fire1")) {
                fire();
            }
        }
        else {
            if(Input.GetButtonDown("Fire1")) {
                fire();
            }
        }
    }

    private void FixedUpdate() {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        //the following float values may need adjustment
        transform.position = new Vector2(playerPos.x + .4f, playerPos.y - .2f);
    }

    //define a set of pre-made gun modifications
    private void createMods() {
        modList.Add("default", new gunMod(20, 1, 10, 1, .5f, 1, false, false, false, false, false));
        modList.Add("machinegun", new gunMod(20, .7f, 100, .9f, .1f, 1, true, false, false, false, false));
        modList.Add("shotgun", new gunMod(18, 1, 5, 1.2f, 1, 8, false, true, false, false, false));
        modList.Add("burst", new gunMod(20, .9f, 7, .9f, .5f, 5, false, false, true, false, false));
        modList.Add("heavy", new gunMod(6, 5, 3, 3, 3, 1, false, false, false, true, true));
        modList.Add("ultimate", new gunMod(25, 3, 250, .8f, .01f, 1, true, true, false, false, false));
    }

    //main function to call
    public void fire() {
        if(timeSinceFired > currentMod.rate/* && stuck value < currentMod.stuckLimit && not in menus*/) {
            //increase stuck value and meter
            if(currentMod.spray) {
                for(int i = 0; i < currentMod.shots; i++) {
                    //launch(firePoint.rotation but offset randomly +- 25 degrees);
                }
            }
            else if(currentMod.burst) {
                StartCoroutine(burst());
            }
            else {
                launch();
            }
            timeSinceFired = 0;
        }
    }

    //create and throw gumballs
    public void launch() {
        launch(firePoint.rotation);
    }

    public void launch(Quaternion rotation) {
        GameObject gumball = Instantiate(Resources.Load("Gumball"), firePoint.position, rotation) as GameObject;
        //Apply velocity, scale, damage, gravity, splash
    }

    //overwrite currentMod as preset
    public void applyMod(string mod) {
        if(modList.ContainsKey(mod)) {
            currentMod.velocity = modList[mod].velocity * permanentMods.velocity;
            currentMod.damage = modList[mod].damage * permanentMods.damage;
            currentMod.stuckLimit = modList[mod].stuckLimit * permanentMods.stuckLimit;
            currentMod.scale = modList[mod].scale * permanentMods.scale;
            currentMod.rate = modList[mod].rate * permanentMods.rate;
            currentMod.shots = modList[mod].shots + permanentMods.shots;
            currentMod.automatic = modList[mod].automatic || permanentMods.automatic;
            currentMod.spray = modList[mod].spray || permanentMods.spray;
            currentMod.burst = modList[mod].burst || permanentMods.burst;
            currentMod.splash = modList[mod].splash || permanentMods.splash;
            currentMod.gravity = modList[mod].gravity || permanentMods.gravity;
        }
        else {
            Debug.LogError("Invalid mod: " + mod);
        }
    }

    //adjust individual attributes of currentMod
    public void alterMod(string attribute, float value, bool permanent) {
        gunMod reference;
        if(permanent) {
            reference = permanentMods;
        }
        else {
            reference = currentMod;
        }
        switch(attribute) {
            case "velocity":
                reference.velocity = value;
                break;
            case "damage":
                reference.damage = value;
                break;
            case "stuckLimit":
                reference.stuckLimit = value;
                break;
            case "scale":
                reference.scale = value;
                break;
            case "rate":
                reference.rate = value;
                break;
            default:
                Debug.LogError("Invalid attribute: " + attribute + "=" + value);
                break;
        }
    }

    public void alterMod(string attribute, int value, bool permanent) {
        gunMod reference;
        if(permanent) {
            reference = permanentMods;
        }
        else {
            reference = currentMod;
        }
        if(attribute == "shots") {
            reference.shots = value;
        }
        else {
            alterMod(attribute, (float)value, permanent);
        }
    }

    public void alterMod(string attribute, bool value, bool permanent) {
        gunMod reference;
        if(permanent) {
            reference = permanentMods;
        }
        else {
            reference = currentMod;
        }
        switch(attribute) {
            case "automatic":
                reference.automatic = value;
                break;
            case "spray":
                reference.spray = value;
                break;
            case "burst":
                reference.burst = value;
                break;
            case "splash":
                reference.splash = value;
                break;
            case "gravity":
                reference.gravity = value;
                break;
            default:
                Debug.LogError("Invalid attribute: " + attribute + "=" + value);
                break;
        }
    }

    //convert string into information
    private dynamic getAttribute(gunMod mod, string attribute) {
        switch(attribute) {
            case "velocity":
                return mod.velocity;
            case "damage":
                return mod.damage;
            case "stuckLimit":
                return mod.stuckLimit;
            case "scale":
                return mod.scale;
            case "rate":
                return mod.rate;
            case "shots":
                return mod.shots;
            case "automatic":
                return mod.automatic;
            case "spray":
                return mod.spray;
            case "burst":
                return mod.burst;
            case "splash":
                return mod.splash;
            case "gravity":
                return mod.gravity;
            default:
                return false;
        }
    }

    //called externally to adjust currentMod temporarily
    public void upgrade(string attribute, dynamic value, float duration) {
        dynamic original = getAttribute(currentMod, attribute);
        alterMod(attribute, value, false);
        StartCoroutine(upgradeTimer(attribute, original, duration));
    }

    //fire multiple shots with one input
    IEnumerator burst() {
        float startTime = Time.time;
        int shotCount = currentMod.shots;
        for(int i = 0; i < shotCount; i++) {
            while(Time.time - startTime < burstDelay) {
                yield return 0;
            }
            launch();
            startTime = Time.time;
        }
    }

    //waits for given time and reverts mod to previous attribute state
    IEnumerator upgradeTimer(string attribute, dynamic original, float duration) {
        float startTime = Time.time;
        while(Time.time - startTime < duration) {
            //update upgrade timer hud
            yield return 0;
        }
        alterMod(attribute, original, false);
    }
}

class gunMod
{
    /* GUIDE
     * velocity     how fast the gumball moves
     * damage       how much the enemy is affected (slowed, hurt)
     * stuckLimit   maximum firing rate before gun jams
     * scale        size of gumball
     * rate         time required between shots
     * shots        number of gumballs per shot (spray/burst only)
     * automatic    can the input be held down to fire continuously
     * spray        shotgun-style varied direction
     * burst        firing multiple times in the same direction with one input
     * splash       affects area instead of point on collision
     * gravity      does the gumball fall overtime
     */
    public float velocity, damage, stuckLimit, scale, rate;
    public int shots;
    public bool automatic, spray, burst, splash, gravity;
    public gunMod(float velocity, float damage, float stuckLimit, float scale, float rate, int shots, bool automatic, bool spray, bool burst, bool splash, bool gravity) {
        this.velocity = velocity;
        this.damage = damage;
        this.stuckLimit = stuckLimit;
        this.scale = scale;
        this.rate = rate;
        this.shots = shots;
        this.automatic = automatic;
        this.spray = spray;
        this.burst = burst;
        this.splash = splash;
        this.gravity = gravity;
    }
}
