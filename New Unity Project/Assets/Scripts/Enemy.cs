using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int eHealth;
    [SerializeField] public int enemyType;
    [SerializeField] public float speed;
    [SerializeField] public Transform wallCheck;
    [SerializeField] private LayerMask m_WhatIsGround;

    public bool stuck = false;
    public bool fly = false;
    public GameObject target;
    private GameObject pTarget;
    public bool splat = false;
    public float timeStuck;
    public float timeDestroy;
    public float wanderTime;
    private float wanderStartTime;
    private bool wander;
    private bool m_FacingRight = true;
    public float stillTime;
    private float stillCurrTime;
    public float attackStrength;
    public float timeBetweenAttacks;
    private float timeSinceAttack = 65535;

    const float k_GroundedRadius = .2f;

    void Start()
    {
        switch (enemyType) {
            case 0:
                eHealth = 1;
                speed = 2;
                break;
            case 1:
                eHealth = 2;
                speed = 1;
                break;
            case 2:
                eHealth = 1;
                fly = true;
                break;
        }
    }

    void FixedUpdate()
    {
        timeSinceAttack += Time.deltaTime;
        if (wander)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), -speed * Time.deltaTime);
            wanderStartTime += Time.deltaTime;
            if (wanderStartTime > wanderTime) {
                wanderStartTime = 0;
                Flip();
                wander = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                stillCurrTime += Time.deltaTime;
                if (stillCurrTime > stillTime) {
                    stillCurrTime = 0;
                    wander = true;
                    Flip();
                }
            }
        }

        if (stuck) {
            timeStuck += Time.deltaTime;
            if (timeStuck > timeDestroy) {
                Destroy(this.gameObject);
            }
        }
    }

    public void Damage() {
        eHealth--;
        if (eHealth <= 0) {
            stuck = true;
            Stuck();
        }
    }

    public void Stuck() {
        speed = 0;
        attackStrength = 0;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        //this.gameObject.layer = (8); // Ground
        this.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.magenta);
    }

    private void Flip()
    {
        if (stuck) {
            return;
        }
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Attack(GameObject player) {
        if(!stuck) {
            player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - gameObject.GetComponent<CircleCollider2D>().transform.position) * attackStrength + new Vector3(0, 2, 0), ForceMode2D.Impulse);
            player.GetComponent<Player>().stun();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            //pTarget = target;
            //target = collision.gameObject;
            if(timeSinceAttack > timeBetweenAttacks) {
                Attack(collision.gameObject);
                timeSinceAttack = 0;
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        target = pTarget;
    //    }
    //}
}
