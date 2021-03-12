using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public int totalNests = 0;
    public int nestsDestroyed = 0;
    public bool roomComplete = false;
    public GameObject portalLocation;
    public GameObject portal;

    private void Start()
    {
        portalLocation = this.gameObject.GetComponentInChildren<CapsuleCollider2D>().gameObject;
        portal = this.gameObject.GetComponentInChildren<Portal>().gameObject;
    }

    void Update()
    {

        if (Input.GetKeyDown("t")) {
            nestsDestroyed = 0;
            totalNests = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (totalNests <= nestsDestroyed && !roomComplete) {
            roomComplete = true;
            completeRoom();
        }

        if (portal.GetComponent<Portal>().gameOver) {
            Debug.Log("GameOver");
        }
    }

    public void completeRoom() {
        //stop waves and move to the next room
        nestsDestroyed = 0;
        totalNests = 0;
        Debug.Log("Nests Destroyed!");
    }
}
