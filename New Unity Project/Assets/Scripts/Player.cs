using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

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
        //check dedicated collider for nearby nest (prioritized) or gumball machine
        //if nest {
        //      use drill (instantiate drill, start timer, disable and retexture nest
        //}
        //else if machine {
        //      interact (UI?)
        //}
    }
}
