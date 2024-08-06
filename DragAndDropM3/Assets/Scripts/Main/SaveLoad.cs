using System;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveData saveData;
    public static int statKilledInd = 0;
    public static int statHeadshotsInd = 1;
    public static int statTimeInd = 2;
    public static void LoadFields() {
        saveData = new SaveData();
        string allSaves = PlayerPrefs.GetString("allSaves", "");
        if (allSaves != "") {
            saveData = JsonUtility.FromJson<SaveData>(allSaves);
        }
    }

    public static void SaveFields() {
        string allSaves = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("allSaves", allSaves);
        PlayerPrefs.Save();
    }

    [Serializable]
    public class SaveData {
        public bool isRate = false;
        public bool showFirstTutorialPC = true;
        public bool showFirstTutorialMobile = true;
        public bool mobileFirstStart = true;

        public float soundMusic = 0.4f;
        public float soundSFX = 0.4f;

        public float sensMouse = 0.5f;
        public float sensPad = 0.25f;

        public bool shadowEnable = true;

        public int levelsOpened = 1;
        public int score = 1;
    }
}