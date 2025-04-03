using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideSheetUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform panel;
    public float slideDuration = 0.3f;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isOpen = false;

    private float dragSpeed = 0f;
    public float closeSpeedThreshold = 300f; // 빠르게 드래그할 때 즉시 닫히는 속도

    public System.Action<bool> OnPanelActivated { get; set; }

    public void InjectActivateEvent(System.Action<bool> OnPanelActivated)
    {
        this.OnPanelActivated = OnPanelActivated;
    }

    public virtual void Initialize()
    {
        float panelWidth = panel.rect.width;
        hiddenPosition = new Vector2(panelWidth, 0);
        visiblePosition = Vector2.zero;
        panel.anchoredPosition = hiddenPosition;
    }

    public void TogglePanel()
    {
        if (isOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
        isOpen = !isOpen;
    }

    private void OpenPanel()
    {
        panel.DOAnchorPos(visiblePosition, slideDuration);
        OnPanelActivated?.Invoke(true);
    }

    private void ClosePanel()
    {
        panel.DOAnchorPos(hiddenPosition, slideDuration);
        OnPanelActivated?.Invoke(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 속도 계산
        dragSpeed = eventData.delta.x / Time.deltaTime;

        // 사용자가 오른쪽으로 드래그하여 닫을 경우
        float newX = panel.anchoredPosition.x + eventData.delta.x;
        panel.anchoredPosition = new Vector2(Mathf.Clamp(newX, 0, hiddenPosition.x), 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(dragSpeed);

        // 빠른 드래그 시 즉시 닫힘
        if (dragSpeed > closeSpeedThreshold)
        {
            ClosePanel();
            isOpen = false;
            return;
        }

        // 일정 거리 이상 밀었으면 닫기
        if (panel.anchoredPosition.x > panel.rect.width / 2)
        {
            ClosePanel();
            isOpen = false;
        }
        else
        {
            OpenPanel();
            isOpen = true;
        }
    }
}
