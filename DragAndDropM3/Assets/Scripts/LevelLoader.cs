using System.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public void Init()
    {
        /*GameObject[] levels = ManagerGame.instance.GetLevels();
        if (levels.Length == 0) { return; }
        int ln = Mathf.Clamp(ManagerGame.instance.GetCurLevel(), 0, levels.Length - 1);
        ManagerGame.instance.SetCurLevel(ln);
        GameObject newLevel = Instantiate(levels[ln]);
        LevelStarter ls = newLevel.GetComponent<LevelStarter>();
        StartCoroutine(LevelStartCoroutine(ls));*/
    }

    private IEnumerator LevelStartCoroutine(LevelStarter _ls) {
        yield return new WaitForSecondsRealtime(ManagerGame.instance.GetBootDelayTime());
        _ls.Init();
    }
}
