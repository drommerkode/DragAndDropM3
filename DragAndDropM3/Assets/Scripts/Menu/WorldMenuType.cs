using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuType : MonoBehaviour
{
    [SerializeField] private List<LevelButton> levelButtons = new List<LevelButton>();
    private int startLevelNum;

    public void Init(int _startLevelNum) {
        startLevelNum = _startLevelNum;
        for (int i = 0; i < levelButtons.Count; i++) {
            levelButtons[i].Init(startLevelNum + i + 1);
        }
    }
}
