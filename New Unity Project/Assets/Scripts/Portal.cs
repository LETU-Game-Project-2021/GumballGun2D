using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public int portalHealth = 2;
    public bool gameOver = false;

    private float maxHealth;
    private Image healthMeter;
    private Vector2 healthBarSize;

    private void Start() {
        maxHealth = portalHealth;
        healthMeter = GameObject.Find("PortalMeterInner").GetComponent<Image>();
        healthBarSize = healthMeter.rectTransform.sizeDelta;
    }

    private void Update()
    {
        if (portalHealth <= 0) {
            gameOver = true;
			SceneManager.LoadScene("GameOver");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !collision.GetComponent<Enemy>().stuck) {
            portalHealth--;
            Destroy(collision.gameObject);
            float width = portalHealth / maxHealth * healthBarSize.x;
            healthMeter.rectTransform.sizeDelta = new Vector2(width, healthBarSize.y);
            FindObjectOfType<SoundManager>().Play("Portal");
        }
    }
}
