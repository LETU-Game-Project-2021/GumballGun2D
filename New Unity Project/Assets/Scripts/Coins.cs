using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    public Text coinCounter;
    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-8f, 8f), Random.Range(10f, 30f)) * 10);
        coinCounter = GameObject.Find("CoinCount").GetComponent<Text>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.gameObject.GetComponent<Player>().coins++;
            coinCounter.text = collision.gameObject.GetComponent<Player>().coins.ToString();
            FindObjectOfType<SoundManager>().Play("Coin Pickup");
            Destroy(this.gameObject);
        }
    }
}
