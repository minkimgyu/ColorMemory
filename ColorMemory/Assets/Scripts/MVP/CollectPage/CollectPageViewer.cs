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
    Button _playBtn;

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
        Button playBtn,

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

        _playBtn = playBtn;
        _collectPagePresenter = collectPagePresenter;

        _playBtn.onClick.AddListener(() => { collectPagePresenter.PlayCollectMode(); });
        artworkScrollUI.OnDragEnd += collectPagePresenter.ChangeArtworkDescription;
        ActiveContent(false);
    }

    public void ChangeArtDescription(string title, string description)
    {
        _titleTxt.text = title;
        _descriptionTxt.text = description;
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

    public void SelectStage(Vector2Int index)
    {
        int childCount = _stageUIContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            int row = i / 5;
            int col = i % 5;

            if(row == index.x && col == index.y)
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
