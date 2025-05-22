using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FilterUI;

public class CollectPageViewer
{
    GameObject _content;

    GameObject _artworkInfoContent;
    GameObject _artworkCompleteRatioContent;

    Button _artworkInfoBtn;
    Button _artworkCompleteRatioBtn;

    TMP_Text _titleTxt;
    TMP_Text _descriptionTxt;

    CustomProgressUI _currentComplete;
    TMP_Text _currentCompleteRatio;

    CustomProgressUI _totalComplete;
    TMP_Text _totalCompleteRatio;

    ArtworkScrollUI _artworkScrollUI;

    Image _completeSlider;

    TMP_Text _leftCompleteText;
    TMP_Text _totalCompleteText;

    TMP_Text _stageNameTxt;
    TMP_Text _selectStageTitle;

    TMP_Text _stageUsedHintUseTitle;
    TMP_Text _stageWrongCountTitle;

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
    TMP_Text _collectionCheerText;

    TMP_Text _ownFilterTitleTxt;
    TMP_Text _rankFilterTitleTxt;
    TMP_Text _dateFilterTitleTxt;

    Toggle[] _ownToggles;
    Toggle[] _rankToggles;
    Toggle[] _dateToggles;

    CollectPagePresenter _collectPagePresenter;

    public CollectPageViewer(
        GameObject content,

        GameObject artworkInfoContent,
        GameObject artworkCompleteRatioContent,

        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        CustomProgressUI currentComplete,
        TMP_Text currentCompleteRatio,

        CustomProgressUI totalComplete,
        TMP_Text totalCompleteRatio,

        ArtworkScrollUI artworkScrollUI,

        Image completeSlider,
        TMP_Text leftCompleteText,
        TMP_Text totalCompleteText,

        TMP_Text stageNameTxt,
        TMP_Text selectStageTitle,

        GameObject selectStageContent,
        Transform stageUIContent,

        TMP_Text stageUsedHintUseTitle,
        TMP_Text stageWrongCountTitle,

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
        TMP_Text collectionCheerText,

        TMP_Text ownFilterTitle,
        TMP_Text rankFilterTitle,
        TMP_Text dateFilterTitle,

        Toggle[] ownToggles,
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

        _stageNameTxt = stageNameTxt;
        _selectStageTitle = selectStageTitle;
        ChangeSelectStageTitle();

        _stageUsedHintUseTitle = stageUsedHintUseTitle;
        _stageWrongCountTitle = stageWrongCountTitle;

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
        _collectionCheerText = collectionCheerText;

        _ownFilterTitleTxt = ownFilterTitle;
        _rankFilterTitleTxt = rankFilterTitle;
        _dateFilterTitleTxt = dateFilterTitle;

        _ownToggles = ownToggles;
        _rankToggles = rankToggles; // 이벤트 걸기
        _dateToggles = dateToggles; // 이벤트 걸기

        _collectPagePresenter = collectPagePresenter;

        _filterExitBtn.onClick.AddListener(() => { collectPagePresenter.ActivateFilterContent(false); });
        _filterOpenBtn.onClick.AddListener(() => { collectPagePresenter.ActivateFilterContent(true); });

        for (int i = 0; i < _ownToggles.Length; i++)
        {
            int ownIndex = i;
            _ownToggles[i].onValueChanged.AddListener((on) => 
            { 
                if (on) collectPagePresenter.OnClickOwnToggle((FilterUI.OwnFilter)ownIndex);
                else collectPagePresenter.OnClickOwnToggle(FilterUI.OwnFilter.All);
            });
        }

        // 기본 필터를 제외한 나머지 필터 경우
        for (int i = 0; i < _rankToggles.Length; i++)
        {
            int rankIndex = i + 1;
            _rankToggles[i].onValueChanged.AddListener((on) => 
            { 
                if(on) collectPagePresenter.OnClickRankToggle((FilterUI.RankFilter)rankIndex);
                else collectPagePresenter.OnUnClickRankToggle((FilterUI.RankFilter)rankIndex);
            });
        }

        for (int i = 0; i < _dateToggles.Length; i++)
        {
            int dateIndex = i;
            _dateToggles[i].onValueChanged.AddListener((on) => 
            { 
                if (on) collectPagePresenter.OnClickDateToggle((FilterUI.DateFilter)dateIndex);
                else collectPagePresenter.OnClickDateToggle(FilterUI.DateFilter.Old);
            });
        }

        _exitBtn.onClick.AddListener(() => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            collectPagePresenter.ActiveSelectStageContent(false); 
        });

        _playBtn.onClick.AddListener(() => { collectPagePresenter.OnClickPlayBtn?.Invoke(); });
        artworkScrollUI.OnDragEnd += collectPagePresenter.OnArtworkScrollChanged; // -> 이거 수정해서 맞는 인덱스 적용해주기
        ActiveContent(false);
    }

    const int maxStageNameSize = 25;
    public void ChangeStageNameText(string stageNameText)
    {
        bool overLength = stageNameText.Length > maxStageNameSize;
        string newStageName;

        if (overLength == true)
        {
            int clampedLength = Mathf.Clamp(maxStageNameSize, 0, stageNameText.Length);
            newStageName = $"{stageNameText.Substring(0, clampedLength)}...";
        }
        else
        {
            newStageName = stageNameText;
        }

        _stageNameTxt.text = newStageName;
    }

    void ChangeSelectStageTitle()
    {
        _selectStageTitle.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.StageSelectTitle);
    }

    public void ChangeCollectionRatioInfo(float ratio)
    {
        string filterTitleFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.FilterTitle);
        _collectionRatioText.text = string.Format(filterTitleFormat, ratio);
    }

    public void ChangeFilterTitle(string ownFilterTitle, string rankFilterTitle, string dateFilterTitle)
    {
        _ownFilterTitleTxt.text = ownFilterTitle;
        _rankFilterTitleTxt.text = rankFilterTitle;
        _dateFilterTitleTxt.text = dateFilterTitle;
    }

    public void ChangeCollectionCheerInfo(string info)
    {
        _collectionCheerText.text = info;
    }

    public void UpdateToggles(FilterUI.OwnFilter ownFilter, List<FilterUI.RankFilter> rankFilters, FilterUI.DateFilter dateFilter)
    {
        for (int i = 0; i < _ownToggles.Length; i++)
        {
            if((int)ownFilter == i) _ownToggles[i].SetIsOnWithoutNotify(true);
            else _ownToggles[i].SetIsOnWithoutNotify(false);
        }

        if(ownFilter != OwnFilter.Clear)
        {
            for (int i = 0; i < _rankToggles.Length; i++)
            {
                _rankToggles[i].interactable = false;
                _rankToggles[i].SetIsOnWithoutNotify(false);
            }

            for (int i = 0; i < _dateToggles.Length; i++)
            {
                _dateToggles[i].interactable = false;
                _dateToggles[i].SetIsOnWithoutNotify(false);
            }
        }
        else
        {
            for (int i = 0; i < _rankToggles.Length; i++)
            {
                _rankToggles[i].interactable = true;
            }

            for (int i = 0; i < _dateToggles.Length; i++)
            {
                _dateToggles[i].interactable = true;
            }

            for (int i = 0; i < _rankToggles.Length; i++)
            {
                bool haveRank = rankFilters.Contains((FilterUI.RankFilter)(i + 1));
                _rankToggles[i].SetIsOnWithoutNotify(haveRank);
            }

            for (int i = 0; i < _dateToggles.Length; i++)
            {
                if ((int)dateFilter == i) _dateToggles[i].SetIsOnWithoutNotify(true);
                else _dateToggles[i].SetIsOnWithoutNotify(false);
            }
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

    public void DestroyFilteredArtwork()
    {
        _filterScrollUI.DestroyFilteredArtwork();
    }


    public void AddFilterItem(SpawnableUI spawnableUI)
    {
        _filterScrollUI.AddFilterItem(spawnableUI);
    }

    public void DestroyFilterItems()
    {
        _filterScrollUI.DestroyFilterItem();
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
        _stageUsedHintUseTitle.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.HintUsage);
        _stageWrongCountTitle.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.WrongCount);

        string countTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);
        string countsTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);


        if (hintCount > 1) _stageHintUseCount.text = string.Format(countsTxt, hintCount);
        else _stageHintUseCount.text = string.Format(countTxt, hintCount);

        if (wrongCount > 1) _stageWrongCount.text = string.Format(countsTxt, wrongCount);
        else _stageWrongCount.text = string.Format(countTxt, wrongCount);
    }

    public void ChangeArtDescription(string title, string description)
    {
        _titleTxt.text = title;
        _descriptionTxt.text = description;
    }

    public void ChangeArtCompleteRatio(float currentRatio, float totalRatio)
    {
        _currentComplete.FillValue = currentRatio;
        _currentCompleteRatio.text = $"{Mathf.RoundToInt(currentRatio * 100)}%";

        _totalComplete.FillValue = totalRatio;
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
