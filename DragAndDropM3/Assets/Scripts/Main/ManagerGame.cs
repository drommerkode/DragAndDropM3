using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static ADV;
using static SaveLoad;

public class ManagerGame : MonoBehaviour {

    [Header("Main")]
    [SerializeField] private bool isMobile = false;
    [SerializeField] private bool isAuth = false;
    private bool isPause = false;
    private bool isPauseADV = false;
    [SerializeField] private float initAdvTime = 0.2f;
    [SerializeField] private float logoTime = 1.3f;
    [SerializeField] private float goMainMenuTime = 0.5f;
    [SerializeField] private float completeFailedTime = 2.5f;

    [Header("Sounds")]
    [SerializeField] private SoundsForPlay mainMusic;

    [Header("Times")]
    [SerializeField] private float bootDelay = 0.2f;

    //ADV
    private ADV adv;

    //game
    private int curLevel;

    #region FOR_SAVE
    // save if need
    private int languageNumber = -1;
    #endregion

    #region EVENTS
    public static UnityEvent OnSetSoundSFX = new UnityEvent();
    public static UnityEvent OnSetSoundMusic = new UnityEvent();
    public static UnityEvent OnAuthSuccess = new UnityEvent();
    public static UnityEvent<bool> OnShowADV = new UnityEvent<bool>();
    public static UnityEvent OnLoadUserData = new UnityEvent();
    public static UnityEvent<bool> OnPauseSet = new UnityEvent<bool>();

    public static UnityEvent OnSceneLoading = new UnityEvent();
    public static UnityEvent<int> OnStartChangeLanguage = new UnityEvent<int>();

    public static UnityEvent OnLevelLoaded = new UnityEvent();
    public static UnityEvent OnShowLevelResult = new UnityEvent();
    public static UnityEvent OnLevelCompleted = new UnityEvent();
    public static UnityEvent OnLevelFailed = new UnityEvent();

    public static UnityEvent<bool> OnShowLevelMenu = new UnityEvent<bool>();
    public static UnityEvent OnScoreRevardGeted = new UnityEvent();
    #endregion

    public static ManagerGame instance { get; private set; }
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            adv = GetComponent<ADV>();
            LoadUserData();
            return;
        }
        Destroy(gameObject);
    }

    private void Start() {
        GetLang();
        CheckAdBlock();
        StartCoroutine(StartAuthCoroutine());
        LogoCheck();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    public bool GetIsMobile() {
        return isMobile;
    }

    public void SetIsMobile(bool _value) {
        isMobile = _value;
    }

    public float GetBootDelayTime() {
        return bootDelay;
    }

    public int GetCurLevel() {
        return curLevel;
    }

    public void SetCurLevelNext() {
        curLevel++;
    }

    public void SetCurLevel(int _curLevel) {
        curLevel = _curLevel;
    }

    #region Events
    public void ChangeCursorState(CursorLockMode _cursorLockMode) {
        if (!isMobile) {
            Cursor.lockState = _cursorLockMode;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LevelLoaded() {
        OnLevelLoaded.Invoke();
    }

    private void LevelResult() {
        //SetPause(true);
        OnShowLevelResult.Invoke();
    }

    public void LevelCompleted() {
        //levelCompleteSound.PlaySound();
        if (curLevel == saveData.levelsOpened) {
            saveData.levelsOpened++;
        }
        Debug.Log("curLevel - " + curLevel + " | " + "levelsOpened - " + saveData.levelsOpened);
        SaveUserData();
        OnLevelCompleted.Invoke();
        StartCoroutine(ShowLevelResultCoroutione());
    }

    public void LevelFailed() {
        //playerDeathSound.PlaySound();
        OnLevelFailed.Invoke();
        StartCoroutine(ShowLevelResultCoroutione());
    }

    private IEnumerator ShowLevelResultCoroutione() {
        yield return new WaitForSecondsRealtime(completeFailedTime);
        //ShowCommonADV();
        //yield return new WaitForSecondsRealtime(0.2f);
        LevelResult();
    }

    public void ShadowOnOff(bool _value) {
        if (_value) {
            saveData.shadowEnable = true;
        }
        else {
            saveData.shadowEnable = false;
        }
        SetShadowEnabled();
    }

    private void SetShadowEnabled() {
        if (saveData.shadowEnable) {
            QualitySettings.shadows = ShadowQuality.HardOnly;
        }
        else {
            QualitySettings.shadows = ShadowQuality.Disable;
        }
    }

    public void ShowLevelMenu(bool _value) {
        OnShowLevelMenu.Invoke(_value);
    }

    #endregion

    #region Language
    public int GetLanguageNumber() {
        return languageNumber;
    }

    public void StartChangeLanguage(int _value) {
        languageNumber = _value;
        OnStartChangeLanguage.Invoke(languageNumber);
    }
    #endregion

    #region SAVELOAD

    public void LoadUserData() {
        LoadFields();
        ChangeMainDataAfterLoad();
    }

    public void ChangeMainDataAfterLoad() {
        if (isMobile && saveData.mobileFirstStart) {
            saveData.mobileFirstStart = false;
            saveData.shadowEnable = false;
            SetShadowEnabled();
        }
        OnLoadUserData.Invoke();
    }

    public void SaveUserData() {
        SaveFields();
#if UNITY_EDITOR 
        return;
#else
        adv.SaveDataADV();
#endif
    }
    #endregion

    #region ADV
    private IEnumerator StartAuthCoroutine() {
        yield return new WaitForSecondsRealtime(initAdvTime);
        IsItMobile();
        StartAuth();
    }

    private void IsItMobile() {
#if UNITY_EDITOR 
        return;
#else
        adv.IsItMobile();
#endif
    }

    private void StartAuth() {
#if UNITY_EDITOR
        return;
#else
        adv.StartAuth();
#endif
    }

    private void GetLang() {
#if UNITY_EDITOR
        return;
#else
        adv.GetLang();
#endif
    }

    private void CheckAdBlock() {
#if UNITY_EDITOR
        return;
#else
        adv.CheckAdBlock();
#endif
    }

    public void RateGame() {
#if UNITY_EDITOR
        return;
#else
        adv.RateGame();
#endif
    }

    public void Auth() {
#if UNITY_EDITOR
        return;
#else
        adv.Auth();
#endif
    }

    public bool GetIsAuth() {
        return isAuth;
    }

    public void SetIsAuth(bool _value) {
        isAuth = _value;
    }

    public void AuthSuccess() {
        OnAuthSuccess.Invoke();
    }

    public void GameplayStart() {
#if UNITY_EDITOR
        return;
#else
        adv.GameplayStart();
#endif
    }

    public void GameplayStop() {
#if UNITY_EDITOR
        return;
#else
        adv.GameplayStop();
#endif
    }

    #endregion

    #region Fullscreen/Rewarded

    public void ShowCommonADV() {
#if UNITY_EDITOR
        return;
#else
        adv.ShowCommonADV();
#endif
    }

    public void ShowRewardADV(RewardType _rewardType) {
#if UNITY_EDITOR
        RewardGeted(_rewardType);
        return;
#else
        adv.ShowRewardADV(_rewardType);
#endif

    }

    public void RewardGeted(RewardType _rewardType) {
        switch (_rewardType) {
            case RewardType.curScore:
                saveData.score += saveData.lastScore;
                saveData.lastScore *= 2;
                SaveUserData();
                OnScoreRevardGeted.Invoke();
                break;
            case RewardType.skip:
                SaveUserData();
                ManagerScenes.GoLevel();
                break;
        }
    }

    #endregion

    #region PAUSE
    public void SetPause(bool _value) {
        isPause = _value;
        Time.timeScale = isPause ? 0 : 1;
        OnPauseSet.Invoke(_value);
    }

    public bool GetPause() {
        return isPause;
    }

    public void PauseForADV(bool _value) {
        AudioListener.volume = _value == true ? 0 : 1;
        SetPause(_value);
        SetPauseADV(_value);
        OnShowADV.Invoke(_value);
    }

    public void SetPauseADV(bool _value) {
        isPauseADV = _value;
    }

    public bool GetPauseADV() {
        return isPauseADV;
    }

    #endregion

    #region SCENE
    public void SceneLoading() {
        if (!ManagerScenes.GetIsLogo()) {
            OnSceneLoading.Invoke();
        }
    }
    private void LogoCheck() {
        if (ManagerScenes.GetIsLogo()) {
            StartCoroutine(AfterLogo());
        }
        else {
            mainMusic.PlaySound();
        }
    }

    private IEnumerator AfterLogo() {
        yield return new WaitForSecondsRealtime(logoTime);
        ShowCommonADV();
        yield return new WaitForSecondsRealtime(goMainMenuTime);
        ManagerScenes.GoMainMenu();
        mainMusic.PlaySound();
    }
    #endregion

    #region SOUND
    public void SetSoundSFX(float _value) {
        saveData.soundSFX = _value;
        OnSetSoundSFX.Invoke();
    }

    public void SetSoundMusic(float _value) {
        saveData.soundMusic = _value;
        OnSetSoundMusic.Invoke();
    }
    #endregion
}
