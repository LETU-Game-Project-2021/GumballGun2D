using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public string levelName;
    //Level control
    void OnTrigerEnter2D(Collider2D other){
        //Changing level by scene name
        if (other.CompareTag("Player")){
            SceneManager.LoadScene(levelName);
        }
    }
}
