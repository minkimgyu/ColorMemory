using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPatternUI : SpawnableUI
{
    const int maxSize = 5;
    [SerializeField] Transform _content;
    GameObject[,] _dots;

    public override void Initialize(MapData data, Color[] pickColors)
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
