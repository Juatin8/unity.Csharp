using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public static void ChangeAlpha(GameObject gb, float alpha)   //�ı�ͼƬ͸����
    {
        if(alpha < 0) { alpha = 0; }
        if(alpha > 1) { alpha = 1; }
        gb.GetComponent<Image>().color = new Color(255, 255, 255, alpha);
    }
}
