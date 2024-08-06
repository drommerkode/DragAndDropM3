using System.Collections;
using UnityEngine;

public class BootStrap : MonoBehaviour {

    [SerializeField] private ManagerUI managerUI;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private LevelMenuCreate levelMenuCreate;

    private void Start() {
        StartCoroutine(BootCoroutine());
    }

    private IEnumerator BootCoroutine() {
        yield return new WaitForSecondsRealtime(ManagerGame.instance.GetBootDelayTime());

        GameObject fgo;

        if (managerUI == null) {
            fgo = GameObject.FindGameObjectWithTag(Global.TAG_ManagerUI);
            fgo?.TryGetComponent<ManagerUI>(out managerUI);
        }

        if (levelLoader == null) {
            fgo = GameObject.FindGameObjectWithTag(Global.TAG_LevelLoader);
            fgo?.TryGetComponent<LevelLoader>(out levelLoader);
        }

        if (levelMenuCreate == null) {
            fgo = GameObject.FindGameObjectWithTag(Global.TAG_LevelMenuCreate);
            fgo?.TryGetComponent<LevelMenuCreate>(out levelMenuCreate);
        }

        managerUI?.Init();
        levelLoader?.Init(managerUI);
        //levelMenuCreate?.Init();
    }
}
