#if UNITY_EDITOR
using UnityEditor;
#endif
public class ApplicationQuitSimulator
{
#if UNITY_EDITOR
    [MenuItem("Simulation/Simulate Application Quit")]
    public static void SimulateApplicationQuit()
    {
        // ģ��Ӧ�ó����˳�
        EditorApplication.isPlaying = false;
    }
#endif
}