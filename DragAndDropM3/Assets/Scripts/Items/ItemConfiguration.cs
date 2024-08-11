using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfiguration", menuName = "MySO/New ItemConfiguration")]
public class ItemConfiguration : ScriptableObject {
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Vector3 _scale = Vector3.one;
    [SerializeField] private Vector3 _colliderScale = Vector3.one;

    public Mesh mesh => _mesh;
    public Vector3 scale => _scale;
    public Vector3 colliderScale => _colliderScale;
}