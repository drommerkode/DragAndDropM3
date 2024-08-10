using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int levelNum;
    [SerializeField] private int itemsVariants = 10;
    [SerializeField] private int levelType;
    [SerializeField] private List<GameObject> level = new List<GameObject>();
    [SerializeField] private int levelCountsPerWorld = 5;

    public void Init(ManagerUI _managerUI) {

        int openedLevel = SaveLoad.saveData.levelsOpened;
        int curLevelMenuType = (openedLevel - 1) / levelCountsPerWorld;

        //int levelMenuType = curLevelMenuType + i - 1;
        int levelMenuTypeDiv = curLevelMenuType / level.Count;
        int levelMenuTypeAfterCycle = curLevelMenuType - levelMenuTypeDiv * level.Count;

        levelNum = ManagerGame.instance.GetCurLevel();
        GameObject l = Instantiate(level[levelMenuTypeAfterCycle]);
        ManagerLevel mi = l.GetComponent<ManagerLevel>();
        mi.Init(itemsVariants, _managerUI);
        //ManagerGame.instance.LevelLoaded();
    }
}
