using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearPatternUI : SpawnableUI
{
    const int maxSize = 5;
    [SerializeField] TMP_Text _stageTxt;
    [SerializeField] Transform _content;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;
    GameObject[,] _dots;

    public override void Initialize(int currentStageCount, MapData data, Color[] pickColors) 
    {
        _stageTxt.text = currentStageCount.ToString();
        ChangePattern(data, pickColors);
    }


    public override void Initialize(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors)
    {
        _stageTxt.text = $"{currentStageCount}/{totalStageCount}";
        ChangePattern(data, pickColors);
    }

    void ChangePattern(MapData data, Color[] pickColors)
    {
        _dots = new GameObject[maxSize, maxSize];
        for (int i = 0; i < maxSize * maxSize; i++)
        {
            Transform child = _content.GetChild(i);
            int maxCol = i % maxSize;
            int maxRow = i / maxSize;
            _dots[maxRow, maxCol] = child.gameObject;
            child.gameObject.SetActive(false);
        }

        int row = data.DotColor.GetLength(0);
        int height = data.DotColor.GetLength(1);

        _gridLayoutGroup.constraintCount = row;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int colorIndex = data.DotColor[i, j];

                _dots[i, j].gameObject.SetActive(true);
                _dots[i, j].GetComponent<Image>().color = pickColors[colorIndex];
            }
        }
    }
}
