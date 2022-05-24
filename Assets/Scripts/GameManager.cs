using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool removeAds;
    public int age;
    public static GameManager instance;
    public enum GameDifficulty { easy, normal, hard }
    public GameDifficulty difficulty;
    public GunsData[] guns;
    public bool isVibration = true;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }
    public GameObject loadingPanel, tapToContinue;
    public Image loadingBar;
    public void LoadSceneAsync(string sceneName, bool tapWait)
    {
        StartCoroutine(LoadScene(sceneName, tapWait));
    }
    IEnumerator LoadScene(string sceneName, bool tapWait)
    {
        loadingPanel.SetActive(true);
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingBar.fillAmount = asyncOperation.progress / 0.9f;
            if (asyncOperation.progress >= 0.9f)
            {
                if (!tapWait)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                else
                {
                    tapToContinue.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                loadingPanel.SetActive(!asyncOperation.allowSceneActivation);
            }
            yield return null;
        }
    }
}
