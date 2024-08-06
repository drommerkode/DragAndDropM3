using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumText;
    [SerializeField] private GameObject levelLock;
    [SerializeField] private GameObject button;
    private int levelNum;
    private bool isLevelOpen;

    public void Init(int _levelNum) {
        int levelsOpened = SaveLoad.saveData.levelsOpened;
        levelNum = _levelNum;
        if (levelNum < levelsOpened) {
            button.SetActive(false);
        }
        else {
            if (levelNum == levelsOpened) {
                isLevelOpen = true;
                levelLock.SetActive(false);
                levelNumText.gameObject.SetActive(true);
                levelNumText.text = levelNum.ToString();
            }
            else {
                levelLock.SetActive(true);
                levelNumText.gameObject.SetActive(false);
            }
        }
    }

    public void GoLevel() {
        if (!isLevelOpen) { return; }
        ManagerGame.instance.SetCurLevel(levelNum);
        ManagerScenes.GoLevel();
    }
}
