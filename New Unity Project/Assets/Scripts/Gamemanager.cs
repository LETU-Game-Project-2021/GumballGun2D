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
    public GameObject barrier;
	public LevelComplete completed;
    public int roomNumber = 0;
    public Transform position1;
    public Transform position2;

    private void Start()
    {
        portalLocation = this.gameObject.GetComponentInChildren<CapsuleCollider2D>().gameObject;
        portal = this.gameObject.GetComponentInChildren<Portal>().gameObject;
        roomNumber = 0;
    }

    void Update()
    {
        if (totalNests <= nestsDestroyed && !roomComplete) {
            roomComplete = true;
            completeRoom();
        }

    }

    public void completeRoom() {
        //stop waves and move to the next room
        nestsDestroyed = 0;
        totalNests = 0;
        roomNumber++;
        Debug.Log("Nests Destroyed!");
        switch (roomNumber) {
            case 1:
                barrier.transform.position = position1.position;
                break;
            case 2:
                barrier.transform.position = position2.position;
                break;
        }

		//completed.GameComplete();
		
    }
}
