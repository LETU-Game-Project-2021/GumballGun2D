using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            if (collision.gameObject.GetComponent<Player>().nest.gameObject.GetComponent<Nest>().destroyed) {
                collision.gameObject.GetComponent<Player>().drillPickup = true;
            }
            if (collision.gameObject.GetComponent<Player>().destroyDrill == true) {
                Destroy(this.gameObject);
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

}
