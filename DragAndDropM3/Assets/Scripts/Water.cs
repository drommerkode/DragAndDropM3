using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed;
    private MeshRenderer mr;
    private Material material;

    private void Start() {
        mr = GetComponent<MeshRenderer>();
        material = mr.material;
    }

    private void Update() {
        material.mainTextureOffset = Time.time * scrollSpeed;
    }
}
