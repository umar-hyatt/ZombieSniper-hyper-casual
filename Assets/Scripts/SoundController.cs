using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public AudioSource gamePlayBG, win, lose, zombie;
    public void ChangeMusicVolume(float volume)
    {
        gamePlayBG.volume=volume;
    }
    public void ChangeSoundVolume(float volume)
    {
        win.volume=volume;
        lose.volume=volume;
        zombie.volume=volume;
    }
    public void Vibration()
    {
        if (!GameManager.instance.isVibration) return;
        Handheld.Vibrate();
    }
}
