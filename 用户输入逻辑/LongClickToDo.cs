using UnityEngine;
using UnityEngine.EventSystems;

public class LongClickToDo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float downTime = 0;
    private bool isDragging = false;
    private float timelimit =0.8f;

    void Update()
    {
        if(isDragging){
            // 长按xx秒
            if(Time.time - downTime > timelimit)
            {
                isDragging = false;
                // 你要处理的逻辑
                StateController.Inst.SetConstructState((int)EnumConstructState.Construct);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        downTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        downTime = 0;
    }
}
