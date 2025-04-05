using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CustomSlider : Slider
{
    private float lastValue;

    protected override void Start()
    {
        base.Start();
        lastValue = value;
    }

    public Action<float> onHandlePointerUp { get; set; }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (lastValue != value)
        {
            Debug.Log($"Value changed to {value} on release!");
            // 여기서 애니메이션 실행, 사운드, 이벤트 등
            lastValue = value;
            onHandlePointerUp?.Invoke(lastValue);
        }
    }
}
