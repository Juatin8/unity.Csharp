using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class LongPressDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float longPressDuration = 0.6f;
    private bool isLongPressing = false;
    private bool isDragging = false;

    private Image image;
    private RectTransform rectTransform;

    private Vector2 originalPosition;

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isLongPressing = true;
        StartCoroutine(LongPressCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressing = false;
        isDragging = false;
        image.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition + eventData.delta;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, currentPosition.y);
        }
    }

    private IEnumerator LongPressCoroutine()
    {
        yield return new WaitForSeconds(longPressDuration);

        if(isLongPressing)
        {
            isDragging = true;
            image.raycastTarget = false;
        }
    }
}
