using UnityEngine;

public class MoveAroundUI : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Transform center;
    private RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        MoveCircle();
    }

    void MoveCircle() {
        rectTransform.RotateAround(center.position, Vector3.forward, speed * Time.unscaledDeltaTime);
        rectTransform.eulerAngles = Vector3.zero;
    }
}
