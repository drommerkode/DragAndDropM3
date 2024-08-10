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
        langRU.Add("%GameName%",        "Сокровища капибары");
        langRU.Add("%Play%",            "Играть");
        langRU.Add("%Settings%",        "Настройки");
        langRU.Add("%Auth%",            "Авторизация");
        langRU.Add("%Back%",            "Назад");
        langRU.Add("%Yes%",             "Да");
        langRU.Add("%No%",              "Нет");
        langRU.Add("%Level%",           "Уровень");
        langRU.Add("%Loading%",         "Загрузка...");
        langRU.Add("%Pause%",           "Пауза");
        langRU.Add("%Menu%",            "Меню");
        langRU.Add("%SFX%",             "Звуки");
        langRU.Add("%Music%",           "Музыка");
        langRU.Add("%Shadows%",         "Тени");
        langRU.Add("%ExitToMenu?%",     "Выйти в меню?");
        langRU.Add("%SignIn?%",         "Авторизоваться?");
        langRU.Add("%AuthBonus%",       "Авторизация позволит вам использовать облачные сохранения");
        langRU.Add("%Rate?%",           "Оценить?");
        langRU.Add("%Logo%",            "Игра от Сергея Ивлева");

        langRU.Add("%Advertising%",     "Реклама");

        langRU.Add("%Results%",         "Результаты");
        langRU.Add("%Total%",           "Всего");
        langRU.Add("%Collected%",       "Собрано");
        langRU.Add("%Restart%",         "Перезапуск");
        langRU.Add("%Next%",            "Далее");
        langRU.Add("%Skip%",            "Пропустить");
        langRU.Add("%LevelCompleted%",  "Уровень завершен"); 
        langRU.Add("%GameСompleted%",   "Игра пройдена");

        langRU.Add("%Tut_1%",           "Перетаскивай и соединяй одинаковые предметы");
        langRU.Add("%Tut_2%",           "Чем быстрее ты это делаешь, тем больше монет получаешь");

        langRU.Add("%CompleteInfo_1%",  "Ахахахахаха!!!\nВсе сокровища будут наши!!!");
        langRU.Add("%CompleteInfo_2%",  "Ещё! Ещё! Ещё!");
        langRU.Add("%CompleteInfo_3%",  "Нужно больше золота!!!");
        langRU.Add("%CompleteInfo_4%",  "Богатая капибара -\nсчастливая капибара");
        langRU.Add("%CompleteInfo_5%",  "Никто не собирает сокровища\nлучше тебя");
        langRU.Add("%CompleteInfo_6%",  "Какая красота");
    }

    private void SetEnLanguageStrings() {
        langEN.Add("%GameName%",        "Capybara's Treasures");
        langEN.Add("%Play%",            "Play");
        langEN.Add("%Settings%",        "Settings");
        langEN.Add("%Auth%",            "Authorization");
        langEN.Add("%Back%",            "Back");
        langEN.Add("%Yes%",             "Yes");
        langEN.Add("%No%",              "No");
        langEN.Add("%Level%",           "Level");
        langEN.Add("%Loading%",         "Loading...");
        langEN.Add("%Pause%",           "Pause");
        langEN.Add("%Menu%",            "Menu");
        langEN.Add("%SFX%",             "SFX");
        langEN.Add("%Music%",           "Music");
        langEN.Add("%Shadows%",         "Shadows");
        langEN.Add("%ExitToMenu?%",     "Exit to menu?");
        langEN.Add("%SignIn?%",         "Sign in?");
        langEN.Add("%AuthBonus%",       "Authorization will allow you to use cloud saves");
        langEN.Add("%Rate?%",           "Rate?");
        langEN.Add("%Logo%",            "Game by Sergey Ivlev");

        langEN.Add("%Advertising%",     "Advertising");

        langEN.Add("%Results%",         "Results");
        langEN.Add("%Total%",           "Total");
        langEN.Add("%Collected%",       "Collected");
        langEN.Add("%Restart%",         "Restart");
        langEN.Add("%Next%",            "Next");
        langEN.Add("%Skip%",            "Skip");
        langEN.Add("%LevelCompleted%",  "Level completed");
        langEN.Add("%GameСompleted%",   "Game completed");

        langEN.Add("%CompleteInfo_1%", "Ahahahahahaha!!!\nAll the treasures will be ours!!!");
        langEN.Add("%CompleteInfo_2%", "More! More! More!");
        langEN.Add("%CompleteInfo_3%", "We need more gold!!!");
        langEN.Add("%CompleteInfo_4%", "A rich capybara is a\nhappy capybara");
        langEN.Add("%CompleteInfo_5%", "No one collects treasures\nbetter than you");
        langEN.Add("%CompleteInfo_6%", "How beautiful");
    }
    #endregion
}
