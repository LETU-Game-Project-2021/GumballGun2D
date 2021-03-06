﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_JetForce = 5f;                             // Amount of force added while using the jetpack
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if(!wasGrounded) {
                    OnLandEvent.Invoke();
                    this.GetComponent<Player>().remainingJumps = this.GetComponent<Player>().totalJumps-1;
                }
            }
        }
    }


    public void Move(float move, bool jump, int jumps, bool fly, bool jetpack)
    {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        // If the player should jump...
        if ((m_Grounded||jumps>0) && jump && !jetpack)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            FindObjectOfType<SoundManager>().Play("Jump");
            m_Rigidbody2D.velocity = new Vector2(0, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            this.GetComponent<Player>().remainingJumps--;
        }
        if(jetpack && fly) {
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JetForce), ForceMode2D.Force);
        }
    }


    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Rotate the player and it's children
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        //Correct the rotation of the gun
        this.GetComponentInChildren<Weapon>().transform.localScale = Vector3.Scale(this.GetComponentInChildren<Weapon>().transform.localScale, new Vector3(-1, -1, 1));
    }
}