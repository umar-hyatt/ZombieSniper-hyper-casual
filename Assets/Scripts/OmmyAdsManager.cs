using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmmyAdsManager : MonoBehaviour
{
    public void ShowIntentitial()
    {
        if(GameManager.instance.removeAds)return;
        
    }
    public void ShowRewarded()
    {
        if(GameManager.instance.removeAds)return;

    }
    public void ShowBanner()
    {
        if(GameManager.instance.removeAds)return;

    }
}
