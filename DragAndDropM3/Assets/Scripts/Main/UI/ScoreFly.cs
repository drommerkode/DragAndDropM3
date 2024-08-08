using TMPro;
using UnityEngine;

public class ScoreFly : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI addscoreText;
    [SerializeField] private float decreaseScale = 2f;
    private float curScale;

    private void Awake() {
        curScale = transform.localScale.x;
    }

    public void SetScore(int _score) { 
        addscoreText.text = "+" + _score;
    }

    private void Update() {
        curScale -= decreaseScale * Time.deltaTime;
        if (curScale > 0) {
            transform.localScale = Vector3.one * curScale;
        } else {
            Destroy(gameObject);
        }
    }
}
