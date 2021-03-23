using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool boy;
    public bool girl;
    private int sceneToContinue;
    private string continuePlay;

    void Start(){
        boy = false;
        girl = false;
        continuePlay = "True";
    }
    public void PlayGame(){
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BoySelected()
    {
        boy = true;
        if (boy == true)
        {
            PlayerPrefs.SetInt("Selected", 0);

        }
    }
    public void GirlSelected()
    {
        girl = true;
        if(girl == true){
            PlayerPrefs.SetInt("Selected", 0);
        }
    }
    public void CitySelected(){
         SceneManager.LoadScene(1);
    }

    public void ForestSelected(){
         SceneManager.LoadScene(2);
    }

    public void BeachSelected(){
         SceneManager.LoadScene(3);
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetString("continue", continuePlay);
        sceneToContinue = PlayerPrefs.GetInt("SavedScene");
        if(sceneToContinue !=0){
            SceneManager.LoadScene(sceneToContinue);
        } else
        {
            return;
        }
    }
}
