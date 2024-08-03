using TMPro;
using UnityEngine;

public class LanguageChangeString : MonoBehaviour
{
    public string textID;
    private TextMeshProUGUI textString;

    private void Awake() {
        ManagerLanguages.LanguageChangedEvent.AddListener(LanguageChanged);
        textString = GetComponent<TextMeshProUGUI>();
        LanguageChanged();
    }

    private void LanguageChanged() {
        textString.text = ManagerLanguages.GetLocalisationString(textID);
    }
}
