using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon: MonoBehaviour
{
    public gunMod currentMod, permanentMods;
    private Dictionary<string, gunMod> modList;
    private float timeSinceFired = 0;
    private float burstDelay = 0.1f;
    public Rigidbody2D player;

    // Start is called before the first frame update
    void Start() {
        createMods();
        currentMod = modList["default"];
        permanentMods = new gunMod(1, 1, 1, 1, 1, 0, false, false, false, false, false);
    }

    // Update is called once per frame
    void Update() {
        //set location (or parent)
        //rotate sprite to point at mouse
        //reduce stuck value and meter
        if(currentMod.automatic) {
            if(Input.GetButton("Fire1")) {
                //move this to player class
                //fire();
            }
        }
        else {
            if(Input.GetButtonDown("Fire1")) {
                //fire();
            }
        }
    }

    private void createMods() {
        modList.Add("default", new gunMod(100, 1, 10, 1, .5f, 1, false, false, false, false, false));
        modList.Add("machinegun", new gunMod(100, .7f, 100, .9f, .1f, 1, true, false, false, false, false));
        modList.Add("shotgun", new gunMod(90, 1, 5, 1.2f, 1, 8, false, true, false, false, false));
        modList.Add("burst", new gunMod(100, .9f, 7, .9f, .5f, 5, false, false, true, false, false));
        modList.Add("heavy", new gunMod(30, 5, 3, 3, 3, 1, false, false, false, true, true));
    }

    public void fire() {
        if(timeSinceFired > currentMod.rate/* && stuck value < currentMod.stuckLimit*/) {
            //increase stuck value and meter
            if(currentMod.spray) {
                for(int i = 0; i < currentMod.shots; i++) {
                    //launch(randomly offset direction);
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

    public void launch() {
        //launch(default direction);
    }

    public void launch(Vector2 direction) {
        //Instantiate
        //Apply velocity, scale, damage, gravity, splash
    }

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

    public void alterMod(string attribute, float value) {
        switch(attribute) {
            case "velocity":
                currentMod.velocity = value;
                break;
            case "damage":
                currentMod.damage = value;
                break;
            case "stuckLimit":
                currentMod.stuckLimit = value;
                break;
            case "scale":
                currentMod.scale = value;
                break;
            case "rate":
                currentMod.rate = value;
                break;
            default:
                Debug.LogError("Invalid attribute: " + attribute + "=" + value);
                break;
        }
    }

    public void alterMod(string attribute, int value) {
        if(attribute == "shots") {
            currentMod.shots = value;
        }
        else {
            alterMod(attribute, (float)value);
        }
    }

    public void alterMod(string attribute, bool value) {
        switch(attribute) {
            case "automatic":
                currentMod.automatic = value;
                break;
            case "spray":
                currentMod.spray = value;
                break;
            case "burst":
                currentMod.burst = value;
                break;
            case "splash":
                currentMod.splash = value;
                break;
            case "gravity":
                currentMod.gravity = value;
                break;
            default:
                Debug.LogError("Invalid attribute: " + attribute + "=" + value);
                break;
        }
    }

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
}

public class gunMod
{
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
