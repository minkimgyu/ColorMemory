using System;
using UnityEngine;
using GoogleMobileAds.Api;
using NetworkService.Manager;
using GooglePlayGames.BasicApi;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

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

    [SerializeField] public GameObject HomePage;
    private BannerView _bannerView;
    private RewardedAd _rewardedAd;
    private bool _isBannerLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
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
        MoneyManager moneyManager = new MoneyManager();

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

        await moneyManager.EarnPlayerMoneyAsync(userId, (int)reward.Amount);
        int money = await moneyManager.GetMoneyAsync(userId);

        HomePage.GetComponent<HomePage>().TopElementPresenter.ChangeGoldCount(money);
    }
    #endregion
}
