using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-8f, 8f), Random.Range(10f, 30f)) * 10);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.gameObject.GetComponent<Player>().coins++;
            FindObjectOfType<SoundManager>().Play("Coin Pickup");
            Destroy(this.gameObject);
        }
    }
}
