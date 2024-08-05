
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveLoad;

public class ManagerUI : MonoBehaviour {

    [SerializeField] private SoundsForPlay soundPress;

    [Header("Menu links")]
    [SerializeField] private GameObject fullFade;
    [SerializeField] private GameObject halfFade;
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

    [Header("ADV links")]
    [SerializeField] private GameObject authButton;

    [Header("Setting links")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Toggle shadowToggle;

    [Header("Tutorial")]

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
        ManagerGame.OnShowLevelResult.AddListener(GoResultMenu);
        ManagerGame.OnLevelCompleted.AddListener(GoСompletedMenu);
        ManagerGame.OnLevelFailed.AddListener(GoFailedMenu);
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
            StartCoroutine(HideLoadingMenuCoroutine());
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
        StartCoroutine(HideLoadingMenuCoroutine());
    }

    private IEnumerator HideLoadingMenuCoroutine() {
        yield return new WaitForSecondsRealtime(ManagerGame.instance.GetBootLoadingTime());
        HideLoadingMenu();
        if (menuState != MenuState.tutorialMenu) {
            ManagerGame.instance.SetPause(false);
        }
    }

    private void SetLoadingUserData() {
        SetSliderMusicSFX();
        SetShadowToggle();
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
        ManagerGame.instance.ShowCommonADV();
        ManagerScenes.GoLevel();
    }

    public void PressSound() {
        soundPress.PlaySound();
    }

    #region DEACTIVATE
    private void DeactiveAllUI() {
        foreach (GameObject uiItem in AllUIForDeactive) {
            uiItem.SetActive(false);
        }
    }

    private void DeactiveAllFade() {
        fullFade.SetActive(false);
        halfFade.SetActive(false);
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
            inGameMenu.SetActive(true);
            menuState = MenuState.inGameMenu;
            ManagerGame.instance.SetPause(false);
            ManagerGame.instance.SetPauseADV(false);
            //ManagerGame.instance.ChangeCursorState(CursorLockMode.Locked);
            ManagerGame.instance.GameplayStart();
        }
    }

    public void GoTutorial() {
        ManagerGame.instance.SetPause(true);
        ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        halfFade.SetActive(true);
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

    public void GoResultMenu() {
        DeactiveAllUI();
        buttonSkip.SetActive(!isLevelCompleted);
        buttonNext.SetActive(isLevelCompleted);
        halfFade.SetActive(true);
        resultMenu.SetActive(true);
        menuState = MenuState.resultMenu;
        //ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
        ManagerGame.instance.GameplayStop();
    }

    public void GoСompletedMenu() {
        DeactiveAllUI();
        isLevelCompleted = true;
        completeMenu.SetActive(true);
        menuState = MenuState.completeMenu;
    }

    public void GoFailedMenu() {
        DeactiveAllUI();
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

    #region ADV
    public void ShowRewardADV() {
        ManagerGame.instance.ShowRewardADV(ADV.RewardType.respawn);
    }
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
        PressSound();
        ManagerGame.instance.ShadowOnOff(_value);
    }
    
    private void SetShadowToggle() {
        shadowToggle.isOn = saveData.shadowEnable;
    }
    #endregion

    #region ADV
    private void ShowADV(bool _value) {
        ManagerGame.instance.ChangeCursorState(CursorLockMode.None);
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
                    GoResultMenu();
                    break;
                default:
                    GoInGameMenu();
                    break;
            }
        }
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
