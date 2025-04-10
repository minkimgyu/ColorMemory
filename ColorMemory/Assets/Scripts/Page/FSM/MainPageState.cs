using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageState : BaseState<HomePage.InnerPageState>
{
    GameMode.Type _type = GameMode.Type.Collect;
    EffectFactory _effectFactory;
    DotFactory _dotFactory;

    const int _dotSize = 6;
    const float _contentSize = 0.8f;

    Image _modeTitleImg;
    ToggleBtn _toggleBtn;
    Transform _dotParent;

    Image _playBtnImg;
    Button _playBtn;

    GameObject _mainContent;

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

    MainPagePresenter _homePagePresenter;

    readonly Color[] _pickColors = new Color[] { new Color(255f/255f, 182f/255f, 108f/255f), new Color(255f/255f, 236f/255f, 89f/255f), new Color(219f/255f, 200f/255f, 255f/255f) };

    public MainPageState(
        GameMode.Type type,
        EffectFactory effectFactory,
        DotFactory dotFactory,
        Image modeTitleImg,
        ToggleBtn toggleBtn,
        Transform dotParent,
        Button playBtn,

        GameObject mainContent,
        Dictionary<GameMode.Type, Sprite> modeTitleIconAssets,

        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _type = type;

        _modeTitleImg = modeTitleImg;
        _toggleBtn = toggleBtn;
        _dotParent = dotParent;

        _effectFactory = effectFactory;
        _dotFactory = dotFactory;

        _mainContent = mainContent;

        _playBtn = playBtn;
        _playBtnImg = _playBtn.gameObject.GetComponent<Image>();

        _toggleBtn.Initialize();

        Dot[,] dots;
        dots = new Dot[_dotSize, _dotSize];

        for (int i = 0; i < _dotSize; i++)
        {
            for (int j = 0; j < _dotSize; j++)
            {
                Dot dot = _dotFactory.Create(Dot.Name.Basic);
                dot.Initialize();
                dot.Inject(_effectFactory, new Vector2Int(i, j), (v2) => { dot.Pop(_pickColors[Random.Range(0, _pickColors.Length)]); });

                dot.transform.SetParent(_dotParent);
                dots[i, j] = dot;
            }
        }

        _dotParent.localScale = Vector3.one * _contentSize;

        // _toggleBtn 이거 넣기
        // _playBtn 이거 넣기

        MainPageModel homePageModel = new MainPageModel(_type, modeTitleIconAssets, _dotColors, _playBtnColors);
        _homePagePresenter = new MainPagePresenter(homePageModel, OnClickPlayBtn);
        MainPageViewer homePageViewer = new MainPageViewer(_mainContent, _modeTitleImg, _playBtn, _playBtnImg, _toggleBtn, dots, _homePagePresenter);
        _homePagePresenter.InjectViewer(homePageViewer);

        _homePagePresenter.ActiveContent(false);
    }

    public void OnClickPlayBtn(GameMode.Type type)
    {
        switch (type)
        {
            case GameMode.Type.Challenge:
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.ChallengeScene);
                break;
            case GameMode.Type.Collect:
                _fsm.SetState(HomePage.InnerPageState.Collection);
                break;
            default:
                break;
        }
    }

    public override void OnClickShopBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Shop);
    }

    public override void OnClickRankingBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Ranking);
    }

    // 이건 HomePagePresenter 통해서 작동시켜주자
    public override void OnStateEnter()
    {
        _homePagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _homePagePresenter.ActiveContent(false); // home 닫아주기
    }
}
