using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimationManager : MonoBehaviour
{
    public static IEnumerator Fade(GameObject feedbackObject)      // 让物件上挂的图片逐渐消失的脚本
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
            yield return null;  // 在每一帧结束时暂停一下
        }
        feedbackObject.SetActive(false); //隐藏物件
        feedbackObject.GetComponent<Image>().color = initialColor; //恢复原来颜色
    }

    public static void FadeIn(GameObject feedbackObject) //让物件上挂的图片逐渐出现的脚本
    {
        feedbackObject.SetActive(true);
        feedbackObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        feedbackObject.GetComponent<Image>().DOFade(1f, 0.5f);
    }


    public static void Popup(GameObject feedbackObject) //让物件弹出效果
    {
        feedbackObject.GetComponent<RectTransform>().localScale = Vector3.zero;  //将物件缩小到0
        feedbackObject.GetComponent<RectTransform>().DOScale(1, 0.2f).SetEase(Ease.InOutBounce);
        feedbackObject.GetComponent<RectTransform>().DOScale(2, 0.2f).SetEase(Ease.InOutBounce);
    }

    public static void Shrink(GameObject feedbackObject) //让物件缩小效果
    {
        feedbackObject.GetComponent<RectTransform>().DOScale(0f, 0.5f).SetEase(Ease.InOutBounce);
    }
    public static void ShrinkAndFade(GameObject gb) //让物件缩小效果
    {
        float duration = 0.5f;
        var canvasGroup = gb.GetComponent<CanvasGroup>();
        var rectTransform = gb.GetComponent<RectTransform>();
        var tween = DOTween.Sequence();
        tween.Join(canvasGroup.DOFade(0, duration));
        tween.Join(rectTransform.DOScale(0, duration));
        tween.Play();
    }

    public static void Bounce(GameObject gb)  //让物体突然放大一下再复原
    {
        gb.GetComponent<RectTransform>().DOScale(15f, 0.5f);
        gb.GetComponent<RectTransform>().DOScale(1, 0.5f);
    }

}