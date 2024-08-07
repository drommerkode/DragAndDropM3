using System.Collections;
using UnityEngine;

public class ScoreReact : MonoBehaviour
{
    [SerializeField] private float scaleAdd = 0.2f;
    [SerializeField] private float changeScaleSpeed = 2f;
    private float startScaleX;
    private IEnumerator scoreReactCoroutine;

    private void Awake() {
        startScaleX = transform.localScale.x;
    }

    public void React() {
        if (scoreReactCoroutine != null) {
            StopCoroutine(scoreReactCoroutine);
        }
        float targetScale = startScaleX + scaleAdd;
        scoreReactCoroutine = ScoreReactCoroutine(targetScale);
        StartCoroutine(scoreReactCoroutine);
    }

    private IEnumerator ScoreReactCoroutine(float _targetScale) {
        while (transform.localScale.x != _targetScale) {
            transform.localScale = GetUpdatedFOV(_targetScale);
            yield return null;
        }
        while (transform.localScale.x != startScaleX) {
            transform.localScale = GetUpdatedFOV(startScaleX);
            yield return null;
        }
    }

    private Vector3 GetUpdatedFOV(float _target) {
        float newScaleX = transform.localScale.x;
        newScaleX = Mathf.MoveTowards(newScaleX, _target, changeScaleSpeed * Time.deltaTime);
        return Vector3.one * newScaleX;
    }
}
