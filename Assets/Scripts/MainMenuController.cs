using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject loadingPnl,tapContinue;
    public Image loadingBar;
    [Header("GamePlay Scene Name")]
    public string gamePlaySceneName;
    public string policyLink, rateUsLink;

    public void OnPlayButton()
    {
        GameManager.instance.LoadSceneAsync(gamePlaySceneName,true);
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
        if (increment) { soundSlider.value += .1f; return; }
        soundSlider.value -= 1;
    }
    public void OnMusicChange(bool increment)
    {
        if (increment) { musicSlider.value += .1f; return; }
        musicSlider.value -= 1;
    }
    public void VibrationOnOff(Sprite off)
    {
        SoundController.instance.Vibration();
        if (GameManager.instance.isVibration)
        {
            vibrationImage.sprite = off;
            GameManager.instance.isVibration = false;
        }
        else
        {
            vibrationImage.sprite = vibrationSprite;
            GameManager.instance.isVibration = true;
        }
    }
    public Image musicImage, soundImage, vibrationImage;
    public Sprite musicSprite, soundSprite, vibrationSprite;
    public void OnMusiceValueChange(Sprite Off)
    {
        SoundController.instance.ChangeMusicVolume(musicSlider.value);
        if (musicSlider.value == 0)
        {
            musicImage.sprite = Off;
        }
        else
        {
            musicImage.sprite = musicSprite;
        }
    }
    public void OnSoundValueChange(Sprite Off)
    {
        SoundController.instance.ChangeSoundVolume(soundSlider.value);
        if (soundSlider.value == 0)
        {
            soundImage.sprite = Off;
        }
        else
        {
            soundImage.sprite = soundSprite;
        }
    }
}
