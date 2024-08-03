using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumText;
    private int levelNum;
    private Button button;

    private void Awake() {
    }

    public void Init(int _levelNum) {
        button = GetComponent<Button>();
        levelNum = _levelNum;
        levelNumText.text = (levelNum + 1).ToString();
        if (levelNum >= SaveLoad.saveData.levelsOpened) {
            button.interactable = false;
        }
    }

    public void GoLevel() {
        ManagerScenes.GoLevel();
    }
}
