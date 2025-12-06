using UnityEngine;
using GoogleMobileAds.Api;
using NetworkService.Manager;
using GooglePlayGames.BasicApi;

public class AdManager : IAdManager
{
#if UNITY_ANDROID
    private string _bannerAdUnitId = "ca-app-pub-3196408244005495/8792796991";
    private string _rewardedAdUnitId = "ca-app-pub-3196408244005495/2939233881";
#elif UNITY_IPHONE
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
    private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _bannerAdUnitId = "unused";
    private string _rewardedAdUnitId = "unused";
#endif

    private BannerView _bannerView;
    private RewardedAd _rewardedAd;
    private bool _isBannerLoaded = false;

    public AdManager()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob Initialized");
        });

        LoadRewardedAd();
    }

    #region Banner Ad

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyBannerAd();
        }

        int bannerWidth = 320;
        int bannerHeight = 100;

        AdSize adSize = new AdSize(bannerWidth, bannerHeight);
        _bannerView = new BannerView(_bannerAdUnitId, adSize, AdPosition.Bottom);
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        if (!_isBannerLoaded)
        {
            var adRequest = new AdRequest();
            _bannerView.LoadAd(adRequest);
            _isBannerLoaded = true;
        }
    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
            _isBannerLoaded = false;
        }
    }

    #endregion

    #region Rewarded Ad

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedAdUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded: " + ad.GetResponseInfo());

                _rewardedAd = ad;

                _rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    LoadRewardedAd();
                };
            });
    }


    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                HandleRewardAsync(reward);
            });
        }
        else
        {
            Debug.Log("Failed to load ad.");
        }
    }

    private async void HandleRewardAsync(Reward reward)
    {
        IAssetService currencyService = new LocalCurrencyService();

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserID;

        bool canEarn = await currencyService.EarnPlayerMoneyAsync(userId, (int)reward.Amount);
        if(canEarn == false)
        {
            Debug.LogError("Failed to earn money for the user.");
            return;
        }

        int money = await currencyService.GetCurrency(userId);

        // home page 씬인 경우 업데이트 진행
        HomePage homePage = UnityEngine.Object.FindObjectOfType<HomePage>();
        if (homePage == null) return;

        homePage.TopElementPresenter.ChangeGoldCount(money);
    }
    #endregion
}
