﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int portalHealth = 2;
    public bool gameOver = false;

    private void Update()
    {
        if (portalHealth <= 0) {
            gameOver = true;
			SceneManager.LoadScene("GameOver");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            portalHealth--;
            Destroy(collision.gameObject); 
        }
    }
}
