using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    private void Start() {
        transform.GetComponentInChildren<ParticleSystem>().Play();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            if (collision.gameObject.GetComponent<Player>().nest.gameObject.GetComponent<Nest>().destroyed) {
                collision.gameObject.GetComponent<Player>().drillPickup = true;
                collision.gameObject.GetComponent<Player>().currentDrill = this.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().nest.gameObject.GetComponent<Nest>().destroyed)
            {
                collision.gameObject.GetComponent<Player>().drillPickup = false;
            }
        }
    }

    public IEnumerator displayTimer(float duration) {
        GameObject hud = GameObject.Find("WorldCanvas");
        Image bar = (Instantiate(Resources.Load("DrillTimerBar"), transform.position, Quaternion.identity) as GameObject).GetComponent<Image>();
        bar.rectTransform.SetParent(hud.transform);
        float startTime = Time.time;
        float width = bar.rectTransform.sizeDelta.x * hud.transform.localScale.x;
        float height = bar.rectTransform.sizeDelta.y * hud.transform.localScale.y;
        bar.transform.position = bar.transform.position - new Vector3(/*bar.transform.localScale.x * hud.transform.localScale.x*/0, bar.transform.localScale.y * hud.transform.localScale.y / -2f, 0);
        while(Time.time - startTime < duration) {
            bar.rectTransform.sizeDelta = new Vector2(width * (duration - (Time.time-startTime)) / duration, height);
            yield return 0;
        }
        Destroy(bar.gameObject);
        transform.GetComponentInChildren<ParticleSystem>().Stop();
    }
}
