using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfiguration", menuName = "MySO/New ItemConfiguration")]
public class ItemConfiguration : ScriptableObject {
    [SerializeField] private Mesh _mesh;

    public Mesh mesh => _mesh;
}