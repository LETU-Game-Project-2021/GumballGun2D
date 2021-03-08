using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public int totalNests = 0;
    public int nestsDestroyed = 0;
    public bool roomComplete = false;

    void Update()
    {
        if (Input.GetKeyDown("escape")) {
            Debug.Log("Quit");
            Application.Quit();
        }

        if (Input.GetKeyDown("t")) {
            nestsDestroyed = 0;
            totalNests = 0;
            SceneManager.LoadScene("SampleScene");
        }

        if (totalNests <= nestsDestroyed && !roomComplete) {
            roomComplete = true;
            completeRoom();
        }
    }

    public void completeRoom() {
        //stop waves and move to the next room
        nestsDestroyed = 0;
        totalNests = 0;
        Debug.Log("Nests Destroyed!");
    }
}
