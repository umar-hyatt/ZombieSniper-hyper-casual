using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yodo1.MAS;

[RequireComponent((typeof(Button)))]
public class RewardedAd : MonoBehaviour
{
    Button rvBtn;
    [Header("PlacementID (optional) ")]
    [Tooltip("Enter your Rewarded Ad placement ID. Leave empty if you do not have one.")]
    public string placementID;
    [Space(10)]
    [Header("Rewarded AD Events")]
    [SerializeField] UnityEvent OnRewardedAdOpened;
    [SerializeField] UnityEvent OnRewardedAdClosed;
    [Header("Award User Here")]
    [SerializeField] UnityEvent OnAdReceivedReward;
    [SerializeField] UnityEvent OnRewardedAdError;

    private void Awake()
    {
        rvBtn = GetComponent<Button>();
    }

    private void Start()
    {
        rvBtn.onClick.AddListener(TaskOnClick);
    }

    private void OnEnable()
    {
        Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;
    }

    private void OnDisable()
    {
        Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent -= OnRewardedAdOpenedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent -= OnRewardedAdClosedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent -= OnRewardedAdErorEvent;
    }

    void TaskOnClick()
    {
        if (Yodo1U3dMas.IsRewardedAdLoaded())
        {
            if (string.IsNullOrEmpty(placementID))
            {
                Yodo1U3dMas.ShowRewardedAd();
            }
            else
            {
                Yodo1U3dMas.ShowRewardedAd(placementID);
            }
        }
        else
        {
            Debug.Log(Yodo1U3dMas.TAG + "NoCode Reward video ad has not been cached.");
        }
    }

    private void OnRewardedAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Rewarded ad opened");
        OnRewardedAdOpened.Invoke();
    }

    private void OnRewardedAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Rewarded ad closed");
        OnRewardedAdClosed.Invoke();
    }

    private void OnAdReceivedRewardEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Rewarded ad received reward");
        OnAdReceivedReward.Invoke();
    }

    private void OnRewardedAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "NoCode Rewarded ad error - " + adError.ToString());
        OnRewardedAdError.Invoke();
    }



}
