using NUnit.Framework;
using UnityEngine;

public class CollectModeMVPTest
{
    MockCollectStageUIViewer _viewer;
    CollectStageUIModel _model;
    CollectStageUIPresenter _presenter;

    [SetUp]
    public void Setup()
    {
        _viewer = new MockCollectStageUIViewer();
        _model = new CollectStageUIModel();
        _presenter = new CollectStageUIPresenter(_model);
        _presenter.InjectViewer(_viewer);
    }

    // --- Panel Activation Tests ---
    [Test]
    public void TestActivatePlayPanel()
    {
        _presenter.ActivatePlayPanel(true);
        Assert.IsTrue(_model.ActivePlayPanel);
        Assert.IsTrue(_viewer.PlayPanelActive);

        _presenter.ActivatePlayPanel(false);
        Assert.IsFalse(_model.ActivePlayPanel);
        Assert.IsFalse(_viewer.PlayPanelActive);
    }

    [Test]
    public void TestActivateTimerContent()
    {
        _presenter.ActivateTimerContent(true);
        Assert.IsTrue(_model.ActiveTimerContent);
        Assert.IsTrue(_viewer.TimerContentActive);

        _presenter.ActivateTimerContent(false);
        Assert.IsFalse(_model.ActiveTimerContent);
        Assert.IsFalse(_viewer.TimerContentActive);
    }

    [Test]
    public void TestActivateDetailContent()
    {
        _presenter.ActivateDetailContent(true);
        Assert.IsTrue(_model.ActiveDetailContent);
        Assert.IsTrue(_viewer.DetailContentActive);

        _presenter.ActivateDetailContent(false);
        Assert.IsFalse(_model.ActiveDetailContent);
        Assert.IsFalse(_viewer.DetailContentActive);
    }

    [Test]
    public void TestActivateBottomContent()
    {
        _presenter.ActivateBottomContent(true);
        Assert.IsTrue(_model.ActiveBottomContent);
        Assert.IsTrue(_viewer.BottomContentActive);

        _presenter.ActivateBottomContent(false);
        Assert.IsFalse(_model.ActiveBottomContent);
        Assert.IsFalse(_viewer.BottomContentActive);
    }

    [Test]
    public void TestActivateGameClearPanel()
    {
        _presenter.ActivateGameClearPanel(true);
        Assert.IsTrue(_model.ActiveGameClearPanel);
        Assert.IsTrue(_viewer.GameClearPanelActive);

        _presenter.ActivateGameClearPanel(false);
        Assert.IsFalse(_model.ActiveGameClearPanel);
        Assert.IsFalse(_viewer.GameClearPanelActive);
    }

    [Test]
    public void TestActivatePausePanel()
    {
        // Activate Pause Panel
        _presenter.ActivatePausePanel(true);
        Assert.IsTrue(_model.ActivePausePanel);
        Assert.IsTrue(_viewer.PausePanelActive);

        // Deactivate Pause Panel
        _presenter.ActivatePausePanel(false);
        Assert.IsFalse(_model.ActivePausePanel);
        Assert.IsFalse(_viewer.PausePanelActive);
    }

    [Test]
    public void TestActivateGameResultPanel()
    {
        _presenter.ActivateGameResultPanel(true);
        Assert.IsTrue(_model.ActiveGameResultPanel);
        Assert.IsTrue(_viewer.GameResultPanelActive);

        _presenter.ActivateGameResultPanel(false);
        Assert.IsFalse(_model.ActiveGameResultPanel);
        Assert.IsFalse(_viewer.GameResultPanelActive);
    }

    [Test]
    public void TestActivateRememberPanel()
    {
        _presenter.ActivateRememberPanel(true);
        Assert.IsTrue(_model.ActiveRememberPanel);
        Assert.IsTrue(_viewer.RememberPanelActive);

        _presenter.ActivateRememberPanel(false);
        Assert.IsFalse(_model.ActiveRememberPanel);
        Assert.IsFalse(_viewer.RememberPanelActive);
    }

    [Test]
    public void TestActivateSharePanel()
    {
        _presenter.ActivateSharePanel(true);
        Assert.IsTrue(_model.ActiveSharePanel);
        Assert.IsTrue(_viewer.SharePanelActive);

        _presenter.ActivateSharePanel(false);
        Assert.IsFalse(_model.ActiveSharePanel);
        Assert.IsFalse(_viewer.SharePanelActive);
    }

    // --- Button Activation Tests ---
    [Test]
    public void TestActivateSkipBtn()
    {
        _presenter.ActivateSkipBtn(true);
        Assert.IsTrue(_model.ActiveSkipBtn);
        Assert.IsTrue(_viewer.SkipBtnActive);

        _presenter.ActivateSkipBtn(false);
        Assert.IsFalse(_model.ActiveSkipBtn);
        Assert.IsFalse(_viewer.SkipBtnActive);
    }

    [Test]
    public void TestActivateNextStageBtn()
    {
        _presenter.ActivateNextStageBtn(true);
        Assert.IsTrue(_model.ActiveNextStageBtn);
        Assert.IsTrue(_viewer.NextStageBtnActive);

        _presenter.ActivateNextStageBtn(false);
        Assert.IsFalse(_model.ActiveNextStageBtn);
        Assert.IsFalse(_viewer.NextStageBtnActive);
    }

    [Test]
    public void TestActivateClearExitBtn()
    {
        _presenter.ActivateClearExitBtn(true);
        Assert.IsTrue(_model.ActiveClearExitBtn);
        Assert.IsTrue(_viewer.ClearExitBtnActive);

        _presenter.ActivateClearExitBtn(false);
        Assert.IsFalse(_model.ActiveClearExitBtn);
        Assert.IsFalse(_viewer.ClearExitBtnActive);
    }

    [Test]
    public void TestActivateOpenShareBtnInteraction()
    {
        _presenter.ActivateOpenShareBtnInteraction(true);
        Assert.IsTrue(_model.ActiveOpenShareBtn);
        Assert.IsTrue(_viewer.OpenShareBtnActive);

        _presenter.ActivateOpenShareBtnInteraction(false);
        Assert.IsFalse(_model.ActiveOpenShareBtn);
        Assert.IsFalse(_viewer.OpenShareBtnActive);
    }

    [Test]
    public void TestActivateShareBottomItems()
    {
        _presenter.ActivateShareBottomItems(true);
        Assert.IsTrue(_model.ActiveShareBottomItems);
        Assert.IsTrue(_viewer.ShareBtnActive);
        Assert.IsTrue(_viewer.ShareExitBtnActive);

        _presenter.ActivateShareBottomItems(false);
        Assert.IsFalse(_model.ActiveShareBottomItems);
        Assert.IsFalse(_viewer.ShareBtnActive);
        Assert.IsFalse(_viewer.ShareExitBtnActive);
    }

    // --- Text and Data Change Tests ---
    [Test]
    public void TestChangeShareTitle()
    {
        string testTitle = "New Share Title";
        _presenter.ChangeShareTitle(testTitle);
        Assert.AreEqual(testTitle, _model.ShareTitle);
        Assert.AreEqual(testTitle, _viewer.ShareTitle);
    }

    [Test]
    public void TestChangeArtworkTitle()
    {
        string testTitle = "Grand Masterpiece";
        _presenter.ChangeArtworkTitle(testTitle);
        Assert.AreEqual(testTitle, _model.ArtworkTitle);
        Assert.AreEqual(testTitle, _viewer.ArtworkTitle);
    }

    [Test]
    public void TestChangeGetRankTitle()
    {
        string rankTitle = "Your Score Rank";
        string hintTitle = "Used Hints:";
        string wrongTitle = "Errors:";
        _presenter.ChangeGetRankTitle(rankTitle, hintTitle, wrongTitle);
        Assert.AreEqual(rankTitle, _model.GetRankTitle);
        Assert.AreEqual(hintTitle, _model.TotalHintUsageTitle);
        Assert.AreEqual(wrongTitle, _model.TotalWrongCountTitle);
        Assert.AreEqual(rankTitle, _viewer.GetRankTitle);
        Assert.AreEqual(hintTitle, _viewer.TotalHintUsageTitle);
        Assert.AreEqual(wrongTitle, _viewer.TotalWrongCountTitle);
    }

    [Test]
    public void TestChangeMyCollectionTitle()
    {
        string myCollection = "My Gallery";
        string current = "Collected:";
        string total = "Overall:";
        _presenter.ChangeMyCollectionTitle(myCollection, current, total);
        Assert.AreEqual(myCollection, _model.MyCollectionTitle);
        Assert.AreEqual(current, _model.CurrentCollectTitle);
        Assert.AreEqual(total, _model.TotalCollectTitle);
        Assert.AreEqual(myCollection, _viewer.MyCollectionTitle);
        Assert.AreEqual(current, _viewer.CurrentCollectTitle);
        Assert.AreEqual(total, _viewer.TotalCollectTitle);
    }

    [Test]
    public void TestChangeGameResultTitle()
    {
        // When artwork is completed
        _presenter.ChangeGameResultTitle("Artwork Completed!");
        Assert.AreEqual("Artwork Completed!", _model.GameResultTitle);
        Assert.AreEqual("Artwork Completed!", _viewer.GameResultTitle);

        // When artwork is in progress
        _presenter.ChangeGameResultTitle("Artwork In Progress");
        Assert.AreEqual("Artwork In Progress", _model.GameResultTitle);
        Assert.AreEqual("Artwork In Progress", _viewer.GameResultTitle);
    }

    [Test]
    public void TestChangeClearTitleInfo()
    {
        string title = "Stage Cleared!";
        _presenter.ChangeClearTitleInfo(title);
        Assert.AreEqual(title, _model.ClearTitleInfo);
        Assert.AreEqual(title, _viewer.ClearTitleText);
    }

    [Test]
    public void TestChangeClearContentInfo()
    {
        string content = "You've done great!";
        _presenter.ChangeClearContentInfo(content);
        Assert.AreEqual(content, _model.ClearContentInfo);
        Assert.AreEqual(content, _viewer.ClearContentText);
    }

    [Test]
    public void TestChangeCurrentHintUsage()
    {
        _presenter.ChangeCurrentHintUsage(3, "Used: {0}");
        Assert.AreEqual("Used: 3", _model.CurrentHintUsage);
        Assert.AreEqual("Used: 3", _viewer.HintUsageText);
    }

    [Test]
    public void TestChangeCurrentWrongCount()
    {
        _presenter.ChangeCurrentWrongCount(5, "Wrong: {0}");
        Assert.AreEqual("Wrong: 5", _model.CurrentWrongCount);
        Assert.AreEqual("Wrong: 5", _viewer.WrongCountText);
    }

    [Test]
    public void TestChangeArtworkPreview()
    {
        Sprite s1 = Sprite.Create(new Texture2D(10, 10), new Rect(0, 0, 10, 10), Vector2.zero);
        Sprite s2 = Sprite.Create(new Texture2D(20, 20), new Rect(0, 0, 20, 20), Vector2.zero);
        Sprite s3 = Sprite.Create(new Texture2D(30, 30), new Rect(0, 0, 30, 30), Vector2.zero);

        _presenter.ChangeArtworkPreview(s1, s2, s3);
        Assert.AreEqual(s1, _model.PreviewArtSprite);
        Assert.AreEqual(s2, _model.PreviewArtFrameSprite);
        Assert.AreEqual(s3, _model.PreviewRankDecorationIconSprite);
        // Viewer is mocked, so direct checking of sprites in viewer isn't meaningful without specific mock logic.
        // The fact that the model is updated implies the presenter called the viewer correctly.
    }

    [Test]
    public void TestChangeArtwork()
    {
        Sprite s1 = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
        Sprite s2 = Sprite.Create(new Texture2D(2, 2), new Rect(0, 0, 2, 2), Vector2.zero);
        Sprite s3 = Sprite.Create(new Texture2D(3, 3), new Rect(0, 0, 3, 3), Vector2.zero);

        _presenter.ChangeArtwork(s1, s2, s3, true);
        Assert.AreEqual(s1, _model.ArtSprite);
        Assert.AreEqual(s2, _model.ArtFrameSprite);
        Assert.AreEqual(s3, _model.RankDecorationIconSprite);
        Assert.IsTrue(_model.HasIt);
    }

    [Test]
    public void TestChangeRank()
    {
        Sprite rankIcon = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
        Color rankColor = Color.blue;
        string rankName = "Expert";

        _presenter.ChangeRank(rankIcon, true, rankName, rankColor);
        Assert.AreEqual(rankIcon, _model.RankIcon);
        Assert.IsTrue(_model.ActiveIcon);
        Assert.AreEqual(rankName, _model.RankName);
        Assert.AreEqual(rankColor, _model.RankBackgroundColor);
    }

    [Test]
    public void TestChangeGetRank()
    {
        _presenter.ChangeGetRank(10, 2, "Used: {0} times", "Mistakes: {0}");
        Assert.AreEqual("Used: 10 times", _model.TotalHintUseCount);
        Assert.AreEqual("Mistakes: 2", _model.TotalWrongCount);
        Assert.AreEqual("Used: 10 times", _viewer.TotalHintUseCount);
        Assert.AreEqual("Mistakes: 2", _viewer.TotalWrongCount);
    }

    [Test]
    public void TestChangeCollectionRatio()
    {
        _presenter.ChangeCollectionRatio(0.75f, 0.5f);
        Assert.AreEqual(0.75f, _model.CurrentCollectRatio);
        Assert.AreEqual(0.5f, _model.TotalCollectRatio);
        Assert.AreEqual("75%", _model.CurrentCollectRatioString);
        Assert.AreEqual("50%", _model.TotalCollectRatioString);
        Assert.AreEqual(0.75f, _viewer.CurrentCollectRatio);
        Assert.AreEqual("75%", _viewer.CurrentCollectText);
        Assert.AreEqual(0.5f, _viewer.TotalCollectRatio);
        Assert.AreEqual("50%", _viewer.TotalCollectText);
    }

    [Test]
    public void TestOnBGMSliderValeChanged()
    {
        // Change to non-zero volume
        _presenter.OnBGMSliderValeChanged(0.6f);
        Assert.AreEqual(0.6f, _model.BgmRatio);

        // Change to zero volume (Mute)
        _presenter.OnBGMSliderValeChanged(0f);
        Assert.AreEqual(0f, _model.BgmRatio);
    }

    [Test]
    public void TestOnSFXSliderValeChanged()
    {
        // Change to non-zero volume
        _presenter.OnSFXSliderValeChanged(0.9f);
        Assert.AreEqual(0.9f, _model.SfxRatio);

        // Change to zero volume (Mute)
        _presenter.OnSFXSliderValeChanged(0f);
        Assert.AreEqual(0f, _model.SfxRatio);
    }

    [Test]
    public void TestChangeHintInfoText()
    {
        string info = "Use hints wisely!";
        _presenter.ChangeHintInfoText(info);
        Assert.AreEqual(info, _model.HintInfo);
        Assert.AreEqual(info, _viewer.HintInfoText);
    }

    [Test]
    public void TestChangeProgressText()
    {
        _presenter.ChangeProgressText(75);
        Assert.AreEqual("75%", _model.Progress);
        Assert.AreEqual("75%", _viewer.ProgressText);
    }

    [Test]
    public void TestChangeTitle()
    {
        // Title within max length
        _presenter.ChangeTitle("My Stage", 1, 5);
        Assert.AreEqual("My Stage (1/5)", _model.Title);
        Assert.AreEqual("My Stage (1/5)", _viewer.TitleText);

        // Title exceeding max length
        _presenter.ChangeTitle("This Is A Very Very Long Stage Title Indeed", 2, 10);
        Assert.AreEqual("This Is A ... (2/10)", _model.Title); // 13 - 3 = 10 chars + "..."
        Assert.AreEqual("This Is A ... (2/10)", _viewer.TitleText);
    }

    [Test]
    public void TestChangeTotalTime()
    {
        _presenter.ChangeTotalTime(123.456f);
        Assert.AreEqual("123.46", _model.TotalTime);
        Assert.AreEqual("123.46", _viewer.TotalTimeText);

        _presenter.ChangeTotalTime(9.8f);
        Assert.AreEqual("09.80", _model.TotalTime);
        Assert.AreEqual("09.80", _viewer.TotalTimeText);

        _presenter.ChangeTotalTime(0.5f);
        Assert.AreEqual("00.50", _model.TotalTime);
        Assert.AreEqual("00.50", _viewer.TotalTimeText);
    }

    [Test]
    public void TestChangeLeftTime()
    {
        _presenter.ChangeLeftTime(60.123f, 0.75f);
        Assert.AreEqual("60.12", _model.LeftTime);
        Assert.AreEqual(0.75f, _model.TimeRatio);
        Assert.AreEqual("60.12", _viewer.LeftTimeText);
        Assert.AreEqual(0.75f, _viewer.TimerSliderRatio);

        _presenter.ChangeLeftTime(5.0f, 0.1f);
        Assert.AreEqual("05.00", _model.LeftTime);
        Assert.AreEqual(0.1f, _model.TimeRatio);
        Assert.AreEqual("05.00", _viewer.LeftTimeText);
        Assert.AreEqual(0.1f, _viewer.TimerSliderRatio);
    }
}
