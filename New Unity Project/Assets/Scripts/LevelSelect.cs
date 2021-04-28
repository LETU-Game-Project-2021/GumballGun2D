using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
	private float oldTime = 1;
	public GameObject select1UI;
	// public GameObject select2UI;
	// public GameObject select3UI;
	// public GameObject select4UI;
	// public GameObject select5UI;

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void select1()
	{
		oldTime = Time.timeScale;
		select1UI.SetActive(true);
		Time.timeScale = 0f;
	}
	public void exit1()
	{
		select1UI.SetActive(false);
        Time.timeScale = oldTime;
	}
	
	public void goToLevel1()
	{
		select1UI.SetActive(false);
		Time.timeScale = 1f;
		SceneManager.LoadScene("Level 1");
	}
	
	
}
