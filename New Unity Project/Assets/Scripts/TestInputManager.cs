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
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Heavy Hose");
            gun.applyMod("heavyHose");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("+10 coins");
            player.changeCoin(10);
        }*/
    }
}
