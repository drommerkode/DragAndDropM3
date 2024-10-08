using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private MeshFilter meshFilter;

    public ItemConfiguration itemConfiguration;
    [SerializeField] private LayerMask maskItem;
    [SerializeField] private ParticleSystem partDestroy;
    [SerializeField] private float minYPos = -5f;
    [HideInInspector] public GrabItModStatic graber;
    [HideInInspector] public ManagerLevel managerItem;
    [HideInInspector] public Item doubleItem;
    private bool canMerge = false;
    private float waitStartMergeTime = 1f;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        ManagerGame.OnLevelLoaded.AddListener(Activate);
        //Spawn();
    }

    private void Activate() {
        StartCoroutine(ScartCanMergeCoroutine());
    }

    private IEnumerator ScartCanMergeCoroutine() { 
        yield return new WaitForSeconds(waitStartMergeTime);
        canMerge = true;
    }

    public void Spawn() {
        meshFilter.mesh = itemConfiguration.mesh;
        transform.localScale = itemConfiguration.scale;
        StartCoroutine(AddColliderCoroutine());
        StartCoroutine(CheckYDestroyPosition());
    }

    private IEnumerator AddColliderCoroutine() {
        yield return null;
        BoxCollider bc = gameObject.AddComponent<BoxCollider>();
        //bc.size = bc.size * itemConfiguration.colliderScale;
        bc.size = new Vector3(  bc.size.x * itemConfiguration.colliderScale.x, 
                                bc.size.y * itemConfiguration.colliderScale.y, 
                                bc.size.z * itemConfiguration.colliderScale.z);
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

    public void ItemMerge(bool _merge, Vector3 _collisionPoint, bool _heightDestroy = false) {
        graber?.StopGrab();
        canMerge = false;
        CreateDestoyEffect(_merge, _heightDestroy);
    }

    private void CreateDestoyEffect(bool _merge, bool _heightDestroy) {
        if (!_heightDestroy) {
            Instantiate(partDestroy, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        managerItem?.DestroyItem(_merge, transform.position + Vector3.up * 0.5f);
    }

    private IEnumerator CheckYDestroyPosition() { 
        WaitForSeconds wfs = new WaitForSeconds(2f);
        while (true) {
            yield return wfs;
            if (transform.position.y < minYPos) {
                doubleItem?.ItemMerge(true, Vector3.zero);
                ItemMerge(false, Vector3.zero, true);
                StopAllCoroutines();
            }
        }
    }
}
