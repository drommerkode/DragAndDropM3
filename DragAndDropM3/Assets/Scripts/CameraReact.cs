using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraReact : MonoBehaviour
{
    private Camera cameraMain;
    [SerializeField] private float addPosition = 0.2f;
    [SerializeField] private float changePosSpeed = 10f;
    [SerializeField] private float addFOV = 1f;
    [SerializeField] private float changeFOVSpeed = 10f;
    private float startFOV;
    private Vector3 startPos;
    private IEnumerator cameraShakeFOVCoroutine;
    private IEnumerator cameraShakePositionCoroutine;

    private void Awake() {
        cameraMain = GetComponent<Camera>();
        startFOV = cameraMain.fieldOfView;
        startPos = cameraMain.transform.position;
    }

    public void React() {
        if (cameraShakeFOVCoroutine != null) { 
            StopCoroutine(cameraShakeFOVCoroutine);
        }
        float randomFOV = Random.Range(-addFOV, addFOV);
        float targetFOV = cameraMain.fieldOfView + randomFOV;
        cameraShakeFOVCoroutine = CameraShakeFOVCoroutine(targetFOV);
        StartCoroutine(cameraShakeFOVCoroutine);

        if (cameraShakePositionCoroutine != null) {
            StopCoroutine(cameraShakePositionCoroutine);
        }
        float randomPos = Random.Range(-addPosition, addPosition);
        Vector3 targetPOS = cameraMain.transform.position + Vector3.one * randomPos;
        cameraShakePositionCoroutine = CameraShakePositionCoroutine(targetPOS);
        StartCoroutine(cameraShakePositionCoroutine);
    }

    private IEnumerator CameraShakeFOVCoroutine(float _target) {
        while (cameraMain.fieldOfView != _target) {
            cameraMain.fieldOfView = GetUpdatedFOV(_target);
            yield return null;
        }
        while (cameraMain.fieldOfView != startFOV) {
            cameraMain.fieldOfView = GetUpdatedFOV(startFOV);
            yield return null;
        }
    }

    private float GetUpdatedFOV(float _target) {
        return Mathf.MoveTowards(cameraMain.fieldOfView, _target, changeFOVSpeed * Time.deltaTime);
    }

    private IEnumerator CameraShakePositionCoroutine(Vector3 _target) {
        while (cameraMain.transform.position != _target) {
            cameraMain.transform.position = GetUpdatedPos(_target);
            yield return null;
        }
        while (cameraMain.transform.position != startPos) {
            cameraMain.transform.position = GetUpdatedPos(startPos);
            yield return null;
        }
    }

    private Vector3 GetUpdatedPos(Vector3 _target) {
        Vector3 newRot = cameraMain.transform.position;
        if (newRot.x != _target.x) {
            newRot.x = Mathf.MoveTowardsAngle(newRot.x, _target.x, changePosSpeed * Time.deltaTime);
        }
        if (newRot.y != _target.y) {
            newRot.y = Mathf.MoveTowardsAngle(newRot.y, _target.y, changePosSpeed * Time.deltaTime);
        }
        if (newRot.z != _target.z) {
            newRot.z = Mathf.MoveTowardsAngle(newRot.z, _target.z, changePosSpeed * Time.deltaTime);
        }
        return newRot;
    }
}
