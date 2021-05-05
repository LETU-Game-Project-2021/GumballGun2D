using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private float oldTime = 1f;
    public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!GameIsPaused)
			{
				
                Pause();
            }
			else if(pauseMenuUI.activeSelf)
			{
                Resume();
            }
		}
    }
	
	public void Resume()
	{
		pauseMenuUI.SetActive(false);
        Time.timeScale = oldTime;
		GameIsPaused = false;
	}
	
	void Pause()
	{
        oldTime = Time.timeScale;
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}
	
	public void MenuReturn()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
		SceneManager.LoadScene(0);
		Debug.Log("Back to Menu");
	}
	
}