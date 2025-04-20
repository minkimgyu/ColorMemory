using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomProgressUI : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    float _fillValue = 0;
    public float FillValue { set { _fillValue = value; } }

    [SerializeField] Image _circleFillImage;
    [SerializeField] RectTransform _handlerEdgeImage;
    [SerializeField] RectTransform _fillHandler;

    void OnValidate()
    {
        // ������ �󿡼� ���� ����Ǿ��� �� �����
        FillCircleValue(_fillValue);
    }

    // Update is called once per frame
    void Update()
    {
        FillCircleValue(_fillValue);
    }

    void FillCircleValue(float value)
    {
        if (_circleFillImage == null || _handlerEdgeImage == null || _fillHandler == null)
            return;

        if (value == 0)
        {
            _handlerEdgeImage.gameObject.SetActive(false);
            _fillHandler.gameObject.SetActive(false);
        }
        else
        {
            _handlerEdgeImage.gameObject.SetActive(true);
            _fillHandler.gameObject.SetActive(true);
        }

        _circleFillImage.fillAmount = value;

        // �ݽð� �������� ������ �����ϹǷ� ������ ����� ȸ��
        float angle = value * 360f;
        _fillHandler.localEulerAngles = new Vector3(0, 0, angle); // �ð� ���� -> �ݽð� �������� ����
        _handlerEdgeImage.localEulerAngles = new Vector3(0, 0, -angle); // ������ ���� �ݴ� ����
    }
}
