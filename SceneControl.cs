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
        // 获取当前场景的名称
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 卸载当前场景
        SceneManager.UnloadSceneAsync(currentSceneName);

        // 加载当前场景
        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Single);
    }
}


