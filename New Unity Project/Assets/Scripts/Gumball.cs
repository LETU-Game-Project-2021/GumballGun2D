using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumball : MonoBehaviour
{
    public float sprayRotationLimit = 25;
    public float splashCount = 15;

    public Rigidbody2D rb;

    private float velocity, damage;
    private bool splash, gravity;

    private float timeToLive;

    // Start is called before the first frame update
    void Start()
    {
        timeToLive = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(gravity) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-Physics2D.gravity.magnitude * Time.deltaTime);
        }
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag != "Player" && collision.tag != "Gum" && collision.tag != "Weapon") {
            if(splash) {
                createSplash(transform.position);
            }

            Destroy(gameObject);
        }
        if(collision.tag == "Enemy") {
            //collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collision.GetComponent<Enemy>().Damage();
            //set speed of "Spider" to 0
            //create splat
        }
    }

    //attributes
    public void spray(bool s) {
        if(s) {
            transform.Rotate(0, 0, Random.Range(-sprayRotationLimit, sprayRotationLimit));
        }
        rb.velocity = transform.right * velocity;
    }
    public void setVelocity(float v) {
        velocity = v;
    }

    public void setDamage(float d) {
        damage = d;
    }

    public void setSplash(bool s) {
        splash = s;
    }

    public void setGravity(bool g) {
        gravity = g;
    }

    //generate gumballs as splash effect
    private void createSplash(Vector3 location) {
        for(int i = 0; i < splashCount; i++) {
            Quaternion rotation = Quaternion.Euler(0,0,360/splashCount*i);
            GameObject ball = Instantiate(Resources.Load("Gumball"), location, rotation) as GameObject;
            Gumball g = ball.GetComponent<Gumball>();
            g.setVelocity(3);
            g.setDamage(.5f);
            g.setSplash(false);
            g.setGravity(true);
            g.spray(false);
        }
    }
}
/*
GameObject gumball = Instantiate(Resources.Load("Gumball"), firePoint.position, rotation) as GameObject;
gumball.transform.localScale *= currentMod.scale;
Gumball g = gumball.GetComponent<Gumball>();
        g.setVelocity(currentMod.velocity);
        g.setDamage(currentMod.damage);
        g.setSplash(currentMod.splash);
        g.setGravity(currentMod.gravity);
        g.spray(currentMod.spray);
*/
