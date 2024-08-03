using TMPro;
using UnityEngine;

public class SliderCounter : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private float multiplier = 100f;
    public void SetCounter(float _value) {
        textCounter.text = (_value * multiplier).ToString("F0");
    }
}
