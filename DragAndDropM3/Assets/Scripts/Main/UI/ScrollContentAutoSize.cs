
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContentAutoSize : MonoBehaviour
{
    [SerializeField] private bool needWidth;
    //[SerializeField] private bool needHeight;
    private RectTransform rt;
    private GridLayoutGroup glg;
    private float lastWidth;
    private float lastHeight;

    void Start()
    {
        glg = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
    }

    void FixedUpdate() {
        if (needWidth) {
            if (lastHeight == Screen.height) { return; }
            lastHeight = Screen.height;
            UpdateContentTransformWidth();
        }
    }

    public void UpdateContentTransformWidth() {
        Vector2Int glgSize = GetColumnAndRow();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, glgSize.x * 128 + (glgSize.x - 1) * glg.spacing.x + glg.padding.left + glg.padding.right);
    }

    private Vector2Int GetColumnAndRow() {
        int childCount = glg.transform.childCount;
        if (childCount == 0) { return Vector2Int.zero; }

        Vector2Int size = Vector2Int.one;
        RectTransform firstChildObj = glg.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 firstChildPos = firstChildObj.anchoredPosition;

        for (int i = 1; i < glg.transform.childCount; i++) {
            RectTransform currentChildObj = glg.transform.GetChild(i).GetComponent<RectTransform>();
            Vector2 currentChildPos = currentChildObj.anchoredPosition;
            if (currentChildPos.x == firstChildPos.x) {
                size.y++;
            }
            else {
                break;
            }
        }
        size.x = (int)Math.Ceiling((double)childCount / size.y);
        return size;
    }
}
