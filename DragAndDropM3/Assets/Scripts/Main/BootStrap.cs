using System.Collections;
using UnityEngine;

public class BootStrap : MonoBehaviour {

    [SerializeField] private ManagerUI managerUI;

    [SerializeField] private LevelLoader levelLoader;

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

        managerUI?.Init();
        levelLoader?.Init();
    }
}
