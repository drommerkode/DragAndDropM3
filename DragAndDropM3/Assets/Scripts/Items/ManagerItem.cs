using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManagerItem : MonoBehaviour
{

    [SerializeField] private int allVariants = 20;
    [SerializeField] private Item item;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<ItemConfiguration> itemConfs = new List<ItemConfiguration>();
    [SerializeField] private List<ItemConfiguration> spawnConfs = new List<ItemConfiguration>();
    [SerializeField] private int curCount = 0;
    private int curScore;

    private CameraReact cameraReact;
    private ManagerUI managerUI;

    public void Init(int _allVariants, ManagerUI _managerUI) {
        cameraReact = Camera.main.GetComponent<CameraReact>();

        allVariants = _allVariants;
        managerUI = _managerUI;

        managerUI.SetInGameScore(0);

        allVariants = Mathf.Clamp(allVariants, 0, itemConfs.Count);
        SpawnItems();
    }

    private void SpawnItems() {

        Shuffle(itemConfs);
        int spawnCOunt = allVariants * 2;
        for (int i = 0; i < allVariants; i++) {
            spawnConfs.Add(itemConfs[i]);
            spawnConfs.Add(itemConfs[i]);
        }
        Shuffle(spawnConfs);

        int pointNum;
        for (int i = 0; i < spawnCOunt; i++) {
            pointNum = Random.Range(0, spawnPoints.Count);
            Item itm = Instantiate(item, spawnPoints[pointNum].position, Quaternion.identity);
            itm.itemConfiguration = spawnConfs[i];
            itm.managerItem = this;
            itm.Spawn();
            curCount++;
        }
        ManagerGame.instance.LevelLoaded();
    }

    public void DestroyItem() {
        cameraReact.React();
        curScore = curScore + 1;// + mod
        managerUI.SetInGameScore(curScore);
        managerUI.ScoreReactInGame();

        curCount--;
        if (curCount <= 0) {
            SaveLoad.saveData.score += curScore;
            ManagerGame.instance.LevelCompleted();
        }
    }

    public void Shuffle<T>(List<T> list) {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
