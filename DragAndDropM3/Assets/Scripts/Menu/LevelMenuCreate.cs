using System.Collections.Generic;
using UnityEngine;

public class LevelMenuCreate : MonoBehaviour
{
    [SerializeField] private List<WorldMenuType> levelMenuTypes = new List<WorldMenuType>();
    [SerializeField] private int levelCountsPerWorld = 5;
    [SerializeField] private float zOffset = 2.5f;
    [SerializeField] private float levelMenuTypeZSize = 13;
    [SerializeField, Range(3,10)] private int levelMenusCount = 3;
    [SerializeField] private GameObject levelMenuObjects;
    [SerializeField] private GameObject mainMenuObjects;
    private bool menuCreated;

    private void Awake() {
        ManagerGame.OnShowLevelMenu.AddListener(ShowLevelMenu);
    }

    private void ShowLevelMenu(bool _value) {
        levelMenuObjects.SetActive(_value);
        mainMenuObjects.SetActive(!_value);
        if (_value && !menuCreated) {
            Init();
        }
    }

    public void Init() {
        int openedLevel = SaveLoad.saveData.levelsOpened;
        int curLevelMenuType = (openedLevel - 1) / levelCountsPerWorld;
        int zOfsetPerLevel = (openedLevel - 1) % levelCountsPerWorld;

        for (int i = 0; i < levelMenusCount; i++) {
            int levelMenuType = curLevelMenuType + i - 1;
            int levelMenuTypeDiv = levelMenuType / levelMenuTypes.Count;
            int levelMenuTypeAfterCycle = levelMenuType - levelMenuTypeDiv * levelMenuTypes.Count;

            if (levelMenuTypeAfterCycle >= 0) {
                WorldMenuType wmt = Instantiate(levelMenuTypes[levelMenuTypeAfterCycle], levelMenuObjects.transform);
                wmt.transform.position = new Vector3(0, 0, (i - 1) * levelMenuTypeZSize - zOffset * zOfsetPerLevel);
                wmt.Init(levelMenuType * levelCountsPerWorld);
            }
        }
        menuCreated = true;
    }
}
