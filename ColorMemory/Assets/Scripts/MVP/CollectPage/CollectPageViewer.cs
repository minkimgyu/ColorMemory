using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectPageViewer
{
    GameObject _content;
    TMP_Text _titleTxt;
    TMP_Text _descriptionTxt;

    ArtworkScrollUI _artworkScrollUI;

    Image _completeSlider;

    TMP_Text _leftCompleteText;
    TMP_Text _totalCompleteText;

    GameObject _selectStageContent;
    Transform _stageUIContent;
    Button _exitBtn;
    Button _playBtn;

    GameObject _stageDetailContent;
    TMP_Text _stageHintUseCount;
    TMP_Text _stageWrongCount;

    CollectPagePresenter _collectPagePresenter;

    public CollectPageViewer(
        GameObject content,
        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        ArtworkScrollUI artworkScrollUI,

        Image completeSlider,
        TMP_Text leftCompleteText,
        TMP_Text totalCompleteText,

        GameObject selectStageContent,
        Transform stageUIContent,
        Button exitBtn,
        Button playBtn,

        GameObject stageDetailContent,
        TMP_Text stageHintUseCount,
        TMP_Text stageWrongCount,

        CollectPagePresenter collectPagePresenter)
    {
        _content = content;
        _titleTxt = titleTxt;
        _descriptionTxt = descriptionTxt;
        _artworkScrollUI = artworkScrollUI;

        _completeSlider = completeSlider;
        _leftCompleteText = leftCompleteText;
        _totalCompleteText = totalCompleteText;

        _selectStageContent = selectStageContent;
        _stageUIContent = stageUIContent;

        _exitBtn = exitBtn;
        _playBtn = playBtn;

        _stageDetailContent = stageDetailContent;
        _stageHintUseCount = stageHintUseCount;
        _stageWrongCount = stageWrongCount;

        _collectPagePresenter = collectPagePresenter;

        _exitBtn.onClick.AddListener(() => { collectPagePresenter.ActiveSelectStageContent(false); });
        _playBtn.onClick.AddListener(() => { collectPagePresenter.PlayCollectMode(); });
        artworkScrollUI.OnDragEnd += collectPagePresenter.ChangeArtworkDescription;
        ActiveContent(false);
    }

    public void ActiveStageDetailContent(bool active)
    {
        _stageDetailContent.SetActive(active);
    }

    public void ChangeCurrentProgress(int currentProgress, int totalProgress = 16)
    {
        float ratio = (float)currentProgress / totalProgress;
        int percentage = Mathf.RoundToInt(ratio * 100);

        _leftCompleteText.text = $"{Mathf.RoundToInt(percentage)}%";
        _totalCompleteText.text = $"100%";
        _completeSlider.fillAmount = ratio;
    }

    public void ChangeStageDetails(int hintCount, int wrongCount)
    {
        _stageHintUseCount.text = $"{hintCount}회";
        _stageWrongCount.text = $"{wrongCount}회";
    }

    public void ChangeArtDescription(string title, string description)
    {
        _titleTxt.text = title;
        _descriptionTxt.text = description;
    }

    public void SetUpArtworkScroll(int itemCount)
    {
        _artworkScrollUI.SetUp(itemCount);
    }

    public void AddArtwork(SpawnableUI artwork)
    {
        _artworkScrollUI.AddItem(artwork.transform);
    }

    public void RemoveAllArtwork()
    {
        _artworkScrollUI.DestroyItems();
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }

    public void SelectStage(int index)
    {
        int childCount = _stageUIContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            StageUI stageUI = _stageUIContent.GetChild(i).GetComponent<StageUI>();
            if(i == index)
            {
                _stageUIContent.GetChild(i).GetComponent<StageUI>().ChangeSelect(true);
            }
            else
            {
                _stageUIContent.GetChild(i).GetComponent<StageUI>().ChangeSelect(false);
            }
        }
    }

    public void AddStage(SpawnableUI spawnableUI)
    {
        spawnableUI.transform.SetParent(_stageUIContent);
    }

    public void RemoveAllStage()
    {
        for (int i = _stageUIContent.childCount - 1; i >= 0; i--)
        {
            _stageUIContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    public void ActiveSelectStageContent(bool active)
    {
        _selectStageContent.SetActive(active);
    }
}
