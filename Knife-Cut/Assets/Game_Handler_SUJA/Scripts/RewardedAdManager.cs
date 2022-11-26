using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;
using System;
using UnityEngine.Events;


public class RewardedAdManager : MonoBehaviour
{

    private int rewIndex = 3;
    public RewardedAd[] rewardedAd;
    public string[] rewardedAdID; 
    [HideInInspector] public AdRequest[] requestRewarded;
    bool show_ad_as_index;
    private bool showAds;
    public UnityEvent onRewarded;
    public UnityEvent onClose;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TryToFetch");
    }
    IEnumerator TryToFetch()
    {
        yield return new WaitUntil(() => GoogleSheetHandler.googlesheetInitilized);
        CreateRewardAd();
    }
     void CreateRewardAd()
    {
        showAds = GoogleSheetHandler.showAds;
        rewardedAdID = new string[rewIndex];
        rewardedAdID[0] = GoogleSheetHandler.g_rewarded1;
        RequestRewardedAd(rewardedAdID[0], 0);
        rewardedAdID[1] = GoogleSheetHandler.g_rewarded2;
        RequestRewardedAd(rewardedAdID[1], 1);
        rewardedAdID[2] = GoogleSheetHandler.g_rewarded3;
        RequestRewardedAd(rewardedAdID[2], 2);
        show_ad_as_index = GoogleSheetHandler.show_ad_as_index;
    }
    public void RequestRewardedAd(string rewardedID, int rewIndex)
    {
        this.rewardedAd[rewIndex] = new RewardedAd(rewardedID);
        rewardedAd[rewIndex].OnAdLoaded += HandleRewardBasedVideoLoaded;

        //rewardedAd[rewIndex].OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        rewardedAd[rewIndex].OnUserEarnedReward += HandleRewardBasedVideoRewarded;

        rewardedAd[rewIndex].OnAdClosed += HandleRewardBasedVideoClosed;


        requestRewarded[rewIndex] = new AdRequest.Builder().Build();

        this.rewardedAd[rewIndex].LoadAd(requestRewarded[rewIndex]);

    }
    public int CurrentRewIndex()
    {
        if (GoogleSheetHandler.show_g_rewarded1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_rewarded2 == true)
        {
            return 1;
        }
        else
             if (GoogleSheetHandler.show_g_rewarded3 == true)
        {
            return 2;
        }
        else
            return 0;

    }
    public static int Rew_Index2
    {
        get { return PlayerPrefs.GetInt("Rew_Index2", 0); }
        set { PlayerPrefs.SetInt("Rew_Index2", value); }
    }
    IEnumerator WaitAplayRewardedAd()
    {
        if (show_ad_as_index == false)
        {
            while (!rewardedAd[CurrentRewIndex()].IsLoaded())
            {
                yield return null;
            }
            rewardedAd[CurrentRewIndex()].Show();
        }
        else
        {
            while (!rewardedAd[Rew_Index2].IsLoaded())
            {
                yield return null;
            }
            rewardedAd[Rew_Index2].Show();
            CheckRewIndex();
        }

    }
    void CheckRewIndex()
    {
        Rew_Index2++;
        if (Rew_Index2 >= Rew_Index2)
        {
            Rew_Index2 = 0;
        }
    }
    public void ShowRewardedAd()
    {
        if (!showAds)
            return;
        StartCoroutine(WaitAplayRewardedAd());

        //if (!rewardedAd.IsLoaded())
        //{
        //    unity_AdManager.ShowRewardedVideo();

        //}
        //else
        //{
        //    rewardedAd.Show();
        //}
    }
    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }   
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //  Debug.Log("Rewarded Video ad loaded successfully");

    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
        ////interstitialAd.Destroy();
        ////RequestFullScreenAd();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        

    }
    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        // Debug.Log ("Rewarded video has closed");
        //adManager.CallRewardedAdClosedEvent();
        RequestRewardedAd(rewardedAdID[CurrentRewIndex()], CurrentRewIndex());
        //rewardedAd[CurrentRewIndex()].LoadAd(requestRewarded[CurrentRewIndex()]);

    }

}
