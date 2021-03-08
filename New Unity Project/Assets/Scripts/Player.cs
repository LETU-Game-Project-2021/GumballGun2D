using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    public bool drill = false;
    public bool upgrade = false;
    public GameObject nest;
    public GameObject drillObject;
    public Upgrader upgrader;
    public int avalibleDrills = 1;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {

            jump = true;

        }

        if(Input.GetButtonDown("Use"))
        {
            playerUse();

        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void playerUse() {

        if(drill && avalibleDrills > 0) {
            Instantiate(drillObject, nest.transform.position, Quaternion.identity);
            avalibleDrills--;
            nest.gameObject.GetComponent<Nest>().startDrill();
        } 
        else if(upgrade/* && sufficient coins*/) {
            upgrader.getTemporaryEnhancement();
        }
        //else if machine {
        //      interact (UI?)
        //}
    }
}
