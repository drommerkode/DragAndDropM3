using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    [SerializeField] private bool autoLevelNum = true;
    [SerializeField] private int levelNum;
    [SerializeField] private bool autoItemsVariants = true;
    [SerializeField] private int itemsVariants = 10;
    [SerializeField] private int levelType;
    [SerializeField] private List<GameObject> level = new List<GameObject>();
    [SerializeField] private int levelCountsPerWorld = 5;

    public void Init(ManagerUI _managerUI) {

        if (autoLevelNum) {
            //levelNum = SaveLoad.saveData.levelsOpened;
            levelNum = ManagerGame.instance.GetCurLevel();
        }
        int curLevelMenuType = (levelNum - 1) / levelCountsPerWorld;

        //int levelMenuType = curLevelMenuType + i - 1;
        int levelMenuTypeDiv = curLevelMenuType / level.Count;
        int levelMenuTypeAfterCycle = curLevelMenuType - levelMenuTypeDiv * level.Count;

        if (autoItemsVariants) {
            itemsVariants = 4 + levelNum * 2;
        }

        GameObject l = Instantiate(level[levelMenuTypeAfterCycle]);
        ManagerLevel mi = l.GetComponent<ManagerLevel>();
        mi.Init(itemsVariants, _managerUI);
        //ManagerGame.instance.LevelLoaded();
    }
}
