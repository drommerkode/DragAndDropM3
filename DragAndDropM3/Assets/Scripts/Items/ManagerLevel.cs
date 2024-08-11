using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static SaveLoad;

public class ManagerLevel : MonoBehaviour
{
    [Header("End level")]
    [SerializeField] private float endLevelDelay = 1f;

    [Header("Merge")]
    [SerializeField] private int mergeScore = 2;
    [SerializeField] private ScoreFly scoreFly;

    [Header("Items")]
    [SerializeField] private int allVariants = 20;
    [SerializeField] private Item item;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<ItemConfiguration> itemConfs = new List<ItemConfiguration>();
    private List<ItemConfiguration> spawnConfs = new List<ItemConfiguration>();
    private int curCount = 0;

    [Header("Multiplier")]
    [SerializeField] private int maxScoreMultiplier = 8;
    [SerializeField] private float minSpeedScoreMultiplier = 0.2f;
    [SerializeField] private float speedScoreMultiplierAdd = 0.1f;
    private int curScoreMultiplier = 1;
    private float curTimeScoreMultiplier = 1;
    private float curSpeedScoreMultiplier;
    private IEnumerator scoreMultiplierCoroutine;

    private CameraReact cameraReact;
    private ManagerUI managerUI;

    public void Init(int _allVariants, ManagerUI _managerUI) {
        saveData.lastScore = 0;
        cameraReact = Camera.main.GetComponent<CameraReact>();

        allVariants = _allVariants;
        managerUI = _managerUI;

        managerUI.SetInGameScore();

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

        int pointNum = 0;
        for (int i = 0; i < spawnCOunt; i++) {
            pointNum++;
            if (pointNum >= spawnPoints.Count) {
                pointNum = 0;
            }

            Item itm = Instantiate(item, spawnPoints[pointNum].position, Quaternion.identity);
            itm.itemConfiguration = spawnConfs[i];
            itm.managerItem = this;
            itm.Spawn();
            curCount++;
        }
        ManagerGame.instance.LevelLoaded();
    }

    public void DestroyItem(bool _merge, Vector3 _mergePosition) {
        if (_merge) {
            int ms = mergeScore * curScoreMultiplier;
            saveData.lastScore += ms;
            managerUI.SetInGameScore();
            managerUI.ScoreReactInGame();

            ScoreFly sf = Instantiate(scoreFly);
            sf.transform.position = _mergePosition;
            sf.SetScore(ms);

            cameraReact.React();

            curScoreMultiplier++;
            curScoreMultiplier = Mathf.Clamp(curScoreMultiplier, 1, maxScoreMultiplier);
            managerUI.ScoreMultiplierShow(true);
            managerUI.ScoreMultiplierReactUp();
            AfterChangeCurScoreMultiplier();
        }

        curCount--;

        if (curCount <= 0) {
            saveData.score += saveData.lastScore;

            if (scoreMultiplierCoroutine != null) {
                StopCoroutine(scoreMultiplierCoroutine);
            }

            managerUI.ScoreMultiplierShow(false);
            managerUI.HidePauseButton();
            StartCoroutine(EndLevelCoroutine());
        }
    }

    private IEnumerator EndLevelCoroutine() { 
        yield return new WaitForSeconds(endLevelDelay);
        ManagerGame.instance.LevelCompleted();
    }

    private void AfterChangeCurScoreMultiplier() {
        if (scoreMultiplierCoroutine != null) {
            StopCoroutine(scoreMultiplierCoroutine);
        }
        scoreMultiplierCoroutine = ScoreMultiplierCoroutine();
        StartCoroutine(scoreMultiplierCoroutine);
    }

    private IEnumerator ScoreMultiplierCoroutine() {
        managerUI.ScoreMultiplierUpdateText("x" + curScoreMultiplier);

        curSpeedScoreMultiplier = minSpeedScoreMultiplier + speedScoreMultiplierAdd * curScoreMultiplier;
        Debug.Log(curSpeedScoreMultiplier);
        curTimeScoreMultiplier = 1;

        while (curTimeScoreMultiplier > 0) {
            curTimeScoreMultiplier -= minSpeedScoreMultiplier * curSpeedScoreMultiplier * Time.deltaTime;
            managerUI.ScoreMultiplierUpdateProgress(curTimeScoreMultiplier);
            yield return null;
        }

        curScoreMultiplier--;
        curScoreMultiplier = Mathf.Clamp(curScoreMultiplier, 1, maxScoreMultiplier);
        managerUI.ScoreMultiplierReactDown();

        if (curScoreMultiplier > 1) {
            AfterChangeCurScoreMultiplier();
        }
        else {
            managerUI.ScoreMultiplierShow(false);
            curSpeedScoreMultiplier = minSpeedScoreMultiplier;
            curScoreMultiplier = 1;
        }
    }

    private void Shuffle<T>(List<T> list) {
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
