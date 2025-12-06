using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdManager 
{
    #region Banner Ad

    public void CreateBannerView() { }

    public void LoadBannerAd() { }

    public void DestroyBannerAd() { }

    #endregion



    #region Rewarded Ad

    public void LoadRewardedAd() { }

    public void ShowRewardedAd() { }

    #endregion    
}

public class NullAdManager : IAdManager
{
}