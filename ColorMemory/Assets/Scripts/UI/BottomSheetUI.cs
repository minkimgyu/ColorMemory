using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BottomSheetUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform panel;
    public float slideDuration = 0.3f;
    public float speedThreshold = 500f; // ������ �巡�� �� ��� ������ �ӵ�
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isOpen = false;
    private float dragSpeed = 0f;

    [SerializeField] float _hiddenPositionOffset = 522;
    [SerializeField] float _visiblePoistionOffset = 450;

    void Start()
    {
        float panelHeight = panel.rect.height;
        hiddenPosition = new Vector2(0, -panelHeight + _hiddenPositionOffset); // ȭ�� �Ʒ��� ����
        visiblePosition = new Vector2(0, -_visiblePoistionOffset); // ȭ�� ������ ����
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
        // �巡�� �ӵ� ���
        dragSpeed = eventData.delta.y / Time.deltaTime;

        // �г� �̵� (���� ���� ������, �Ʒ��� �и� ����)
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

        // ���� �巡�� �� ��� ����
        else if (dragSpeed < -speedThreshold)
        {
            ClosePanel();
            isOpen = false;
            return;
        }

        // �г��� ȭ���� �� �̻� �������� �ݱ�, �׷��� ������ �ٽ� ����
        float midPoint = (hiddenPosition.y + visiblePosition.y) / 2;

        // �г��� ȭ���� �� �̻� �������� �ݱ�, �׷��� ������ �ٽ� ����
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
