using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; private void Awake() {if(instance==null)instance=this;}
    public GameObject gameOverPanel;
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
