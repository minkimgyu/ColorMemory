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
    Button _selectBtn;

    ArtworkScrollUI _artworkScrollUI;

    GameObject _selectStageContent;
    Transform _stageUIContent;

    CollectPagePresenter _collectPagePresenter;

    public CollectPageViewer(
        GameObject content,
        TMP_Text titleTxt,
        TMP_Text descriptionTxt,
        Button selectBtn,

        ArtworkScrollUI artworkScrollUI,

        GameObject selectStageContent,
        Transform stageUIContent,

        CollectPagePresenter collectPagePresenter)
    {
        _content = content;
        _titleTxt = titleTxt;
        _descriptionTxt = descriptionTxt;
        _selectBtn = selectBtn;
        _artworkScrollUI = artworkScrollUI;

        _selectStageContent = selectStageContent;
        _stageUIContent = stageUIContent;

        _collectPagePresenter = collectPagePresenter;

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

    public void AddStage(SpawnableUI spawnableUI)
    {
        spawnableUI.transform.SetParent(_stageUIContent);
    }

    public void RemoveAllStage()
    {
        for (int i = 0; i < _stageUIContent.childCount; i++)
        {
            _stageUIContent.GetChild(i--).GetComponent<StageUI>().DestroyObject();
        }
    }

    public void ActiveSelectStageContent(bool active)
    {
        _selectStageContent.SetActive(active);
    }
}
