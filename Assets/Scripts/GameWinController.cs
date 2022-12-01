using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinController : MonoBehaviour
{
    public static GameWinController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CheckRemainingZombie()
    {
        if (transform.childCount < 1)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
print("GameWin");
    }
}
