using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
	private float oldTime = 1f;
	private float roomID;
	public GameObject select1UI;

    private void Start() {
        StartCoroutine(setDestination());
    }
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			collision.GetComponent<Player>().canWarp = true;
			collision.GetComponent<Player>().warpPortal = this.gameObject;
			//Debug.Log("Hi1");
		}
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			collision.GetComponent<Player>().canWarp = false;
			
		}
	}

    public void select1()
	{
		oldTime = Time.timeScale;
		select1UI.SetActive(true);
		Time.timeScale = 0f;
		//Debug.Log("Hi");
	}
	public void exit1()
	{
		oldTime = 1f;
		select1UI.SetActive(false);
        Time.timeScale = oldTime;
	}
	
	public void goToLevel1()
	{
		select1UI.SetActive(false);
		Time.timeScale = 1f;
		switch(roomID)
		{
			case 0: SceneManager.LoadScene(2);
				break;
			case 1: SceneManager.LoadScene(3);
				break;
			case 2: SceneManager.LoadScene(4);
				break;
		}
		// SceneManager.LoadScene("Level 1");
	}

    IEnumerator setDestination() {
        for(int i = 0; i < 1; i++) {
            yield return 0;
        }
        for(int i = 0; i < GameObject.FindObjectsOfType<LevelSelect>().Length; i++) {
            if(GameObject.FindObjectsOfType<LevelSelect>()[i] == this) {
                roomID = i;
            }
        }
    }
}
