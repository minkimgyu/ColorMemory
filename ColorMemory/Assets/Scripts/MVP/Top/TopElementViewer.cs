using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopElementViewer
{
    TMP_Text _goldTxt;
    Button _homeBtn;
    Button _shopBtn;
    Button _rankingBtn;
    Button _settingBtn;

    public TopElementViewer(
        TMP_Text goldTxt,
        Button homeBtn,
        Button shopBtn,
        Button rankingBtn,
        Button settingBtn,
        TopElementPresenter presenter)
    {
        _goldTxt = goldTxt;

        _homeBtn = homeBtn;
        _homeBtn.onClick.AddListener(() => { presenter.OnClickHomeBtn?.Invoke(); });

        _shopBtn = shopBtn;
        _shopBtn.onClick.AddListener(() => { presenter.OnClickShopBtn?.Invoke(); });

        _rankingBtn = rankingBtn;
        _rankingBtn.onClick.AddListener(() => { presenter.OnClickRankingBtn?.Invoke(); });

        _settingBtn = settingBtn;
        _settingBtn.onClick.AddListener(() => { presenter.OnClickSettingBtn?.Invoke(); });
    }

    public void ChangeGoldCount(int goldCount)
    {
        _goldTxt.text = goldCount.ToString("N0");
    }
}
