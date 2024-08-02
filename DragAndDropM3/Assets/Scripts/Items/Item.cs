using UnityEngine;

public class Item : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    [SerializeField] private ItemConfiguration itemConfiguration;
    [SerializeField] private LayerMask maskItem;
    [SerializeField] private ParticleSystem partDestroy;
    private bool canMerge = true;
    private GrabItModStatic graber;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter.mesh = itemConfiguration.mesh;
        meshCollider.sharedMesh = itemConfiguration.mesh;
    }

    private void OnCollisionEnter(Collision collision) {
        if ((maskItem & (1 << collision.gameObject.layer)) != 0) {
            if (collision.gameObject.TryGetComponent<Item>(out Item otherItem)) {
                if (canMerge && itemConfiguration == otherItem.GetItemConfiguration()) {
                    otherItem.ItemMerge();
                    ItemMerge();
                }
            }
        }
    }

    public void SetGraber(GrabItModStatic _graber) { 
        graber = _graber;
    }

    public void ItemMerge() {
        graber?.StopGrab();
        canMerge = false;
        CreateDestoyEffect();
    }

    private void CreateDestoyEffect() {
        Instantiate(partDestroy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #region Return values
    public ItemConfiguration GetItemConfiguration() {
        return itemConfiguration;
    }
    #endregion
}
