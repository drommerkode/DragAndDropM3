using System.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int levelNum;
    [SerializeField] private int itemsVariants = 10;
    [SerializeField] private int levelType;
    [SerializeField] private GameObject level;

    public void Init(ManagerUI _managerUI) {
        levelNum = ManagerGame.instance.GetCurLevel();
        GameObject l = Instantiate(level);
        ManagerItem mi = l.GetComponent<ManagerItem>();
        mi.Init(itemsVariants, _managerUI);
        //ManagerGame.instance.LevelLoaded();
    }
}
