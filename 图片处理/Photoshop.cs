using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public static class Photoshop 
{
    public static void SetTransparency(Image targetImage,float alpha)
    {
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = Mathf.Clamp01(alpha); // 确保 alpha 在 0 到 1 之间
            targetImage.color = color;
        }
    }

      public static Texture2D ConvertToGrayscale(Texture2D colorTexture)
    {
        Texture2D grayscaleTexture = new Texture2D(colorTexture.width, colorTexture.height);
        for (int y = 0; y < colorTexture.height; y++)
        {
            for (int x = 0; x < colorTexture.width; x++)
            {
                Color color = colorTexture.GetPixel(x, y);
                float gray = color.grayscale;
                grayscaleTexture.SetPixel(x, y, new Color(gray, gray, gray));
            }
        }
        grayscaleTexture.Apply();
        return grayscaleTexture;
    }

public static void SaveAsPNG(Texture2D texture,String path)
{       byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
}

public static void ScaleUniform(Image image,float scaleFactor)
      {  RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
      }
}
