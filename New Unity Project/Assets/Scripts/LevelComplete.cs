﻿using System.Collections;
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
		
		completeLevel.gameObject.SetActive(true);
	}
	
	public void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
}
