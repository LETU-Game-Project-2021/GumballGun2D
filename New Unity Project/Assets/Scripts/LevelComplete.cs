using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public GameObject completeLevel;
	
	private void Start()
	{
		completeLevel.SetActive(false);
	}
	
	public void GameComplete()
	{
        Time.timeScale = 0f;
		completeLevel.gameObject.SetActive(true);
	}
	
	public void NextLevel()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
}
