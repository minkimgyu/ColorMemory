using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ProfileIconName
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
    int _profileIconIndex;
    string _name; // key
    int _score;
    int _ranking;

    public PersonalRankingData(int profileIconIndex, string name, int score, int ranking)
    {
        _profileIconIndex = profileIconIndex;
        _name = name;
        _score = score;
        _ranking = ranking;
    }

    public int ProfileIconIndex { get => _profileIconIndex; }
    public string Name { get => _name; }
    public int Score { get => _score; }
    public int Rank { get => _ranking; }
}

public class RankingUI : SpawnableUI
{
    [SerializeField] Image _backgroundImg;
    [SerializeField] Image _profileImg;

    [SerializeField] TMP_Text _nameTxt;
    [SerializeField] TMP_Text _scoreTxt;
    [SerializeField] TMP_Text _rankTxt;
    [SerializeField] Color _selectedColor;

    RectTransform _rectTransform;

    public override void ChangeSelect(bool select)
    {
        if(select) _backgroundImg.color = _selectedColor;
        else _backgroundImg.color = Color.white;
    }

    public override void ChangeScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

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

        string rankFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Rank);
        _rankTxt.text = string.Format(rankFormat, rank);
    }
}
