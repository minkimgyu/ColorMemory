using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

public class ChallengeModeMVPTest
{
    MockChallengeStageUIViewer _mockChallengeStageUIViewer;
    ChallengeStageUIModel _challengeStageUIModel;
    ChallengeStageUIPresenter _challengeStageUIPresenter;

    [SetUp]
    public void Setup()
    {
        _mockChallengeStageUIViewer = new MockChallengeStageUIViewer();
        _challengeStageUIModel = new ChallengeStageUIModel();
        _challengeStageUIPresenter = new ChallengeStageUIPresenter(_challengeStageUIModel);
        _challengeStageUIPresenter.InjectViewer(_mockChallengeStageUIViewer);
    }

    [Test]
    public void TestBottomContentActivation()
    {
        // ActivateBottomContent tests
        _challengeStageUIPresenter.ActivateBottomContent(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsBottomContentActive, "BottomContent should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveBottomContent, "BottomContent should be active in model.");

        _challengeStageUIPresenter.ActivateBottomContent(false);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsBottomContentActive, "BottomContent should be inactive in viewer.");
        Assert.IsFalse(_challengeStageUIModel.ActiveBottomContent, "BottomContent should be inactive in model.");
    }

    [Test]
    public void TestSkipBtnActivation()
    {
        // ActivateSkipBtn tests
        _challengeStageUIPresenter.ActivateSkipBtn(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsSkipBtnActive, "SkipBtn should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveSkipBtn, "SkipBtn should be active in model.");

        _challengeStageUIPresenter.ActivateSkipBtn(false);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsSkipBtnActive, "SkipBtn should be inactive in viewer.");
        Assert.IsFalse(_challengeStageUIModel.ActiveSkipBtn, "SkipBtn should be inactive in model.");
    }

    [Test]
    public void TestHint()
    {
        // ChangeHintCost tests
        _challengeStageUIPresenter.ChangeHintCost(100, 150);
        Assert.AreEqual("-100", _mockChallengeStageUIViewer.OneColorHintCost, "OneColorHintCost in viewer is incorrect.");
        Assert.AreEqual("-150", _mockChallengeStageUIViewer.OneZoneHintCost, "OneZoneHintCost in viewer is incorrect.");
        Assert.AreEqual("-100", _challengeStageUIModel.OneColorHintCost, "OneColorHintCost in model is incorrect.");
        Assert.AreEqual("-150", _challengeStageUIModel.OneZoneHintCost, "OneZoneHintCost in model is incorrect.");

        // ActivateHint tests
        _challengeStageUIPresenter.ActivateHint(true, false);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsOneColorHintActive, "OneColorHint should be active in viewer.");
        Assert.IsFalse(_mockChallengeStageUIViewer.IsOneZoneHintActive, "OneZoneHint should be inactive in viewer.");
        Assert.IsTrue(_challengeStageUIModel.OneColorHintActive, "OneColorHint should be active in model.");
        Assert.IsFalse(_challengeStageUIModel.OneZoneHintActive, "OneZoneHint should be inactive in model.");

        _challengeStageUIPresenter.ActivateHint(false, true);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsOneColorHintActive, "OneColorHint should be inactive in viewer.");
        Assert.IsTrue(_mockChallengeStageUIViewer.IsOneZoneHintActive, "OneZoneHint should be active in viewer.");
        Assert.IsFalse(_challengeStageUIModel.OneColorHintActive, "OneColorHint should be inactive in model.");
        Assert.IsTrue(_challengeStageUIModel.OneZoneHintActive, "OneZoneHint should be active in model.");
    }

    //---

    [Test]
    public void TestPausePanelActivation()
    {
        // Initial state: Pause panel is inactive
        Assert.IsFalse(_mockChallengeStageUIViewer.IsPausePanelActive, "Pause panel should be inactive initially.");
        Assert.IsFalse(_challengeStageUIModel.ActivePausePanel, "Model's pause panel state should be inactive initially.");

        // Activate pause panel
        _challengeStageUIPresenter.ActivatePausePanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsPausePanelActive, "Pause panel should be active after activation.");
        Assert.IsTrue(_challengeStageUIModel.ActivePausePanel, "Model's pause panel state should be active after activation.");

        Assert.AreEqual(0f, _challengeStageUIModel.BgmRatio, "Model BGM ratio should be updated.");
        Assert.AreEqual(0f, _mockChallengeStageUIViewer.BGMSliderRatio, "SoundPlayer BGM volume should be updated.");

        // Deactivate pause panel
        _challengeStageUIPresenter.ActivatePausePanel(false);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsPausePanelActive, "Pause panel should be inactive after deactivation.");
        Assert.IsFalse(_challengeStageUIModel.ActivePausePanel, "Model's pause panel state should be inactive after deactivation.");

        Assert.AreEqual(0f, _challengeStageUIModel.BgmRatio, "Model BGM ratio should be updated.");
        Assert.AreEqual(0f, _mockChallengeStageUIViewer.BGMSliderRatio, "SoundPlayer BGM volume should be updated.");
    }

    [Test]
    public void TestStageOverPreviewPanel()
    {
        // Activate
        _challengeStageUIPresenter.ActivateStageOverPreviewPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsStageOverPreviewPanelActive, "StageOverPreviewPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveStageOverPreviewPanel, "StageOverPreviewPanel should be active in model.");

        // Deactivate
        _challengeStageUIPresenter.ActivateStageOverPreviewPanel(false);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsStageOverPreviewPanelActive, "StageOverPreviewPanel should be inactive in viewer.");
        Assert.IsFalse(_challengeStageUIModel.ActiveStageOverPreviewPanel, "StageOverPreviewPanel should be inactive in model.");
    }

    //---

    [Test]
    public void TestChangeStageOverInfo()
    {
        _challengeStageUIPresenter.ChangeStageCount(5); // Set stage count for localization
        _challengeStageUIPresenter.ChangeStageOverInfo("Oh no! Time's up", "You couldn't clear Pattern {0}", "Try again next time!");

        Assert.AreEqual("Oh no! Time's up", _mockChallengeStageUIViewer.StageOverTitle, "StageOverTitle is incorrect.");
        Assert.AreEqual("You couldn't clear Pattern 5", _mockChallengeStageUIViewer.StageOverInfo1, "StageOverInfo1 is incorrect.");
        Assert.AreEqual("Try again next time!", _mockChallengeStageUIViewer.StageOverInfo2, "StageOverInfo2 is incorrect.");

        Assert.AreEqual("Oh no! Time's up", _challengeStageUIModel.StageOverTitleText, "Model StageOverTitleText is incorrect.");
        Assert.AreEqual("You couldn't clear Pattern 5", _challengeStageUIModel.StageOverInfo1Text, "Model StageOverInfo1Text is incorrect.");
        Assert.AreEqual("Try again next time!", _challengeStageUIModel.StageOverInfo2Text, "Model StageOverInfo2Text is incorrect.");
    }

    //---

    [Test]
    public void TestPlayPanelActivationAndScoreAndTimeChanges()
    {
        // Activate Play Panel
        _challengeStageUIPresenter.ActivatePlayPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsPlayPanelActive, "PlayPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActivePlayPanel, "PlayPanel should be active in model.");

        // Change Best Score
        _challengeStageUIPresenter.ChangeBestScore(12345);
        Assert.AreEqual("12345", _mockChallengeStageUIViewer.BestScore, "BestScore in viewer is incorrect.");
        Assert.AreEqual(12345, _challengeStageUIModel.BestScore, "BestScore in model is incorrect.");

        // Change Now Score
        _challengeStageUIPresenter.ChangeNowScore(54321);
        Assert.AreEqual("54321", _mockChallengeStageUIViewer.NowScore, "NowScore in viewer is incorrect.");
        Assert.AreEqual(54321, _challengeStageUIModel.NowScore, "NowScore in model is incorrect.");

        // Change Total Time
        _challengeStageUIPresenter.ChangeTotalTime(123.455555f);
        Assert.AreEqual("123.46", _mockChallengeStageUIViewer.TotalTime, "TotalTime in viewer is incorrect.");
        Assert.AreEqual("123.46", _challengeStageUIModel.TotalTime, "TotalTime in model is incorrect.");

        _challengeStageUIPresenter.ChangeTotalTime(9.87f);
        Assert.AreEqual("09.87", _mockChallengeStageUIViewer.TotalTime, "TotalTime with single digit integer part is incorrect.");

        // Change Left Time
        _challengeStageUIPresenter.ChangeLeftTime(60.1f, 0.5f);
        Assert.AreEqual("60.10", _mockChallengeStageUIViewer.LeftTime, "LeftTime in viewer is incorrect.");
        Assert.AreEqual(0.5f, _mockChallengeStageUIViewer.TimerRatio, "TimerRatio in viewer is incorrect.");
        Assert.AreEqual("60.10", _challengeStageUIModel.LeftTime, "LeftTime in model is incorrect.");
        Assert.AreEqual(0.5f, _challengeStageUIModel.TimeRatio, "TimeRatio in model is incorrect.");

        _challengeStageUIPresenter.ChangeLeftTime(5.05f, 0.1f);
        Assert.AreEqual("05.05", _mockChallengeStageUIViewer.LeftTime, "LeftTime with single digit integer part is incorrect.");
    }

    //---

    [Test]
    public void TestStageCountChange()
    {
        _challengeStageUIPresenter.ChangeStageCount(1);
        Assert.AreEqual("1", _mockChallengeStageUIViewer.StageText, "StageText should be '1'.");
        Assert.AreEqual(1, _challengeStageUIModel.StageCount, "Model StageCount should be 1.");

        _challengeStageUIPresenter.ChangeStageCount(10);
        Assert.AreEqual("10", _mockChallengeStageUIViewer.StageText, "StageText should be '10'.");
        Assert.AreEqual(10, _challengeStageUIModel.StageCount, "Model StageCount should be 10.");
    }

    //---

    [Test]
    public void TestRememberPanelActivation()
    {
        _challengeStageUIPresenter.ActivateRememberPanel(true, "Remember the Pattern!");
        Assert.IsTrue(_mockChallengeStageUIViewer.IsRememberPanelActive, "RememberPanel should be active in viewer.");
        Assert.AreEqual("Remember the Pattern!", _mockChallengeStageUIViewer.RememberText, "RememberText in viewer is incorrect.");
        Assert.IsTrue(_challengeStageUIModel.ActiveRememberPanel, "RememberPanel should be active in model.");
        Assert.AreEqual("Remember the Pattern!", _challengeStageUIModel.RememberTxt, "RememberTxt in model is incorrect.");

        _challengeStageUIPresenter.ActivateRememberPanel(false, "Remember the Pattern!");
        Assert.IsFalse(_mockChallengeStageUIViewer.IsRememberPanelActive, "RememberPanel should be inactive in viewer.");
        Assert.IsFalse(_challengeStageUIModel.ActiveRememberPanel, "RememberPanel should be inactive in model.");
    }

    //---

    [Test]
    public void TestHintPanelActivation()
    {
        _challengeStageUIPresenter.ActivateHintPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsHintPanelActive, "HintPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveHintPanel, "HintPanel should be active in model.");

        _challengeStageUIPresenter.ActivateHintPanel(false);
        Assert.IsFalse(_mockChallengeStageUIViewer.IsHintPanelActive, "HintPanel should be inactive in viewer.");
        Assert.IsFalse(_challengeStageUIModel.ActiveHintPanel, "HintPanel should be inactive in model.");
    }

    // ---

    [Test]
    public void TestCoinPanelActivationAndCount()
    {
        _challengeStageUIPresenter.ActiveGoldPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsCoinPanelActive, "CoinPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveCoinPanel, "CoinPanel should be active in model.");

        _challengeStageUIPresenter.ChangeCoinCount(1234567);
        Assert.AreEqual("1,234,567", _mockChallengeStageUIViewer.CoinCount, "CoinCount in viewer is incorrect.");
        Assert.AreEqual("1,234,567", _challengeStageUIModel.CoinCount, "CoinCount in model is incorrect.");
    }

    //---

    [Test]
    public void TestGameOverPanelActivationAndClearStageInfo()
    {
        _challengeStageUIPresenter.ActivateGameOverPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsGameOverPanelActive, "GameOverPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveGameOverPanel, "GameOverPanel should be active in model.");

        _challengeStageUIPresenter.ChangeClearStageCount(12, 12345);
        Assert.AreEqual("12", _mockChallengeStageUIViewer.ClearStageCount, "ClearStageCount in viewer is incorrect.");
        Assert.AreEqual("12,345", _mockChallengeStageUIViewer.ResultScore, "ResultScore in viewer is incorrect.");
        Assert.AreEqual("12", _challengeStageUIModel.ClearStageCount, "ClearStageCount in model is incorrect.");
        Assert.AreEqual("12,345", _challengeStageUIModel.ResultScore, "ResultScore in model is incorrect.");
    }

    //---

    [Test]
    public void TestGameResultPanelActivationAndGoldCount()
    {
        _challengeStageUIPresenter.ActivateGameResultPanel(true);
        Assert.IsTrue(_mockChallengeStageUIViewer.IsGameResultPanelActive, "GameResultPanel should be active in viewer.");
        Assert.IsTrue(_challengeStageUIModel.ActiveGameResultPanel, "GameResultPanel should be active in model.");

        _challengeStageUIPresenter.ChangeResultGoldCount(500, "{0} Gold Acquired!");
        Assert.AreEqual("500 Gold Acquired!", _mockChallengeStageUIViewer.ResultGoldCount, "ResultGoldCount in viewer is incorrect.");
        Assert.AreEqual("500 Gold Acquired!", _challengeStageUIModel.GoldCount, "ResultGoldCount in model is incorrect.");
    }
}
