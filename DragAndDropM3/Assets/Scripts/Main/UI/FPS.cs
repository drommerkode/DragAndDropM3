using System.Collections;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    private void OnEnable() {
        StartCoroutine(fpsCounterCoroutine());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private void Awake() {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    IEnumerator fpsCounterCoroutine() { 
        WaitForSeconds wfs = new WaitForSeconds(0.5f);
        while (true) {
            fpsText.text = "" + (int)(1f / Time.unscaledDeltaTime);
            yield return wfs;
        }
    }
}
