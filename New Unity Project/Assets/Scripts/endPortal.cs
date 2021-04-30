using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endPortal : MonoBehaviour
{
    public LevelComplete level;

    void Start()
    {
        level = FindObjectOfType<LevelComplete>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().inGameWarp = true;
            collision.GetComponent<Player>().warpPortal = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().inGameWarp = false;

        }
    }

    public void Warp() {
        level.GameComplete();
    }
}
