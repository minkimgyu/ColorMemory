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
        float currentRatio = (float)Screen.width / (float)Screen.height; // ���� / ����

        // ȭ��� �����ͼ� IPhone 15 pro���� ���� ������ ��� 0, ���� ������ ��� 1�� �� �ֱ�
        if (currentRatio > referenceResolutionRatio) // ���η� �� �� ���
        {
            _canvasScaler.matchWidthOrHeight = 1;
        }
        else // ���η� �� �� ���
        {
            _canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
