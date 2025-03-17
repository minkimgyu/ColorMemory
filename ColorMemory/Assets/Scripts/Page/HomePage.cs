using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePage : MonoBehaviour
{
    GameMode.Type _type = GameMode.Type.Collect;
    EffectFactory _effectFactory;
    DotFactory _dotFactory;

    const int _dotSize = 6;
    const float _contentSize = 0.8f;

    [SerializeField] Image _modeTitleImg;
    [SerializeField] ToggleBtn _toggleBtn;
    [SerializeField] Transform _dotParent;

    Image _playBtnImg;
    [SerializeField] Button _playBtn;


    // 236f/255f, 232f/255f, 232f/255f // gray
    // 208f/255f, 162f/255f, 117f/255f // brown
    // 113f/255f, 191f/255f, 255f/255f // blue
    // 255f/255f, 154f/255f, 145f/255f // red

    Dictionary<GameMode.Type, Color[,]> _dotColors = new Dictionary<GameMode.Type, Color[,]>()
    {
        {
            GameMode.Type.Challenge, 
            new Color[6 , 6]
            {
                { new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(236f/255f, 232f/255f, 232f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(255f/255f, 154f/255f, 145f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f)},
                { new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(255f/255f, 154f/255f, 145f/255f)}
            }
        },
        {
            GameMode.Type.Collect,
            new Color[6 , 6]
            {
                { new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f)},
                { new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f)},
                { new Color(236f/255f, 232f/255f, 232f/255f), new Color(208f/255f, 162f/255f, 117f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(236f/255f, 232f/255f, 232f/255f), new Color(113f/255f, 191f/255f, 255f/255f), new Color(236f/255f, 232f/255f, 232f/255f)}
            }
        }
    };

    Dictionary<GameMode.Type, Color> _playBtnColors = new Dictionary<GameMode.Type, Color>()
    {
        { GameMode.Type.Challenge, new Color(255f/255f, 154f/255f, 145f/255f) },
        { GameMode.Type.Collect, new Color(113f/255f, 191f/255f, 255f/255f) }
    };

    void PlayGame()
    {
        switch (_type)
        {
            case GameMode.Type.Collect:
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.CollectScene);
                break;
            case GameMode.Type.Challenge:
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.ChallengeScene);
                break;
            default:
                break;
        }
    }

    void ChangeMode(bool isOn)
    {
        if (isOn)
        {
            _type = GameMode.Type.Challenge;
        }
        else
        {
            _type = GameMode.Type.Collect;
        }
    }

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null) return;

        _effectFactory = new EffectFactory(addressableHandler.EffectAssets);
        _dotFactory = new DotFactory(addressableHandler.DotAssets);

        _playBtn.onClick.AddListener(PlayGame);
        _playBtnImg = _playBtn.gameObject.GetComponent<Image>();

        _toggleBtn.Initialize();
        _toggleBtn.OnClick += ChangeMode;

        Dot[,] dots;
        dots = new Dot[_dotSize, _dotSize];

        for (int i = 0; i < _dotSize; i++)
        {
            for (int j = 0; j < _dotSize; j++)
            {
                Dot dot = _dotFactory.Create(Dot.Name.Basic);
                dot.Initialize();
                dot.Inject(_effectFactory, new Vector2Int(i, j), (v2) => { dot.Pop(Color.white); });

                dot.transform.SetParent(_dotParent);
                dots[i, j] = dot;
            }
        }

        _dotParent.localScale = Vector3.one * _contentSize;

        HomePageModel homePageModel = new HomePageModel(_type, addressableHandler.ModeTitleIconAssets, _dotColors, _playBtnColors);
        HomePagePresenter homePagePresenter = new HomePagePresenter(homePageModel);
        HomePageViewer homePageViewer = new HomePageViewer(_modeTitleImg, _playBtnImg, _toggleBtn, dots, homePagePresenter);
        homePagePresenter.InjectViewer(homePageViewer);

        switch (_type)
        {
            case GameMode.Type.Collect:
                _toggleBtn.ChangeState(false);
                break;
            case GameMode.Type.Challenge:
                _toggleBtn.ChangeState(true);
                break;
            default:
                break;
        }
    }
}
