using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Yodo1.MAS;
using UnityEngine.UI;

public class InterstitialAtTimer : MonoBehaviour
{
    [Header("PlacementID (optional) ")]
    public string placementID;
    [Space(10)]
    [Header("Interstitial Ad Timer (Ad will show after specified minutes) ")]
    [Tooltip("Please add time in mins, Interstitial ad will keep showing after the time interval specified.")]
    public float ShowInterstitialAfterMins = 1;
    GameObject adbreak, panel;
    Button btn;

    [Space(10)]
    [Header("Interstitial AD Events (optional)")]
    [SerializeField] UnityEvent OnInterstitialAdOpened;
    [SerializeField] UnityEvent OnInterstitialAdClosed;
    [SerializeField] UnityEvent OnInterstitialAdError;

    private void Start()
    {
#if UNITY_EDITOR
        adbreak = (GameObject)Resources.Load("adbreakpanel");
#endif
        InvokeRepeating("ShowInterstitialAd", ShowInterstitialAfterMins * 60f, ShowInterstitialAfterMins * 60);
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

    void ShowInterstitialAd()
    {
#if UNITY_EDITOR
        panel = Instantiate(adbreak, GameObject.FindObjectOfType<Canvas>().transform);
        btn = panel.GetComponentInChildren<Button>();
        btn.interactable = false;
        btn.onClick.AddListener(OnButtonClick);
        panel.SetActive(true);
        StartCoroutine(Invokee(MakeButtonActive, 1f));
#else
        ShowIntertitial();
#endif
    }

    void MakeButtonActive()
    {
        btn.interactable = true;
    }

    void OnButtonClick()
    {
        ShowIntertitial();
    }

    void ShowIntertitial()
    {
#if UNITY_EDITOR
        Destroy(panel);
        adbreak.SetActive(false);
#endif

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
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad closed - timer");
        OnInterstitialAdClosed.Invoke();
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Interstitial ad error - " + adError.ToString());
        OnInterstitialAdError.Invoke();

    }
    public IEnumerator Invokee(System.Action action, float Delay)
    {
        yield return new WaitForSecondsRealtime(Delay);
        if (action != null)
            action();
    }
}
