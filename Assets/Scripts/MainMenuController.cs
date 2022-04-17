using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("GamePlay Scene Name")]
    public string gamePlaySceneName;
    public string policyLink, rateUsLink;
    public void OnPlayButton()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }
    public void OnShareButton()
    {
    }
    public void RemoveAds()
    {

    }
    public void OnPrivacyButton()
    {
        Application.OpenURL(policyLink);
    }
    public void OnRateUsButton()
    {
        Application.OpenURL(rateUsLink);
    }
    public void OnExitButton(bool yes)
    {
        if (yes)
        {
            Application.Quit();
        }
    }
    public Slider soundSlider, musicSlider;
    public void OnSoundChange(bool increment)
    {
        if (increment) { soundSlider.value += 1; return;}
        soundSlider.value -= 1;
    }
    public void OnMusicChange(bool increment)
    {
        if (increment) { musicSlider.value += 1; return;}
        musicSlider.value -= 1;
    }
}
