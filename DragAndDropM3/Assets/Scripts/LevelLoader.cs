using System.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int levelNum;
    [SerializeField] private int itemsVariants = 10;
    [SerializeField] private LevelType levelType;
    [SerializeField] private GameObject level;

    private enum LevelType {
        spring,
        sand,
        summer,
        volcano,
        fall,
        candy,
        winter,
        castle
    }

    public void Init(ManagerUI _managerUI) {
        GameObject l = Instantiate(level);
        ManagerItem mi = l.GetComponent<ManagerItem>();
        mi.Init(itemsVariants, _managerUI);
        //ManagerGame.instance.LevelLoaded();
    }
}
