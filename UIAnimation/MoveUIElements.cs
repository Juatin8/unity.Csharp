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
        uiElements = GetComponentsInChildren<RectTransform>();  //��ȡ����UIԪ�ص�RectTransform���
        uiElements[currentBig].DOScale(ScaleFactor, 0.5f);     //��������
        averageDistance=calculateAveDis();                     //����ƽ�����
    }

    public void MoveLeft()
    {
        foreach(RectTransform element in uiElements)  //��������UIԪ�ز������ƶ�����
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
        foreach(RectTransform element in uiElements)     //��������UIԪ�ز������ƶ�����
        {
            element.DOAnchorPosX(element.anchoredPosition.x + averageDistance, 0.5f);
        }
        uiElements[currentBig].DOScale(1 / ScaleFactor, 0.5f);
        currentBig--;
        uiElements[currentBig].DOScale(ScaleFactor, 0.5f);
    }

    private float calculateAveDis()
    {
        //����UIԪ��֮���ƽ������
        float totalDistance = 0f;
        for(int i = 1; i < uiElements.Length; i++)
        {
            totalDistance += Mathf.Abs(uiElements[i].anchoredPosition.x - uiElements[i - 1].anchoredPosition.x);
        }
        float averageDistance = totalDistance / (uiElements.Length - 1);
        return averageDistance;
    }
}
