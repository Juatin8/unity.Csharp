using UnityEngine;
using DG.Tweening;

public class MoveUIElements : MonoBehaviour
{
    RectTransform[] uiElements;
    public int currentBig;
    public float ScaleFactor;
    private float averageDistance;

    public void Start()
    {
        uiElements = GetComponentsInChildren<RectTransform>();  //获取所有UI元素的RectTransform组件
        uiElements[currentBig].DOScale(ScaleFactor, 0.5f);     //缩放物体
        averageDistance=calculateAveDis();                     //计算平均间距
    }

    public void MoveLeft()
    {
        foreach(RectTransform element in uiElements)  //遍历所有UI元素并向左移动它们
        {
            element.DOAnchorPosX(element.anchoredPosition.x - averageDistance, 0.5f);
        }
        uiElements[currentBig].DOScale(1/ScaleFactor, 0.5f);
        currentBig++;
        uiElements[currentBig].DOScale(ScaleFactor, 0.5f);
        //Debug.Log(uiElements.Length);
    }

    public void MoveRight()
    {
        foreach(RectTransform element in uiElements)     //遍历所有UI元素并向左移动它们
        {
            element.DOAnchorPosX(element.anchoredPosition.x + averageDistance, 0.5f);
        }
        uiElements[currentBig].DOScale(1 / ScaleFactor, 0.5f);
        currentBig--;
        uiElements[currentBig].DOScale(ScaleFactor, 0.5f);
    }

    private float calculateAveDis()
    {
        //计算UI元素之间的平均距离
        float totalDistance = 0f;
        for(int i = 1; i < uiElements.Length; i++)
        {
            totalDistance += Mathf.Abs(uiElements[i].anchoredPosition.x - uiElements[i - 1].anchoredPosition.x);
        }
        float averageDistance = totalDistance / (uiElements.Length - 1);
        return averageDistance;
    }
}
