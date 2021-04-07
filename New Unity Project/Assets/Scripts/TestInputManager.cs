using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputManager : MonoBehaviour
{
    Weapon gun;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("GumballGun").GetComponent<Weapon>();
        player = GameObject.FindObjectOfType<Player>();
        if(!player) {
            Debug.Log("Could not find player");
        }
        //Debug.Log("Test keys: 1.Default, 2.Automatic, 3.Spray, 4.Ultimate, 5.Burst, 6.Heavy, 7.Splash, 8.Jetpack, 9.Gravity");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Heavy Hose");
            gun.applyMod("heavyHose");
        }
        /*if(Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Default");
            gun.applyMod("default");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("Automatic");
            gun.applyMod("machinegun");
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            Debug.Log("Spray");
            gun.applyMod("shotgun");
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            Debug.Log("Ultimate");
            gun.applyMod("ultimate");
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)) {
            Debug.Log("Burst");
            gun.applyMod("burst");
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)) {
            Debug.Log("Heavy");
            gun.applyMod("heavy");
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)) {
            Debug.Log("Splash");
            gun.alterMod("splash", true, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)) {
            bool temp = !player.jetpack;
            Debug.Log("Jetpack = " + temp);
            player.jetpack = temp;
        }
        if(Input.GetKeyDown(KeyCode.Alpha9)) {
            bool temp = !gun.currentMod.gravity;
            Debug.Log("Gravity = " + temp);
            gun.alterMod("gravity", temp, true);
        }*/
    }
}
