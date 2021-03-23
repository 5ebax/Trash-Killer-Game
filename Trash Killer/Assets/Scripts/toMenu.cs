using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool GameIsPause = false;

    public void update(){
        Shooting[] shootings = FindObjectsOfType<Shooting>();
            foreach (Shooting scripts in shootings)
            {
            scripts.enabled = true;
            }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

}
