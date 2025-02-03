using UnityEditor;
using UnityEngine;
using System.IO;

public class FullScreenScreenshot : EditorWindow
{
    [MenuItem("MyTools/FullScreen Screenshot")]
    public static void CaptureScreenshot()
    {
        // 设置截图保存路径
        string savePath = Path.Combine(Application.persistentDataPath, "Screenshots");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // 设置文件保存路径，文件名为当前时间
        string filePath = Path.Combine(savePath, "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");

        // 使用 ScreenCapture 捕捉当前屏幕并保存为 PNG 文件
        ScreenCapture.CaptureScreenshot(filePath);

        // 打印截图路径
        Debug.Log("Screenshot saved to: " + filePath);
    }
}
