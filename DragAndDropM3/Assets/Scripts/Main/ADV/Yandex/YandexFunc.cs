using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static ADV;
using static SaveLoad;

public class YandexFunc : MonoBehaviour {
    
    public static bool isYandexAuth = false;
    
    [DllImport("__Internal")]
    private static extern void IsItMobile();

    [DllImport("__Internal")]
    private static extern void GetLang();

    [DllImport("__Internal")]
    private static extern void PlayerStartAuth();

    [DllImport("__Internal")]
    private static extern void PlayerAuth();

    [DllImport("__Internal")]
    private static extern void GetUserData();

    [DllImport("__Internal")]
    private static extern void SetUserData(string _data);

    [DllImport("__Internal")]
    private static extern void ShowCommonAdv();

    [DllImport("__Internal")]
    private static extern void ShowRewardAdv(int _rewardType);

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void SetUserLiderScore(int _score);

    #region IsMobile
    public static void IsItMobileYandex() {
        Debug.Log("(Yandex) IsItMobile?");
#if UNITY_WEBGL && !UNITY_EDITOR
        IsItMobile();
#endif
    }

    public void SetIsMobileYandex(string _value) {
        Debug.Log("(Yandex) isMobile - " + _value);
        ManagerGame.instance.SetIsMobile(_value == "true" ? true : false);
    }
    #endregion

    #region Lang
    public static void GetLangYandex() {
        Debug.Log("(Yandex) GetLang");
#if UNITY_WEBGL && !UNITY_EDITOR
        GetLang();
#endif
    }
    public void SetLang(string _value) {
        Debug.Log("(Yandex) SetLang - " + _value);
        int langNum = 10;
        switch (_value) {
            case "be":
            case "kk":
            case "uk":
            case "uz":
            case "ru":
                langNum = 30;
                break;
        }
        ManagerGame.instance.StartChangeLanguage(langNum);
    }
    #endregion

    #region AUTH
    public static void PlayerStartAuthYandex() {
        Debug.Log("(Yandex) Try start auth");
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerStartAuth();
#endif
    }

    public static void PlayerAuthYandex() {
        Debug.Log("(Yandex) Try auth");
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAuth();
#endif
    }

    public void StartAuthLose() {
        //ManagerGame.instance.StartHideLogo();
    }

    public void AuthSuccess(string _data) {
        Debug.Log("(Yandex) Auth success. User - " + _data);
        isYandexAuth = true;
        ManagerGame.instance.SetIsAuth(true);
        ManagerGame.instance.AuthSuccess();
        GetUserData();
    }
#endregion

    #region SAVE/LOAD
    public void GetUserDataYandex(string _data) {
        Debug.Log("(Yandex) Users data from yandex - " + _data);
        string jsonString;
        try {
            jsonString = _data.Remove(_data.Length - 1);
            jsonString = jsonString.Remove(0, 8);
        }
        catch {
            jsonString = _data;
        }
        saveData = JsonUtility.FromJson<SaveData>(jsonString);
        ManagerGame.instance.ChangeMainDataAfterLoad();
        //ManagerGame.instance.StartHideLogo();
    }

    public static void SetUserDataYandex() {
        string json = JsonUtility.ToJson(saveData);
        Debug.Log("(Yandex) Users data to yandex - " + json);
        if (isYandexAuth) {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetUserData(json);
            SetUserLiderScoreYandex(saveData.score);
#endif
        }
    }

#endregion

    #region ADV
    public static void ShowCommonAdvYandex() {
        Debug.Log("(Yandex) Show common Adv");
#if UNITY_WEBGL && !UNITY_EDITOR
        ShowCommonAdv();
#endif
    }

    public static void ShowRewardAdvYandex(int _rewardType) {
        Debug.Log("(Yandex) Show reward Adv");
#if UNITY_WEBGL && !UNITY_EDITOR
        ShowRewardAdv(_rewardType);
#endif
    }

    public void RewardGetting(int _rewardType) {
        Debug.Log("(Yandex) Reward is getting");
        ManagerGame.instance.RewardGeted((RewardType)_rewardType);
    }

    public void PauseForAdv(string _value) {
        Debug.Log("(Yandex) Adv pause - " + _value);
        bool pause = _value == "true" ? true : false;
        ManagerGame.instance.PauseForADV(pause);
    }
#endregion

    #region RATE

    public static void RateGameYandex() {
        if (isYandexAuth) {
#if UNITY_WEBGL && !UNITY_EDITOR
            RateGame();
#endif
        }
    }
#endregion

    #region LIDERBOARD
    public static void SetUserLiderScoreYandex(int _score) {
        Debug.Log("(Yandex) Liderboard. Score - " + _score);
#if UNITY_WEBGL && !UNITY_EDITOR
        SetUserLiderScore(_score);
#endif
    }
#endregion
    
}
