//using GamePush;
using System.Collections.Generic;
using UnityEngine;

public class ADV : MonoBehaviour {

    [Header("ADV")]
    public AdsType adsType = AdsType.no;
    private bool adBlock;

    public enum AdsType {
        no,
        GamePush,
        Yandex
    }

    public enum RewardType { 
        curScore,
        skip
    }

    private List<string> rewardStrings = new List<string>() { "coins", "respawn" };

    private void OnEnable() {
        //GP_Player.OnLoginComplete += AuthSucces;
    }

    private void OnDisable() {
        //GP_Player.OnLoginComplete -= AuthSucces;
    }

    #region MainCheck
    public void CheckAdBlock() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("CheckAdBlock");
        switch (adsType) {
            case AdsType.GamePush:
                /*adBlock = GP_Ads.IsAdblockEnabled();
                Debug.Log("adBlock" + adBlock);*/
                break;
            case AdsType.Yandex:
                adBlock = false;
                break;
        }
    }

    public void GetLang() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("GetLang");
        
        switch (adsType) {
            case AdsType.GamePush:
                /*Language lang = Language.English;
                switch (adsType) {
                    case AdsType.GamePush:
                        lang = GP_Language.Current();
                        break;
                }
                int langNum = 10;
                switch (lang) {
                    case Language.Russian:
                        langNum = 30;
                        break;
                }
                ManagerGame.instance.StartChangeLanguage(langNum);*/
                break;
            case AdsType.Yandex:
                YandexFunc.GetLangYandex();
                break;
        }
    }

    public void IsItMobile() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("IsItMobile");
        switch (adsType) {
            case AdsType.GamePush:
                //ManagerGame.instance.SetIsMobile(GP_Device.IsMobile());
                break;
            case AdsType.Yandex:
                YandexFunc.IsItMobileYandex();
                break;
        }
    }
    #endregion

    #region Auth/SaveLoad
    public void StartAuth() {
        if (adsType == AdsType.no) { return; }
        Debug.Log("StartAuth");
        switch (adsType) {
            case AdsType.GamePush:
                /*bool isAuth = GP_Player.IsLoggedIn();
                Debug.Log("GP_Player.IsLoggedIn()" + isAuth);
                if (isAuth)
                {
                    AuthSucces();
                }*/
                break;
            case AdsType.Yandex:
                YandexFunc.PlayerStartAuthYandex();
                break;
        }
    }

    public void Auth() {
        if (adsType == AdsType.no) { return; }
        Debug.Log("Auth");
        switch (adsType) {
            case AdsType.GamePush:
                //GP_Player.Login();
                break;
            case AdsType.Yandex:
                YandexFunc.PlayerAuthYandex();
                break;
        }
    }

    private void AuthSucces() {
        Debug.Log("AuthSucces");
        string jsonString = "";
        switch (adsType) {
            case AdsType.GamePush:
                /*if (GP_Player.Has("savedata")) {
                    jsonString = GP_Player.GetString("savedata");
                    Debug.Log("(Yandex) Users data - " + jsonString);
                }
                ManagerGame.instance.SetIsAuth(true);
                ManagerGame.instance.AuthSuccess();
                if (jsonString != null && jsonString != "") {
                    saveData = JsonUtility.FromJson<SaveData>(jsonString);
                    ManagerGame.instance.ChangeMainDataAfterLoad();
                }
                */
                break;
        }
        
    }

    public void SaveDataADV() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("SaveDataADV");
        switch (adsType) {
            case AdsType.GamePush:
                /*string jsonString = JsonUtility.ToJson(saveData);
                GP_Player.Set("savedata", jsonString);
                //GP_Player.Set("score", saveData.rating);
                GP_Player.Sync();*/
                break;
            case AdsType.Yandex:
                YandexFunc.SetUserDataYandex();
                break;
        }
    }
    #endregion

    #region FullScreen/Rewerded
    public void ShowCommonADV() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("Try show common ADV");
        if (adBlock) { return; }
        switch (adsType) {
            case AdsType.GamePush:
                /*if (!GP_Ads.IsFullscreenAvailable()) { return; }
                Debug.Log("Show common ADV");
                GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);*/
                break;
            case AdsType.Yandex:
                YandexFunc.ShowCommonAdvYandex();
                break;
        }
    }

    private void OnFullscreenStart() {
        Debug.Log("Fullscreen Start");
        ManagerGame.instance.PauseForADV(true);
    }

    private void OnFullscreenClose(bool _value) {
        Debug.Log("Fullscreen Close");
        ManagerGame.instance.PauseForADV(false);
    }

    public void ShowRewardADV(RewardType _rewardType) {
        if (adsType == AdsType.no) { return; }

        Debug.Log("Try show reward ADV");
        if (adBlock) { return; }
        switch (adsType) {
            case AdsType.GamePush:
                /*if (!GP_Ads.IsRewardedAvailable()) { return; }
                Debug.Log("Show reward ADV");
                GP_Ads.ShowRewarded(rewardStrings[(int)_rewardType], OnRewardedReward, OnRewardedStart, OnRewardedClose);*/
                break;
            case AdsType.Yandex:
                YandexFunc.ShowRewardAdvYandex((int)_rewardType);
                break;
        }
    }

    private void OnRewardedReward(string _rewardString) {
        Debug.Log("Rewarded Reward");
        ManagerGame.instance.PauseForADV(false);
        ManagerGame.instance.RewardGeted((RewardType)rewardStrings.IndexOf(_rewardString));
    }

    private void OnRewardedStart() {
        Debug.Log("Rewarded Start");
        ManagerGame.instance.PauseForADV(true);
    }

    private void OnRewardedClose(bool _value) {
        Debug.Log("Rewarded Close");
        ManagerGame.instance.PauseForADV(false);

    }

    #endregion
    public void RateGame() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("RateGame");
        switch (adsType) {
            case AdsType.GamePush:
                break;
            case AdsType.Yandex:
                YandexFunc.RateGameYandex();
                break;
        }
    }

    //For poki, crazy
    public void GameplayStart() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("GameplayStart");
        switch (adsType) {
            case AdsType.GamePush:
                //GP_Game.GameplayStart();
                break;
        }
    }

    public void GameplayStop() {
        if (adsType == AdsType.no) { return; }

        Debug.Log("GameplayStop");
        switch (adsType) {
            case AdsType.GamePush:
                //GP_Game.GameplayStop();
                break;
        }
    }

}
