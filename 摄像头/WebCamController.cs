using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebCamController : MonoBehaviour
{
    private int currentCamIndex = 0;
    private WebCamTexture webCamTexture;
    public RawImage rawImage;

    public void Start()
    {
        WebCamDevice device = WebCamTexture.devices[currentCamIndex];
        if(!device.Equals(null))
        {
            int width = 1920;
            int height = 1920;

            webCamTexture = new WebCamTexture(device.name, width, height, 24);
            rawImage.texture = webCamTexture;
            webCamTexture.Play();
        }
    }

    public void SwapCameraClicked()
    {
        if(WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;
            if(webCamTexture != null)
            {
                StopCamera();
                StartStopCameraClicked();
            }

        }
    }

    public void StartStopCameraClicked()
    {
        if(webCamTexture != null)
        {
            StopCamera();
        }
        else
        {

            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            if(!device.Equals(null))
            {
                int width = 1920;
                int height = 1080;

                webCamTexture = new WebCamTexture(device.name, width, height, 24);
                rawImage.texture = webCamTexture;
                webCamTexture.Play();
            }
        }
    }

    private void StopCamera()
    {
        rawImage.texture = null;
        webCamTexture.Stop();
        webCamTexture = null;
    }

    public void CapturePhoto()
    {
        if(webCamTexture != null && webCamTexture.isPlaying)
        {
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            byte[] bytes = photo.EncodeToPNG();
            string filename = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            string filepath = Application.persistentDataPath + "/" + filename;

            System.IO.File.WriteAllBytes(filepath, bytes);

            Debug.Log("Photo saved to: " + filepath);
        }
    }

    public void CaptureScreenshot()
    {
        // ½ØÍ¼
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // ±£´æµ½±¾µØ
        byte[] bytes = screenshot.EncodeToPNG();
        string filename = "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filepath = Application.persistentDataPath + "/" + filename;
        System.IO.File.WriteAllBytes(filepath, bytes);

        Debug.Log("Screenshot saved to: " + filepath);
    }

}
