using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageChangeDropDown : MonoBehaviour
{
    [SerializeField] private List<string> textIDs = new List<string>();
    private TMP_Dropdown dropDown;

    private void Awake() {
        ManagerLanguages.LanguageChangedEvent.AddListener(LanguageChanged);
        dropDown = GetComponent<TMP_Dropdown>();
        LanguageChanged();
    }

    private void LanguageChanged() {
        for(int i = 0; i < dropDown.options.Count; i++) {
            dropDown.options[i].text = ManagerLanguages.GetLocalisationString(textIDs[i]);
        }
        dropDown.RefreshShownValue();
    }
}
