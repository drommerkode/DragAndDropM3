using UnityEngine;

public class Item : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    public ItemConfiguration itemConfiguration;
    [SerializeField] private LayerMask maskItem;
    [SerializeField] private ParticleSystem partDestroy;
    [HideInInspector] public GrabItModStatic graber;
    [HideInInspector] public ManagerLevel managerItem;
    private bool canMerge = false;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        ManagerGame.OnLevelLoaded.AddListener(Activate);
        Spawn();
    }

    private void Activate() {
        canMerge = true;
    }

    public void Spawn() {
        meshFilter.mesh = itemConfiguration.mesh;
        meshCollider.sharedMesh = itemConfiguration.mesh;
    }

    private void OnCollisionEnter(Collision collision) {
        if ((maskItem & (1 << collision.gameObject.layer)) != 0) {
            if (collision.gameObject.TryGetComponent<Item>(out Item otherItem)) {
                if (canMerge && itemConfiguration == otherItem.itemConfiguration) {
                    otherItem.ItemMerge(false, Vector3.zero);
                    ItemMerge(true, collision.contacts[0].point);
                }
            }
        }
    }

    public void ItemMerge(bool _merge, Vector3 _collisionPoint) {
        graber?.StopGrab();
        canMerge = false;
        CreateDestoyEffect(_merge);
    }

    private void CreateDestoyEffect(bool _merge) {
        Instantiate(partDestroy, transform.position, Quaternion.identity);
        Destroy(gameObject);
        managerItem?.DestroyItem(_merge, transform.position + Vector3.up * 0.5f);
    }
}
