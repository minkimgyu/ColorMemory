using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBundleUI : SpawnableUI
{
    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _descriptionTxt;
    [SerializeField] TMP_Text _rewardTxt;
    [SerializeField] TMP_Text _priceTxt;
    [SerializeField] Button _buyBtn;
    private string _productId;
    public string ProductId => _productId; 

    public override void InjectClickEvent(System.Action OnClick) 
    {
        _buyBtn.onClick.AddListener(() => { OnClick?.Invoke(); ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); });
    }

    public override void Initialize(string title, string description, int reward, int price, string productId) 
    {
        _titleTxt.text = title;
        _descriptionTxt.text = description;
        _rewardTxt.text = reward.ToString("N0");

        string buyFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundleBuyInfo);
        _priceTxt.text = string.Format(buyFormat, price);
        _productId = productId;
    }
}
