
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static SaveLoad;

public class ManagerUI : MonoBehaviour {

    [Header("Sounds")]
    [SerializeField] private SoundsForPlay soundPress;
    [SerializeField] private SoundsForPlay soundComplete;
    [SerializeField] private SoundsForPlay soundCoin;

    [Header("Menu links")]
    [SerializeField] private GameObject fullFade;
    [SerializeField] private GameObject halfFade;
    [SerializeField] private GameObject upFade;
    [SerializeField] private GameObject tutFade;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject resultMenu;
    [SerializeField] private GameObject completeMenu;
    [SerializeField] private GameObject failedMenu;
    [SerializeField] private GameObject exitToMainMenu;
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private GameObject logoMenu;
    [SerializeField] private GameObject authMenu;
    [SerializeField] private GameObject rateMenu;

    [Header("Additional")]
    [SerializeField] private GameObject pauseButtonInGame;

    [Header("ADV links")]
    [SerializeField] private GameObject authButton;
    [SerializeField] private GameObject advRewardScoreButton;

    [Header("Setting links")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Toggle shadowToggle;

    [Header("Tutorial")]

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreInGame;
    [SerializeField] private ScoreReact scoreReactInGame;
    [SerializeField] private TextMeshProUGUI scoreLevelMenu;
    [SerializeField] private TextMeshProUGUI scoreResultAll;
    [SerializeField] private ScoreReact scoreReactResultAll;
    [SerializeField] private TextMeshProUGUI scoreResultCur;
    [SerializeField] private ScoreReact scoreReactResultCur;
    [SerializeField] private float scoreResultUpdateDelay = 0.5f;
    private bool x2Geted;

    [Header("Score multiplier")]
    [SerializeField] private GameObject scoreMultiplier;
    [SerializeField] private Image scoreMultiplierProgress;
    [SerializeField] private TextMeshProUGUI scoreMultiplierText;
    [SerializeField] private ScoreReact scoreMultiplierReact;

    [Header("ResultMenu")]
    [SerializeField] private GameObject buttonNext;
    [SerializeField] private GameObject buttonSkip;
    private bool isLevelCompleted;

    private MenuState menuState = MenuState.inGameMenu;
    public enum MenuState {
        mainMenu,
        levelMenu,
        inGameMenu,
        tutorialMenu,
        settingsMenu,
        pauseMenu,
        resultMenu,
        completeMenu,
        failedMenu,
        exitToMainMenu,
        loadingMenu,
        logoMenu,
        authMenu,
        rateMenu
    }

    private List<GameObject> AllUIForDeactive = new List<GameObject>();

    private void Awake() {
        ManagerGame.OnAuthSuccess.AddListener(CheckAuth);
        ManagerGame.OnShowADV.AddListener(ShowADV);
        ManagerGame.OnLoadUserData.AddListener(SetLoadingUserData);
        ManagerGame.OnSceneLoading.AddListener(GoLoadingMenu);
        ManagerGame.OnLevelLoaded.AddListener(HideLoadingMenuOnLevelLoaded);
        ManagerGame.OnShowLevelResult.AddListener(GoResultMenu);
        ManagerGame.OnLevelCompleted.AddListener(GoСompletedMenu);
        ManagerGame.OnLevelFailed.AddListener(GoFailedMenu);
        ManagerGame.OnScoreRevardGeted.AddListener(UpdateResultScoreCur);
        UIInput.OnUIESC.AddListener(EscPress);

        AllUIForDeactive = new List<GameObject> {
                                                    mainMenu,
                                                    levelMenu,
                                                    inGameMenu,
                                                    tutorialMenu,
                                                    settingsMenu,
                                                    pauseMenu,
                                                    resultMenu,
                                                    completeMenu,
                                                    failedMenu,
                                                    exitToMainMenu,
                                                    //loadingMenu,
                                                    logoMenu,
                                                    authMenu,
                                                    rateMenu,
                                                };
        if (ManagerScenes.GetIsLevel()) { 
            GoLoadingMenu();
        }
    }

    public void Init() {
        if (ManagerScenes.GetIsLogo()) {
            GoLogo();
            return;
        }

        ManagerGame.instance.SetPause(false);
        ManagerGame.instance.SetPauseADV(false);

        if (ManagerGame.instance.GetIsAuth()) {
            authButton.SetActive(false);
        }

        if (ManagerScenes.GetIsMainMenu()) {
            SetLoadingUserData();
            GoMainMenu();
            return;
        }

        ManagerGame.instance.SetPause(true);

        if (ManagerGame.instance.GetIsMobile()) {
            if (saveData.showFirstTutorialMobile) {
                GoTutorial();
            }
            else {
                GoInGameMenu();
            }
        }
        else {
            if (saveData.showFirstTutorialPC) {
                GoTutorial();
            }
            else {
                GoInGameMenu();
            }
        }

        SetLoadingUserData();
    }

    private void HideLoadingMenuOnLevelLoaded() {
        HideLoadingMenu();
        if (menuState != MenuState.tutorialMenu) {
            ManagerGame.instance.SetPause(false);
        }
    }

    private void SetLoadingUserData() {
        SetSliderMusicSFX();
        SetShadowToggle();
        ScoreUpdateInLevelMenu();
    }

    public void EscPress() {
        if (ManagerGame.instance.GetPauseADV()) { return; }
        PressSound();
        switch (menuState) {
            case MenuState.inGameMenu:
                GoPauseMenu();
                break;
            case MenuState.tutorialMenu:
                if (ManagerGame.instance.GetIsMobile()) {
                    if (saveData.showFirstTutorialMobile) {
                        GoInGameMenu();
                        saveData.showFirstTutorialMobile = false;
                    }
                    else {
                        GoPauseMenu();
                    }
                }
                else {
                    if (saveData.showFirstTutorialPC) {
                        GoInGameMenu();
                        saveData.showFirstTutorialPC = false;
                    }
                    else {
                        GoPauseMenu();
                    }
                }
                break;
            case MenuState.settingsMenu:
                ManagerGame.instance.SaveUserData();
                if (ManagerScenes.GetIsMainMenu()) {
                    GoMainMenu();
                }
                else {
                    GoPauseMenu();
                }
                break;
            case MenuState.pauseMenu:
                GoInGameMenu();
                break;
            case MenuState.exitToMainMenu:
                GoPauseMenu();
                break;
            case MenuState.authMenu:
            case MenuState.levelMenu:
                GoMainMenu();
                break;
            case MenuState.rateMenu:
                GoInGameMenu();
                break;
        }
    }

    public void ChangeLanguage(int _value) {
        PressSound();
        ManagerGame.instance.StartChangeLanguage(_value);
    }

    public void LevelRestart() {
        ManagerGame.instance.ShowCommonADV();
        ManagerScenes.SceneRestart();
    }

    public void LevelNext() {
        ManagerGame.instance.SetCurLevel(saveData.levelsOpened);
        ManagerGame.instance.ShowCommonADV();
        ManagerScenes.GoLevel();
    }

    public void PressSound() {
        soundPress.PlaySound();
    }

    #region EXTRA
    public void HidePauseButton() { 
        pauseButtonInGame.SetActive(false);
    }
    #endregion

    #region SCORE
    public void SetInGameScore() {
        scoreInGame.text = saveData.lastScore.ToString();
    }

    public void ScoreReactInGame() {
        scoreReactInGame.ReactUp();
    }

    private void ScoreUpdateInLevelMenu() { 
        scoreLevelMenu.text = saveData.score.ToString();
    }

    private void UpdateResultScoreAll() {
        StartCoroutine(AddScoreAllCoroutine());
    }

    private void UpdateResultScoreCur() {
        advRewardScoreButton.SetActive(false);
        StartCoroutine(AddScoreCurCoroutine());
    }

    private IEnumerator AddScoreAllCoroutine() { 
        yield return new WaitForSecondsRealtime(scoreResultUpdateDelay);
        scoreResultAll.text = saveData.score.ToString();
        scoreReactResultAll.ReactUp();
        soundCoin.PlaySound();
    }

    private IEnumerator AddScoreCurCoroutine() {
        yield return new WaitForSecondsRealtime(scoreResultUpdateDelay);
        scoreResultCur.text = saveData.lastScore.ToString();
        scoreReactResultCur.ReactUp();
        soundCoin.PlaySound();
        UpdateResultScoreAll();
    }
    #endregion

    #region SCORE MULTIPLIER
    public void ScoreMultiplierShow(bool _value) { 
        scoreMultiplier.SetActive(_value);
    }
    public void ScoreMultiplierUpdateProgress(float _progress) { 
        scoreMultiplierProgress.fillAmount = _progress;
    }

    public void ScoreMultiplierUpdateText(string _text) {
        scoreMultiplierText.text = _text;
    }

    public void ScoreMultiplierReactUp() {
        scoreMultiplierReact.ReactUp();
    }

    public void ScoreMultiplierReactDown() {
        scoreMultiplierReact.ReactDown();
    }
    #endregion

    #region DEACTIVATE
    private void DeactiveAllUI() {
        foreach (GameObject uiItem in AllUIForDeactive) {
            uiItem.SetActive(false);
        }
    }

    private void DeactiveAllFade() {
        fullFade.SetActive(false);
        halfFade.SetActive(false);
        upFade.SetActive(false);
        tutFade.SetActive(false);
    }
    #endregion

    #region GO MENU
    public void GoMainMenu() {
        if (!ManagerScenes.GetIsMainMenu()) {
            ManagerScenes.GoMainMenu();
        }
        else {
            DeactiveAllUI();
            DeactiveAllFade();
            mainMenu.SetActive(true);
            ManagerGame.instance.ShowLevelMenu(false);
            menuState = MenuState.mainMenu;
        }
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        ManagerGame.instance.GameplayStop();
    }

    public void GoInGameMenu() {
        if (!ManagerScenes.GetIsLevel()) {
            ManagerScenes.GoLevel();
        }
        else {
            DeactiveAllUI();
            DeactiveAllFade();
            upFade.SetActive(true);
            inGameMenu.SetActive(true);
            menuState = MenuState.inGameMenu;
            ManagerGame.instance.SetPause(false);
            ManagerGame.instance.SetPauseADV(false);
            //ManagerGame.instance.ChangeCursorState(CursorLockMode.Locked);
            ManagerGame.instance.GameplayStart();
        }
    }
    public void GoLevelMenu() {
        PressSound();
        DeactiveAllUI();
        upFade.SetActive(true);
        levelMenu.SetActive(true);
        ManagerGame.instance.ShowLevelMenu(true);
        menuState = MenuState.levelMenu;
    }

    public void GoTutorial() {
        DeactiveAllUI();
        //ManagerGame.instance.SetPause(true);
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        tutFade.SetActive(true);
        tutorialMenu.SetActive(true);
        menuState = MenuState.tutorialMenu;
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.Locked);
    }

    public void GoSettingsMenu() {
        PressSound();
        DeactiveAllUI();
        halfFade.SetActive(true);
        settingsMenu.SetActive(true);
        menuState = MenuState.settingsMenu;
    }

    public void GoPauseMenu() {
        PressSound();
        ManagerGame.instance.SetPause(true);
        DeactiveAllUI();
        halfFade.SetActive(true);
        pauseMenu.SetActive(true);
        menuState = MenuState.pauseMenu;
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
    }

    public void GoResultMenu(bool _afterReward) {
        DeactiveAllUI();
        DeactiveAllFade();
        buttonSkip.SetActive(!isLevelCompleted);
        buttonNext.SetActive(isLevelCompleted);
        halfFade.SetActive(true);
        resultMenu.SetActive(true);
        menuState = MenuState.resultMenu;
        scoreResultAll.text = (saveData.score - (_afterReward ? 0 : saveData.lastScore)).ToString();
        scoreResultCur.text = saveData.lastScore.ToString();
        UpdateResultScoreAll();
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        ManagerGame.instance.GameplayStop();
    }

    public void GoСompletedMenu() {
        DeactiveAllUI();
        DeactiveAllFade();
        soundComplete.PlaySound();
        isLevelCompleted = true;
        completeMenu.SetActive(true);
        menuState = MenuState.completeMenu;
    }

    public void GoFailedMenu() {
        DeactiveAllUI();
        DeactiveAllFade();
        failedMenu.SetActive(true);
        menuState = MenuState.failedMenu;
    }

    public void GoExitToMainMenu() {
        PressSound();
        ManagerGame.instance.SetPause(true);
        DeactiveAllUI();
        exitToMainMenu.SetActive(true);
        menuState = MenuState.exitToMainMenu;

    }
    private void GoLoadingMenu() {
        DeactiveAllUI();
        fullFade.SetActive(true);
        loadingMenu.SetActive(true);
    }

    private void HideLoadingMenu() {
        fullFade.SetActive(false);
        loadingMenu.SetActive(false);
    }

    private void GoLogo() {
        DeactiveAllUI();
        fullFade.SetActive(true);
        logoMenu.SetActive(true);
        menuState = MenuState.logoMenu;
    }

    #region MOBILE/WEB
    public void GoAuthMenu() {
        PressSound();
        DeactiveAllUI();
        halfFade.SetActive(true);
        authMenu.SetActive(true);
        menuState = MenuState.authMenu;
    }

    public void GoTryAuth() {
        PressSound();
        DeactiveAllUI();
        GoMainMenu();
        ManagerGame.instance.Auth();
    }

    private void GoRateMenu() {
        ManagerGame.instance.SetPause(true);
        DeactiveAllUI();
        halfFade.SetActive(true);
        rateMenu.SetActive(true);
        menuState = MenuState.rateMenu;
    }

    public void GoTryRate() {
        PressSound();
        saveData.isRate = true;
        ManagerGame.instance.RateGame();
    }
    #endregion

    #endregion

    #region SOUND
    public void SetSoundSFX(float _value) {
        ManagerGame.instance.SetSoundSFX(_value);
    }

    public void SetSoundMusic(float _value) {
        ManagerGame.instance.SetSoundMusic(_value);
    }

    private void SetSliderMusicSFX() {
        sliderMusic.value = saveData.soundMusic;
        sliderSFX.value = saveData.soundSFX;
    }
    #endregion

    #region Shadows
    public void ShadowOnOff(bool _value) {
        ManagerGame.instance.ShadowOnOff(_value);
    }
    
    private void SetShadowToggle() {
        shadowToggle.isOn = saveData.shadowEnable;
    }
    #endregion

    #region ADV
    private void ShowADV(bool _value) {
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        if (_value) {
            DeactiveAllUI();
        }
        else {
            switch (menuState) {
                case MenuState.logoMenu:
                    ManagerGame.instance.SetPause(false);
                    GoLogo();
                    break;
                case MenuState.mainMenu:
                    ManagerGame.instance.SetPause(false);
                    GoMainMenu();
                    break;
                case MenuState.settingsMenu:
                    GoSettingsMenu();
                    DopCheckForPauseAfterADV();
                    break;
                case MenuState.completeMenu:
                case MenuState.failedMenu:
                case MenuState.resultMenu:
                    ManagerGame.instance.SetPause(true);
                    GoResultMenu(true);
                    break;
                default:
                    GoInGameMenu();
                    break;
            }
        }
    }

    public void ShowRewardADVScore() {
        ManagerGame.instance.ShowRewardADV(ADV.RewardType.curScore);
    }

    private void DopCheckForPauseAfterADV() {
        if (ManagerScenes.GetIsMainMenu() || ManagerScenes.GetIsLogo()) {
            ManagerGame.instance.SetPause(false);
        }
        else {
            ManagerGame.instance.SetPause(true);
        }
    }

    private void CheckAuth() {
        if (ManagerGame.instance.GetIsAuth()) {
            authButton.SetActive(false);
        }
    }
    #endregion
}
