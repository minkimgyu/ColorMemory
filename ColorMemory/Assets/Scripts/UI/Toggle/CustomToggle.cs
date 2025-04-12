using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : Toggle
{
    protected override void Awake()
    {
        base.Awake();
        // 초기 상태 반영
        ApplyToggleState(isOn);
        onValueChanged.AddListener(ApplyToggleState);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        ApplyToggleState(isOn);
    }
#endif

    private void ApplyToggleState(bool value)
    {
        if (graphic == null) return;

        graphic.gameObject.SetActive(value);
        for (int i = 0; i < graphic.transform.childCount; i++)
        {
            graphic.transform.GetChild(i).gameObject.SetActive(value);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onValueChanged.RemoveListener(ApplyToggleState);
    }
}