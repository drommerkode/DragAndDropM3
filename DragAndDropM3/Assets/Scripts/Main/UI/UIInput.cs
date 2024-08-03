using UnityEngine;
using UnityEngine.Events;

public class UIInput : MonoBehaviour
{
    public static UnityEvent OnUIESC = new UnityEvent();

    private void Update() {
        if (Input.GetButtonDown("Cancel")) { OnUIESC.Invoke(); }
    }
}
