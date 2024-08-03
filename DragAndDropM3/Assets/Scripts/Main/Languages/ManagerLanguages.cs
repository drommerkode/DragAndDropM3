using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Networking.UnityWebRequest;

public class ManagerLanguages : MonoBehaviour
{
    /*
        SystemLanguage num
        ru = 30
        en = 10
    */

    private static int curLanguage = 0;

    public static UnityEvent LanguageChangedEvent = new UnityEvent();

    private static Dictionary<string, string> langCUR = new Dictionary<string, string>();
    private static Dictionary<string, string> langRU = new Dictionary<string, string>();
    private static Dictionary<string, string> langEN = new Dictionary<string, string>();

    private void Awake() {
        ManagerGame.OnStartChangeLanguage.AddListener(ChangeLanguage);
    }

    private void Start() {
        SetAllLanguagesStrings();
        int ln = ManagerGame.instance.GetLanguageNumber();
        if (ln == -1) {
            ChangeLanguage((int)Application.systemLanguage);
        }
        else {
            ChangeLanguage(ln);
        }
    }

    private void ChangeLanguage(int _value) {
        curLanguage = _value;
        SetCurLanguage();
        LanguageChangedEvent.Invoke();
    }

    private void SetCurLanguage() {
        if (curLanguage == (int)SystemLanguage.Russian) {
            langCUR = new Dictionary<string, string>(langRU);
        }
        else {
            langCUR = new Dictionary<string, string>(langEN);
        }
    }

    public static string GetLocalisationString(string _textID) {
        if (langCUR.ContainsKey(_textID)) {
            return langCUR[_textID];
        }
        else {
            return _textID;
        }
    }

    #region SET LANGUAGES STRINGS
    private void SetAllLanguagesStrings() {
        SetRuLanguageStrings();
        SetEnLanguageStrings();
    }
    private void SetRuLanguageStrings() {
        langRU.Add("%GameName%",        "Нубик: обби на шихе");
        langRU.Add("%Play%",            "Играть");
        langRU.Add("%Settings%",        "Настройки");
        langRU.Add("%Auth%",            "Авторизация");
        langRU.Add("%Back%",            "Назад");
        langRU.Add("%Yes%",             "Да");
        langRU.Add("%No%",              "Нет");
        langRU.Add("%Levels%",          "Выбор уровня");
        langRU.Add("%Level%",           "Уровень");
        langRU.Add("%Loading%",         "Загрузка...");
        langRU.Add("%Pause%",           "Пауза");
        langRU.Add("%InGamePause%",     "Tab Пауза");
        langRU.Add("%InGameRespawn%",   "R\nЧекпоинт");
        langRU.Add("%Menu%",            "Меню");
        langRU.Add("%Info%",            "Как играть");
        langRU.Add("%SettingsAudio%",   "Громкость");
        langRU.Add("%SettingsControl%", "Управление");
        langRU.Add("%SettingsGraphics%","Графика");
        langRU.Add("%SFX%",             "Звуки");
        langRU.Add("%Music%",           "Музыка");
        langRU.Add("%Camera%",          "Камера");
        langRU.Add("%Steering%",        "Поворот");
        langRU.Add("%Steer%",           "Руль");
        langRU.Add("%Buttons%",         "Кнопки");
        langRU.Add("%Shadows%",         "Тени");
        langRU.Add("%ExitToMenu?%",     "Выйти в меню?");
        langRU.Add("%SignIn?%",         "Авторизоваться?");
        langRU.Add("%AuthBonus%",       "Авторизация позволит вам использовать облачные сохранения");
        langRU.Add("%Rate?%",           "Оценить?");
        langRU.Add("%Logo%",            "Игра от Сергея Ивлева");

        langRU.Add("%Advertising%",     "Реклама");

        langRU.Add("%Results%",         "Результаты");
        langRU.Add("%Restart%",         "Перезапуск");
        langRU.Add("%Next%",            "Далее");
        langRU.Add("%Skip%",            "Пропустить");
        langRU.Add("%Time%",            "Время");
        langRU.Add("%LevelCompleted%",  "Уровень завершен"); 
        langRU.Add("%GameСompleted%",   "Игра пройдена");
        langRU.Add("%LevelFailed%",     "Уровень провален");

        langRU.Add("%Tut_Steering%",    "Поворот");
        langRU.Add("%Tut_Gas%",         "Газ");
        langRU.Add("%Tut_Front%",       "Вперёд");
        langRU.Add("%Tut_Back%",        "Назад");
        langRU.Add("%Tut_Stop%",        "Трмоз");
        langRU.Add("%Tut_Transmission%","Коробка передач");
        langRU.Add("%Tut_Camera%",      "Камера");
        langRU.Add("%Tut_ChangeControl%", "В настройках можно изменить тип управления");

        langRU.Add("%TutPC_Steering%",     "<color=#f94d58>A/D</color> - Поворот");
        langRU.Add("%TutPC_Gas%",          "<color=#f94d58>W/S</color> - Газ");
        langRU.Add("%TutPC_Stop%",         "<color=#f94d58>Пробел</color> - Тормоз");
        langRU.Add("%TutPC_Camera%",       "<color=#f94d58>Мышь</color> - Камера");
        langRU.Add("%TutPC_Check%",        "<color=#f94d58>R</color> - Чекпоинт");
    }

    private void SetEnLanguageStrings() {
        langEN.Add("%GameName%",        "Noob: Obby in a car");
        langEN.Add("%Play%",            "Play");
        langEN.Add("%Settings%",        "Settings");
        langEN.Add("%Auth%",            "Authorization");
        langEN.Add("%Back%",            "Back");
        langEN.Add("%Yes%",             "Yes");
        langEN.Add("%No%",              "No");
        langEN.Add("%Levels%",          "Level selection");
        langEN.Add("%Level%",           "Level");
        langEN.Add("%Loading%",         "Loading...");
        langEN.Add("%Pause%",           "Pause");
        langEN.Add("%InGamePause%",     "Tab Pause");
        langEN.Add("%InGameRespawn%",   "R\nCheckpoint");
        langEN.Add("%Menu%",            "Menu");
        langEN.Add("%Info%",            "How to play");
        langEN.Add("%SettingsAudio%",   "Volume");
        langEN.Add("%SettingsControl%", "Control");
        langEN.Add("%SettingsGraphics%","Graphics");
        langEN.Add("%SFX%",             "SFX");
        langEN.Add("%Music%",           "Music");
        langEN.Add("%Camera%",          "Camera");
        langEN.Add("%Steering%",        "Steering");
        langEN.Add("%Steer%",           "Wheel");
        langEN.Add("%Buttons%",         "Buttons");
        langEN.Add("%Shadows%",         "Shadows");
        langEN.Add("%ExitToMenu?%",     "Exit to menu?");
        langEN.Add("%SignIn?%",         "Sign in?");
        langEN.Add("%AuthBonus%",       "Authorization will allow you to use cloud saves");
        langEN.Add("%Rate?%",           "Rate?");
        langEN.Add("%Logo%",            "Game by Sergey Ivlev");

        langEN.Add("%Advertising%",     "Advertising");

        langEN.Add("%Results%",         "Results");
        langEN.Add("%Restart%",         "Restart");
        langEN.Add("%Next%",            "Next");
        langEN.Add("%Skip%",            "Skip");
        langEN.Add("%Time%",            "Time");  
        langEN.Add("%LevelCompleted%",  "Level completed");
        langEN.Add("%GameСompleted%",   "Game completed");
        langEN.Add("%LevelFailed%",     "Level failed");

        langEN.Add("%Tut_Steering%",    "Steering");
        langEN.Add("%Tut_Gas%",         "Gas");
        langEN.Add("%Tut_Front%",       "Front");
        langEN.Add("%Tut_Back%",        "Back");
        langEN.Add("%Tut_Stop%",        "Stop");
        langEN.Add("%Tut_Transmission%","Transmission");
        langEN.Add("%Tut_Camera%",      "Camera");
        langEN.Add("%Tut_ChangeControl%","In the settings you can change the control type");

        langEN.Add("%TutPC_Steering%",     "<color=#f94d58>A/D</color> - Steering");
        langEN.Add("%TutPC_Gas%",          "<color=#f94d58>W/S</color> - Gas");
        langEN.Add("%TutPC_Stop%",         "<color=#f94d58>Space</color> - Stop");
        langEN.Add("%TutPC_Camera%",       "<color=#f94d58>Mouse</color> - Camera");
        langEN.Add("%TutPC_Check%",        "<color=#f94d58>R</color> - Checkpoint");
    }
    #endregion
}
