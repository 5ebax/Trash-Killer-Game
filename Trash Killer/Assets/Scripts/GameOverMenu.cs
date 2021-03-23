using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverMenu : MonoBehaviour
{
    private int currentSceneIndex;
    public GameObject gameOver;

    private void Update()
    {
        if (PlayerController.muerte)
        {
            gameOver.SetActive(true);
        }
    }

    public void EndGame(){
        Debug.Log("Game Over");
    }

    public void LoadMenu(){

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
        SceneManager.LoadScene(0);
    }

    public void Reset(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
