using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool isGameActive = false; //Game is active or deactive 

    //GameUI
    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText, currentLevelText, nextLevelText;



    private void Start()
    {
        Current = this;
        int currentLevel = PlayerPrefs.GetInt("currentLevel"); //First input getint=0
        if (SceneManager.GetActiveScene().name!="Level "+currentLevel)//Now Level name
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            currentLevelText.text = (currentLevel + 1).ToString();
            nextLevelText.text = (currentLevel + 2).ToString();
        }
    }

    public void StartLevel()
    {
        PlayerController.Current.ChangeSpeed(PlayerController.Current.runningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        isGameActive = true;
    }
}
