using UnityEngine.SceneManagement;

namespace VFX.Tools
{
    public static class SceneTransition
    {
        public static void ChangeScene(int index) => SceneManager.LoadScene(index);
        public static void ChangeScene(string levelName) => SceneManager.LoadScene(levelName);
        public static void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}