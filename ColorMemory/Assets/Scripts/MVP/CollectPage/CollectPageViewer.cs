using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectPageViewer
{
    GameObject _content;

    GameObject _artworkInfoContent;
    GameObject _artworkCompleteRatioContent;

    Button _artworkInfoBtn;
    Button _artworkCompleteRatioBtn;

    TMP_Text _titleTxt;
    TMP_Text _descriptionTxt;

    Image _currentComplete;
    TMP_Text _currentCompleteRatio;

    Image _totalComplete;
    TMP_Text _totalCompleteRatio;

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

    FilterUI _filterScrollUI;
    Button _filterOpenBtn;
    Button _filterExitBtn;
    GameObject _filterContent;
    TMP_Text _collectionRatioText;
    Toggle[] _rankToggles;
    Toggle[] _dateToggles;

    CollectPagePresenter _collectPagePresenter;

    public CollectPageViewer(
        GameObject content,

        GameObject artworkInfoContent,
        GameObject artworkCompleteRatioContent,

        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        Image currentComplete,
        TMP_Text currentCompleteRatio,

        Image totalComplete,
        TMP_Text totalCompleteRatio,

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

        FilterUI filterScrollUI,
        Button filterOpenBtn,
        Button filterExitBtn,
        GameObject filterContent,
        TMP_Text collectionRatioText,
        Toggle[] rankToggles,
        Toggle[] dateToggles,

        CollectPagePresenter collectPagePresenter)
    {
        _content = content;

        _artworkInfoContent = artworkInfoContent;
        _artworkInfoContent.SetActive(true);

        _artworkCompleteRatioContent = artworkCompleteRatioContent;
        _artworkCompleteRatioContent.SetActive(false);

        _artworkInfoBtn = _artworkInfoContent.GetComponent<Button>();
        _artworkCompleteRatioBtn = _artworkCompleteRatioContent.GetComponent<Button>();

        _artworkInfoBtn.onClick.AddListener(() => { collectPagePresenter.SwitchArtworkInfoContent(false); });
        _artworkCompleteRatioBtn.onClick.AddListener(() => { collectPagePresenter.SwitchArtworkInfoContent(true); });


        _currentComplete = currentComplete;
        _currentCompleteRatio = currentCompleteRatio;

        _totalComplete = totalComplete;
        _totalCompleteRatio = totalCompleteRatio;


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

        _filterScrollUI = filterScrollUI;
        _filterOpenBtn = filterOpenBtn;
        _filterExitBtn = filterExitBtn;
        _filterContent = filterContent;
        _collectionRatioText = collectionRatioText;
        _rankToggles = rankToggles; // 이벤트 걸기
        _dateToggles = dateToggles; // 이벤트 걸기

        _collectPagePresenter = collectPagePresenter;

        _filterExitBtn.onClick.AddListener(() => { collectPagePresenter.ActivateFilterContent(false); });
        _filterOpenBtn.onClick.AddListener(() => { collectPagePresenter.ActivateFilterContent(true); });

        // 기본 필터를 제외한 나머지 필터 경우
        for (int i = 0; i < _rankToggles.Length; i++)
        {
            int rankIndex = i + 1;
            _rankToggles[i].onValueChanged.AddListener((on) => { if(on) collectPagePresenter.OnClickRankToggle((FilterUI.RankFilter)rankIndex); });
        }

        for (int i = 0; i < _dateToggles.Length; i++)
        {
            int dateIndex = i + 1;
            _dateToggles[i].onValueChanged.AddListener((on) => { if (on) collectPagePresenter.OnClickDateToggle((FilterUI.DateFilter)dateIndex); });
        }

        _exitBtn.onClick.AddListener(() => { collectPagePresenter.ActiveSelectStageContent(false); });
        _playBtn.onClick.AddListener(() => { collectPagePresenter.PlayCollectMode(); });
        artworkScrollUI.OnDragEnd += collectPagePresenter.OnArtworkScrollChanged; // -> 이거 수정해서 맞는 인덱스 적용해주기
        ActiveContent(false);
    }

    public void ChangeCollectionRatioInfo(float ratio)
    {
        _collectionRatioText.text = $"현재 전체 명화의 {ratio}%를 수집했어요!";
    }

    public void UnableAllToggles(FilterUI.FilterType type)
    {
        switch (type)
        {
            case FilterUI.FilterType.Rank:
                for (int i = 0; i < _rankToggles.Length; i++)
                {
                    _rankToggles[i].isOn = false;
                }
                break;
            case FilterUI.FilterType.Date:
                for (int i = 0; i < _dateToggles.Length; i++)
                {
                    _dateToggles[i].isOn = false;
                }
                break;
            default:
                break;
        }
    }

    public void SwitchArtworkInfoContent(bool active)
    {
        if(active == true)
        {
            _artworkInfoContent.SetActive(true);
            _artworkCompleteRatioContent.SetActive(false);
        }
        else
        {
            _artworkInfoContent.SetActive(false);
            _artworkCompleteRatioContent.SetActive(true);
        }
    }

    public void ActivateFilterScrollUI(bool active)
    {
        _filterScrollUI.Activate(active);
    }

    public void ActivateFilterBottomSheet(bool active)
    {
        _filterScrollUI.ActivateBottomSheet(active);
    }

    public void AddFilteredArtwork(SpawnableUI spawnableUI)
    {
        _filterScrollUI.AddFilteredArtwork(spawnableUI);
    }

    public void AddFilterItem(SpawnableUI spawnableUI)
    {
        _filterScrollUI.AddFilterItem(spawnableUI);
    }

    public void DestroyFilteredArtwork()
    {
        _filterScrollUI.DestroyFilteredArtwork();
    }


    public void ActiveFilterContent(bool active)
    {
        _filterContent.SetActive(active);
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

    public void ChangeArtCompleteRatio(float currentRatio, float totalRatio)
    {
        _currentComplete.fillAmount = currentRatio;
        _currentCompleteRatio.text = $"{Mathf.RoundToInt(currentRatio * 100)}%";

        _totalComplete.fillAmount = totalRatio;
        _totalCompleteRatio.text = $"{Mathf.RoundToInt(totalRatio * 100)}%";
    }

    public void SetUpArtworkScroll(int itemCount)
    {
        _artworkScrollUI.Setup();
    }

    public void SetArtworkScrollIndex(int scrollIndex)
    {
        _artworkScrollUI.ScrollTo(scrollIndex);
    }

    public void AddArtwork(SpawnableUI artwork)
    {
        _artworkScrollUI.AddItem(artwork.transform);
    }

    public void DestroyAllArtwork()
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
        spawnableUI.transform.localScale = Vector3.one;
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
