using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerItem : MonoBehaviour
{

    [SerializeField] private int allCount = 20;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnTime = 0.5f;
    [SerializeField] private Item item;
    [SerializeField] private ParticleSystem partCreate;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<ItemConfiguration> itemConfs = new List<ItemConfiguration>();
    [SerializeField] private List<ItemConfiguration> spawnConfs = new List<ItemConfiguration>();
    [SerializeField] private int curCount = 0;

    private void Start() {
        SpawnItems();
    }

    private void SpawnItems() {
        int curSpawnConf = 0;
        int itenConfNum;
        spawnConfs.Clear();
        while (curSpawnConf < spawnCount) {
            itenConfNum = Random.Range(0, itemConfs.Count);
            spawnConfs.Add(itemConfs[itenConfNum]);
            spawnConfs.Add(itemConfs[itenConfNum]);
            curSpawnConf += 2;
        }
        Shuffle(spawnConfs);
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine() {
        WaitForSeconds wfs = new WaitForSeconds(spawnTime);
        int pointNum;
        for (int i = 0; i < spawnCount; i++) {
            pointNum = Random.Range(0, spawnPoints.Count);
            Item itm = Instantiate(item, spawnPoints[pointNum].position, Quaternion.identity);
            itm.itemConfiguration = spawnConfs[i];
            itm.managerItem = this;
            itm.Spawn();
            curCount++;
            
            Instantiate(partCreate, spawnPoints[pointNum].position, Quaternion.identity);
            
            yield return wfs;
        }
    }

    public void DestroyItem() {
        allCount--;
        curCount--;
        if (allCount > 0) {
            if (curCount <= 0) {
                SpawnItems();
            }
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
