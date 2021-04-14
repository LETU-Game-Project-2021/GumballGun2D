using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Transform[] nests;
    public Transform backBarrier;
    public Transform[] spawnerLocations;
    public GameObject nestObject;
    public GameObject barrierObject;
    public GameObject waveObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {

            for (int i = 0; i < nests.Length; i++) {
                Instantiate(nestObject, nests[i].position, Quaternion.identity);
            }
            for (int i = 0; i < spawnerLocations.Length; i++) {
                Instantiate(waveObject, spawnerLocations[i].position, Quaternion.identity);
            }
            Instantiate(barrierObject, backBarrier, true);
            Destroy(this.gameObject);
        }
    }
}
