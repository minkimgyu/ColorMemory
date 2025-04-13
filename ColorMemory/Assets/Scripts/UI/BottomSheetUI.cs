using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BottomSheetUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform panel;
    public float slideDuration = 0.3f;
    public float speedThreshold = 500f; // 빠르게 드래그 시 즉시 닫히는 속도
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isOpen = false;
    private float dragSpeed = 0f;

    [SerializeField] float _hiddenPositionOffset = 522;
    [SerializeField] float _visiblePoistionOffset = 450;

    void Start()
    {
        float panelHeight = panel.rect.height;
        hiddenPosition = new Vector2(0, -panelHeight + _hiddenPositionOffset); // 화면 아래로 숨김
        visiblePosition = new Vector2(0, -_visiblePoistionOffset); // 화면 안으로 보임
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
    }

    private void ClosePanel()
    {
        panel.DOAnchorPos(hiddenPosition, slideDuration);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 속도 계산
        dragSpeed = eventData.delta.y / Time.deltaTime;

        // 패널 이동 (위로 당기면 열리고, 아래로 밀면 닫힘)
        float newY = panel.anchoredPosition.y + eventData.delta.y;
        panel.anchoredPosition = new Vector2(0, Mathf.Clamp(newY, hiddenPosition.y, visiblePosition.y));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(dragSpeed);

        if (dragSpeed > speedThreshold)
        {
            OpenPanel();
            isOpen = true;
            return;
        }

        // 빠른 드래그 시 즉시 닫힘
        else if (dragSpeed < -speedThreshold)
        {
            ClosePanel();
            isOpen = false;
            return;
        }

        // 패널이 화면의 반 이상 내려가면 닫기, 그렇지 않으면 다시 열기
        float midPoint = (hiddenPosition.y + visiblePosition.y) / 2;

        // 패널이 화면의 반 이상 내려가면 닫기, 그렇지 않으면 다시 열기
        if (panel.anchoredPosition.y < midPoint)
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
