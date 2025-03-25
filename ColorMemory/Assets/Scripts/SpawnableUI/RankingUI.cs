using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public enum RankingIconName
{
    Icon1,
    Icon2,
    Icon3,
    Icon4,
    Icon5,
}

public struct RankingData
{
    List<PersonalRankingData> _otherRankingDatas;
    PersonalRankingData _myRankingData;

    public RankingData(List<PersonalRankingData> otherRankingDatas, PersonalRankingData myRankingData)
    {
        _otherRankingDatas = otherRankingDatas;
        _myRankingData = myRankingData;
    }

    public List<PersonalRankingData> OtherRankingDatas { get => _otherRankingDatas; }
    public PersonalRankingData MyRankingData { get => _myRankingData; }
}

public struct PersonalRankingData
{
    RankingIconName _iconName;
    string _name;
    int _score;
    int _rank;

    public PersonalRankingData(RankingIconName iconName, string name, int score, int rank)
    {
        _iconName = iconName;
        _name = name;
        _score = score;
        _rank = rank;
    }

    public RankingIconName IconName { get => _iconName; }
    public string Name { get => _name; }
    public int Score { get => _score; }
    public int Rank { get => _rank; }
}

public class RankingUI : SpawnableUI
{
    [SerializeField] Image _profileImg;

    [SerializeField] TMP_Text _nameTxt;
    [SerializeField] TMP_Text _scoreTxt;
    [SerializeField] TMP_Text _rankTxt;

    RectTransform _rectTransform;

    public override void ResetPosition()
    {
        _rectTransform.anchoredPosition = Vector3.zero;
    }

    public override void Initialize(Sprite profileSprite, string title, int score, int rank)
    {
        _rectTransform = GetComponent<RectTransform>();

        _profileImg.sprite = profileSprite;
        _nameTxt.text = title;
        _scoreTxt.text = $"{score:N0}";
        _rankTxt.text = $"#{rank}";
    }
}
