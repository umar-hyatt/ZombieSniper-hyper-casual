using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Yodo1.MAS;
using UnityEngine.UI;

public class InterstitialAtBreaks : MonoBehaviour
{
    [Header("PlacementID (optional) ")]
    public string placementID;

    [Space(10)]
    [Header("Interstitial AD Events (optional) ")]
    [SerializeField] UnityEvent OnInterstitialAdOpened;
    [SerializeField] UnityEvent OnInterstitialAdClosed;
    [SerializeField] UnityEvent OnInterstitialAdError;

    private void OnEnable()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent += OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent += OnInterstitialAdErorEvent;

        if (Yodo1U3dMas.IsInterstitialAdLoaded())
        {
            if (string.IsNullOrEmpty(placementID))
            {
                Yodo1U3dMas.ShowInterstitialAd();
            }
            else
            {
                Yodo1U3dMas.ShowInterstitialAd(placementID);
            }
        }
        else
        {
            Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad has not been cached.");
        }
    }


    private void OnDisable()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent -= OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent -= OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent -= OnInterstitialAdErorEvent;
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad opened");
        OnInterstitialAdOpened.Invoke();
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad closed - breaks");
        OnInterstitialAdClosed.Invoke();
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad error - " + adError.ToString());
        OnInterstitialAdError.Invoke();
    }


}
