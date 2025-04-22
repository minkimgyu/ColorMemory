using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPageViewer
{
    GameObject _content;
    TMPro.TMP_Text _rankingTitleText;
    Transform _scrollContent;
    Transform _myRankingContent;

    public RankingPageViewer(GameObject content, TMPro.TMP_Text rankingTitleText, Transform scrollContent, Transform myRankingContent)
    {
        _content = content;
        _rankingTitleText = rankingTitleText;
        _scrollContent = scrollContent;
        _myRankingContent = myRankingContent;
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }

    public void ChangeRankingTitle(string title)
    {
        _rankingTitleText.text = title;
    }

    public void DestoryRankingItems()
    {
        for (int i = _scrollContent.childCount - 1; i >= 0; i--)
        {
            _scrollContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }

        for (int i = _myRankingContent.childCount - 1; i >= 0; i--)
        {
            _myRankingContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    public void AddRankingItemToScroll(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
        rankingUI.transform.localScale = Vector3.one;
    }

    public void AddMyRankingItem(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_myRankingContent);
        rankingUI.transform.localScale = Vector3.one;
        rankingUI.ResetPosition();
    }
}
