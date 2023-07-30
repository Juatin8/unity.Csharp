#if UNITY_EDITOR
using UnityEditor;
#endif
public class ApplicationQuitSimulator
{
#if UNITY_EDITOR
    [MenuItem("Simulation/Simulate Application Quit")]
    public static void SimulateApplicationQuit()
    {
        // 模拟应用程序退出
        EditorApplication.isPlaying = false;
    }
#endif
}