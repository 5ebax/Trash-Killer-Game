using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    private int currentSceneIndex;
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        Shooting[] shootings = FindObjectsOfType<Shooting>();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPause)
            {
            Resume();
            foreach (Shooting scripts in shootings)
            {
            scripts.enabled = true;
            }
            } else{
            Pause();
            foreach (Shooting scripts in shootings)
            {
            scripts.enabled = false;
            }          
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
