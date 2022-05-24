using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yodo1.MAS;

[RequireComponent((typeof(Button)))]
public class InterstitialAtButton : MonoBehaviour
{
    [Header("PlacementID (optional) ")]
    public string placementID;
    [Space(10)]
    [Header("Interstitial AD Events (optional) ")]
    [SerializeField] UnityEvent OnInterstitialAdOpened;
    [SerializeField] UnityEvent OnInterstitialAdClosed;
    [SerializeField] UnityEvent OnInterstitialAdError;

    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        btn.onClick.AddListener(TaskOnClick);
    }

    private void OnEnable()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent += OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent += OnInterstitialAdErorEvent;
    }

    private void OnDisable()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent -= OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent -= OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent -= OnInterstitialAdErorEvent;
    }

    void TaskOnClick()
    {
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

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad opened");
        OnInterstitialAdOpened.Invoke();
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad closed - buttons");
        OnInterstitialAdClosed.Invoke();
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad error - " + adError.ToString());
        OnInterstitialAdError.Invoke();
    }

}
