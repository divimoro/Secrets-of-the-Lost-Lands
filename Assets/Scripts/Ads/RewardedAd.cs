using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
public class RewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] private string androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string iosAdUnitId = "Rewarded_iOS";

    private string _adUnitId;
    private AdTv _adTv;
    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosAdUnitId
            : androidAdUnitId;
    }

   /* private void Start()
    {
        LoadAd();
    }*/
    
    private void Start()
    {
        StartCoroutine(LoadAdRewarded());
    }

    private IEnumerator LoadAdRewarded()
    {
        yield return new WaitForSeconds(1f);
        LoadAd();
    }
    
    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            //GameLogic.S.IncrementPoint2AfterAds(1);
            _adTv = GetComponent<AdTv>();
            _adTv.DropItems();
            
            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
