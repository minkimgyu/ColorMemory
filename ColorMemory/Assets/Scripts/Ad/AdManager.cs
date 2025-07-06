using System;
using UnityEngine;
using GoogleMobileAds.Api;
using NetworkService.Manager;
using GooglePlayGames.BasicApi;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

#if UNITY_ANDROID
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
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

        int x = (int)Screen.width / 2 + 160; // 배너 너비의 절반
        int y = Screen.height - 150; 

        AdSize adSize = new AdSize(320, 100);
        _bannerView = new BannerView(_bannerAdUnitId, adSize, x, y);
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
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
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });

        _rewardedAd.OnAdFullScreenContentClosed += () => {
            LoadRewardedAd();
        };
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
            Debug.Log("보상형 광고를 아직 로드하지 못했거나, 광고를 표시할 수 없습니다.");
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
