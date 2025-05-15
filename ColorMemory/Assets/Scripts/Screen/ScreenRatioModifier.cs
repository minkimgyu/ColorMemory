using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class ScreenRatioModifier : MonoBehaviour
{
    CanvasScaler _canvasScaler;

    // Start is called before the first frame update
    void Start()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
        Vector2 resolution = _canvasScaler.referenceResolution;

        float referenceResolutionRatio = resolution.x / resolution.y;
        float currentRatio = (float)Screen.width / (float)Screen.height; // 가로 / 세로

        // 화면비를 가져와서 IPhone 15 pro보다 세로 비율이 길면 0, 가로 비율이 길면 1로 값 주기
        if (currentRatio > referenceResolutionRatio) // 가로로 더 긴 경우
        {
            _canvasScaler.matchWidthOrHeight = 1;
        }
        else // 세로로 더 긴 경우
        {
            _canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
