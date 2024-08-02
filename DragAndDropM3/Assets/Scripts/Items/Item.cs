using UnityEngine;

public class Item : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    public ItemConfiguration itemConfiguration;
    [SerializeField] private LayerMask maskItem;
    [SerializeField] private ParticleSystem partDestroy;
    [HideInInspector] public GrabItModStatic graber;
    [HideInInspector] public ManagerItem managerItem;
    private bool canMerge = true;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        Spawn();
    }

    public void Spawn() {
        meshFilter.mesh = itemConfiguration.mesh;
        meshCollider.sharedMesh = itemConfiguration.mesh;
    }

    private void OnCollisionEnter(Collision collision) {
        if ((maskItem & (1 << collision.gameObject.layer)) != 0) {
            if (collision.gameObject.TryGetComponent<Item>(out Item otherItem)) {
                if (canMerge && itemConfiguration == otherItem.itemConfiguration) {
                    otherItem.ItemMerge();
                    ItemMerge();
                }
            }
        }
    }

    public void ItemMerge() {
        graber?.StopGrab();
        canMerge = false;
        CreateDestoyEffect();
    }

    private void CreateDestoyEffect() {
        Instantiate(partDestroy, transform.position, Quaternion.identity);
        Destroy(gameObject);
        managerItem?.DestroyItem();
    }
}
