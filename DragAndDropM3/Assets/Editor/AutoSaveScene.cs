using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoSaveScene : MonoBehaviour
{
    static AutoSaveScene() {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}
