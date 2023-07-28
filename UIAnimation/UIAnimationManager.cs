using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimationManager : MonoBehaviour
{
    public static IEnumerator Fade(GameObject feedbackObject)      // ������Ϲҵ�ͼƬ����ʧ�Ľű�
    {
        yield return new WaitForSeconds(0.2f);
        float fadeTime = 0.5f;
        Color initialColor = feedbackObject.GetComponent<Image>().color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        float t = 0f;
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeTime);
            feedbackObject.GetComponent<Image>().color = Color.Lerp(initialColor, targetColor, normalizedTime);
            yield return null;  // ��ÿһ֡����ʱ��ͣһ��
        }
        feedbackObject.SetActive(false); //�������
        feedbackObject.GetComponent<Image>().color = initialColor; //�ָ�ԭ����ɫ
    }

    public static void FadeIn(GameObject feedbackObject) //������Ϲҵ�ͼƬ�𽥳��ֵĽű�
    {
        feedbackObject.SetActive(true);
        feedbackObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        feedbackObject.GetComponent<Image>().DOFade(1f, 0.5f);
    }


    public static void Popup(GameObject feedbackObject) //���������Ч��
    {
        feedbackObject.GetComponent<RectTransform>().localScale = Vector3.zero;  //�������С��0
        feedbackObject.GetComponent<RectTransform>().DOScale(1, 0.2f).SetEase(Ease.InOutBounce);
        feedbackObject.GetComponent<RectTransform>().DOScale(2, 0.2f).SetEase(Ease.InOutBounce);
    }

    public static void Shrink(GameObject feedbackObject) //�������СЧ��
    {
        feedbackObject.GetComponent<RectTransform>().DOScale(0f, 0.5f).SetEase(Ease.InOutBounce);
    }
    public static void ShrinkAndFade(GameObject gb) //�������СЧ��
    {
        float duration = 0.5f;
        var canvasGroup = gb.GetComponent<CanvasGroup>();
        var rectTransform = gb.GetComponent<RectTransform>();
        var tween = DOTween.Sequence();
        tween.Join(canvasGroup.DOFade(0, duration));
        tween.Join(rectTransform.DOScale(0, duration));
        tween.Play();
    }

    public static void Bounce(GameObject gb)  //������ͻȻ�Ŵ�һ���ٸ�ԭ
    {
        gb.GetComponent<RectTransform>().DOScale(15f, 0.5f);
        gb.GetComponent<RectTransform>().DOScale(1, 0.5f);
    }

}