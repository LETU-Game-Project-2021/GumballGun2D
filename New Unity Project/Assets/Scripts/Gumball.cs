using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumball : MonoBehaviour
{
    public float sprayRotationLimit = 25;

    public Rigidbody2D rb;

    private float velocity, damage;
    private bool splash, gravity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(gravity) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-Physics2D.gravity.magnitude * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag != "Player" && collision.tag != "Gum" && collision.tag != "Weapon") {
            Destroy(this.gameObject);
        }
        if(collision.tag == "Enemy") {
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
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
}
