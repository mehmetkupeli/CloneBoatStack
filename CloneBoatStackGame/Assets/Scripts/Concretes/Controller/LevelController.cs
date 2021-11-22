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
    public Slider levelProgressBar;
    
    //Finish Process
    public float maxDistance;
    public GameObject finishLine;

    //currentLevel
    int currentLevel;

    //Score
    int score;

    //Audio
    public AudioSource gameMusicAudioSource;
    public AudioClip victoryAudioClip, gameOverAudioClip;
    private void Start()
    {
        Current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel"); //First input getint=0
        if (SceneManager.GetActiveScene().name!="Level "+currentLevel)//Now Level name
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            currentLevelText.text = (currentLevel + 1).ToString();
            nextLevelText.text = (currentLevel + 2).ToString();
        }
        gameMusicAudioSource = Camera.main.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isGameActive)
        {
            PlayerController player = PlayerController.Current;
            float distance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
            levelProgressBar.value = 1 - (distance / maxDistance);
        }
    }

    public void StartLevel()
    {
        maxDistance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
        PlayerController.Current.ChangeSpeed(PlayerController.Current.runningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        PlayerController.Current.animator.SetBool("isRunning",true);
        isGameActive = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Loaded restart Active scene 
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level " + (currentLevel+1));
    }
    public void GameOver()
    {
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(gameOverAudioClip);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        isGameActive = false;
    }

    public void FinishGame()
    {
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(victoryAudioClip);
        PlayerPrefs.SetInt("currentLevel",currentLevel+1);
        finishScoreText.text = score.ToString();
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        PlayerController.Current.animator.SetBool("isRunning", false);
        PlayerController.Current.animator.SetBool("isWin", true);
        isGameActive = false;
    }

    public void ChangeScore(int increment)
    {
        score += increment;
        scoreText.text = score.ToString();

    }

}
