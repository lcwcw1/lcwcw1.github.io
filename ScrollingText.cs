using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 50f; // �ؽ�Ʈ �̵� �ӵ�
    private RectTransform rectTransform;
    private float width;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width; // �ؽ�Ʈ �ڽ��� �ʺ�
    }

    void Update()
    {
        // �ؽ�Ʈ�� �������� �̵�
        rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // ���� ��踦 ������ �����ʿ��� �ٽ� ��Ÿ���� ��
        if (rectTransform.anchoredPosition.x < -width)
        {
            rectTransform.anchoredPosition = new Vector2(width, rectTransform.anchoredPosition.y);
        }
    }
}
