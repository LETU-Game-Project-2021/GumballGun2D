using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Transform[] nests;
    public Transform backBarrier;
    public Transform newPortalLoc;
    public Transform[] spawnerLocations;
    public GameObject nestObject;
    public GameObject barrierObject;
    public GameObject waveObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {

            for (int i = 0; i < nests.Length; i++) {
                Instantiate(nestObject, new Vector3(nests[i].position.x, nests[i].position.y, 0), Quaternion.identity);
            }
            for (int i = 0; i < spawnerLocations.Length; i++) {
                Instantiate(waveObject, spawnerLocations[i].position, Quaternion.identity);
            }
            Instantiate(barrierObject, backBarrier.transform.position, Quaternion.identity);
            FindObjectOfType<Gamemanager>().roomComplete = false;
            if (collision.GetComponent<Player>().availableDrills != collision.GetComponent<Player>().totalDrills) {
                collision.GetComponent<Player>().availableDrills = collision.GetComponent<Player>().totalDrills;
            }
            FindObjectOfType<Portal>().gameObject.transform.position = newPortalLoc.position;
            Destroy(this.gameObject);
        }
    }
}
