using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnlockController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;


    public UnityEvent<Vector2> onDragEvent;
    public UnityEvent onEndDragFailEvent;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public Vector2 Delta { get; private set; }

    public void OnDrag(PointerEventData eventData)
    {
        Delta = eventData.delta;
        Vector2 currentPosition = rectTransform.anchoredPosition + Delta;
        rectTransform.anchoredPosition = new Vector2(currentPosition.x, originalPosition.y);

        // 触发事件，将增量传递给订阅者
        onDragEvent.Invoke(Delta);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        float distance = Vector2.Distance(rectTransform.anchoredPosition, originalPosition);
        if(distance > 500f)
        {
            Debug.Log("Unlocked!");
            SceneControl.LoadNextScene();
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;

            onEndDragFailEvent.Invoke();
        }
    }
}
