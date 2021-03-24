using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool fly = false;
    public bool drill = false;
    public bool upgrade = false;
    public GameObject nest;
    public GameObject drillObject;
    public GameObject currentDrill;
    public Upgrader upgrader;
    public int availableDrills = 1;
    public bool doubleJump = false;
    public int totalJumps = 1;
    public int remainingJumps = 1;
    public bool jetpack = false;
    public bool drillPickup = false;
    public int coins;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        //if (Input.GetButton("Jump"))
        {
            jump = true;

        }

        if(Input.GetButton("Jump")) {
            fly = true;
        }

        if(Input.GetButtonDown("Use"))
        {
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

        if (drill && availableDrills > 0)
        {
            Instantiate(drillObject, nest.transform.position, Quaternion.identity);
            FindObjectOfType<SoundManager>().Play("Drill Start");
            availableDrills--;
            nest.gameObject.GetComponent<Nest>().startDrill();
        }
        else if (upgrade/* && sufficient coins*/)
        {
            if(!upgrader.tempActive) {
                upgrader.getTemporaryEnhancement();
                FindObjectOfType<SoundManager>().Play("Gumball Machine");
            }
        }
        else if (drillPickup) {

            availableDrills++;
            Destroy(currentDrill);
            drillPickup = false;

        }
    }
}
