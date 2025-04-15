using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopElementViewer
{
    TMP_Text _goldTxt;

    public TopElementViewer(TMP_Text goldTxt)
    {
        _goldTxt = goldTxt;
    }

    public void ChangeGoldCount(int goldCount)
    {
        _goldTxt.text = goldCount.ToString("N0");
    }
}
