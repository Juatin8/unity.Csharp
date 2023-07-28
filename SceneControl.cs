using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadPreviousScene()
    {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadSceneAsync(previousSceneIndex);
    }

    public static void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }

    public static void RestartScene()
    {
        // ��ȡ��ǰ����������
        string currentSceneName = SceneManager.GetActiveScene().name;

        // ж�ص�ǰ����
        SceneManager.UnloadSceneAsync(currentSceneName);

        // ���ص�ǰ����
        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Single);
    }
}


