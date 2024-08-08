using System.Collections;
using UnityEngine;

public class ScoreReact : MonoBehaviour
{
    [SerializeField] private float scaleAdd = 0.2f;
    [SerializeField] private float changeScaleSpeed = 2f;
    private float startScaleX;
    private IEnumerator scoreReactCoroutine;
    float targetScale;

    private void Awake() {
        startScaleX = transform.localScale.x;
    }

    public void ReactUp() {
        targetScale = startScaleX + scaleAdd;
        StartCoroutine(targetScale);
    }

    public void ReactDown() {
        targetScale = startScaleX - scaleAdd;
        StartCoroutine(targetScale);
    }

    private void StartCoroutine(float _targetScale) {
        if (scoreReactCoroutine != null) {
            StopCoroutine(scoreReactCoroutine);
        }
        scoreReactCoroutine = ScoreReactCoroutine(_targetScale);
        StartCoroutine(scoreReactCoroutine);
    }

    private IEnumerator ScoreReactCoroutine(float _targetScale) {
        while (transform.localScale.x != _targetScale) {
            transform.localScale = GetUpdatedScale(_targetScale);
            yield return null;
        }
        while (transform.localScale.x != startScaleX) {
            transform.localScale = GetUpdatedScale(startScaleX);
            yield return null;
        }
    }

    private Vector3 GetUpdatedScale(float _target) {
        float newScaleX = transform.localScale.x;
        newScaleX = Mathf.MoveTowards(newScaleX, _target, changeScaleSpeed * Time.deltaTime);
        return Vector3.one * newScaleX;
    }
}
