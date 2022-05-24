using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; private void Awake() { if (instance == null) instance = this; }
    public GameObject gameOverPanel;
    public string homeSceneName;
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void GamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void HomeButton()
    {
        GameManager.instance.LoadSceneAsync(homeSceneName,false);
    }
    public void RestartButton()
    {
        GameManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name,true);
    }
}
