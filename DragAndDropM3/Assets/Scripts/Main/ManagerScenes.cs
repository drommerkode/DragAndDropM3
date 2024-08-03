using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScenes : MonoBehaviour {
    private static AsyncOperation loadingOperation;
    private static string sceneLogo = "Logo";
    private static string sceneMainMenu = "Menu";
    private static string sceneLevel = "Level";

    public static void SceneRestart() {
        SceneLoading(SceneManager.GetActiveScene().name);
    }

    public static void GoMainMenu() {
        if (SceneManager.GetActiveScene().name != sceneMainMenu) {
            SceneLoading(sceneMainMenu);
        }
    }

    public static void GoLevel() {
        SceneLoading(sceneLevel);
    }

    public static bool GetIsLogo() {
        return SceneManager.GetActiveScene().name == sceneLogo ? true : false;
    }

    public static bool GetIsMainMenu() {
        return SceneManager.GetActiveScene().name == sceneMainMenu ? true : false;
    }

    public static bool GetIsLevel() {
        return SceneManager.GetActiveScene().name == sceneLevel ? true : false;
    }

    public static void SceneLoading(string _sceneName) {
        ManagerGame.instance.SetPause(true);
        ManagerGame.instance.SceneLoading();
        loadingOperation = SceneManager.LoadSceneAsync(_sceneName);
    }
}
