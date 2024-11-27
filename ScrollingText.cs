using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 50f; // 텍스트 이동 속도
    private RectTransform rectTransform;
    private float width;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width; // 텍스트 박스의 너비
    }

    void Update()
    {
        // 텍스트를 왼쪽으로 이동
        rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // 왼쪽 경계를 넘으면 오른쪽에서 다시 나타나게 함
        if (rectTransform.anchoredPosition.x < -width)
        {
            rectTransform.anchoredPosition = new Vector2(width, rectTransform.anchoredPosition.y);
        }
    }
}
