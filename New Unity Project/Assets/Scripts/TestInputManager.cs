using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputManager : MonoBehaviour
{
    Weapon gun;
    Player player;
    KeyCode[] cheat = new KeyCode[] {KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return};
    private int index = 0;
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
        if(Input.anyKeyDown) {
            if(Input.GetKeyDown(cheat[index])) {
                index++;
            }
            else {
                index = 0;
            }
            if(index == cheat.Length) {
                index = 0;
                gun.applyMod("ultimate");
                player.jetpack = true;
                player.availableDrills = player.totalDrills = 3;
                player.changeCoin(50);
                SoundManager.instance.Play("Coin Pickup");
            }
        }
    }
}
