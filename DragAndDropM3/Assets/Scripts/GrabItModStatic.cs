using UnityEngine;

[System.Serializable]
public class GrabObjectProperties{
	public bool useGravity = false;
	public float drag = 10;
	public float angularDrag = 10;
	public RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotation;		
}

public class GrabItModStatic : MonoBehaviour {

	[Header("Grab properties")]
	[SerializeField, Range(4,50)] private float grabSpeed = 7;
	[SerializeField, Range(4 ,25)] private float grabMaxDistance = 10;

	[Header("Affected Rigidbody Properties")]
	[SerializeField] private GrabObjectProperties grabProperties = new GrabObjectProperties();

    private GrabObjectProperties defaultProperties = new GrabObjectProperties();

	[Header("Layers")]
	[SerializeField] private LayerMask maskItem;
    [SerializeField] private LayerMask maskGround;
    [SerializeField] private Vector3 targetPosY = new Vector3(0, 0.5f, 0);

    private Rigidbody targetRB = null;
    private Transform cameraTransform;

    private Vector3 targetPos;
    private GameObject hitPointObject;
	private Item curItem;
	private SelectionOutlineController curOutline;

    private bool grabbing = false;
	private bool isHingeJoint = false;
    private LineRenderer lineRenderer;

	void Awake() {
        cameraTransform = base.transform;
		hitPointObject = new GameObject("Point");
		lineRenderer = GetComponent<LineRenderer>();
        curOutline = GetComponent<SelectionOutlineController>();

    }

	void Update() {
		if(grabbing) {
            SetTargetPosition();

            if (!isHingeJoint) {
                targetRB.constraints = grabProperties.constraints;
            }

			if( Input.GetMouseButtonUp(0) ) {
                curItem.graber = null;
                curOutline.ClearNewTarget();
                StopGrab();
            }
		}
		else {
			if(Input.GetMouseButtonDown(0))	{
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, grabMaxDistance, maskItem)) {
					if (hitInfo.collider.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                        GrabStart(rb, hitInfo.distance);
                        grabbing = true;
                    }
                    if (hitInfo.collider.TryGetComponent<Item>(out curItem)) {
                        curItem.graber = this;
                    }
					curOutline.SetNewTarget(hitInfo);
                }
			}
		}
	}

	public void StopGrab() {
        Reset();
        grabbing = false;
    }

	private void SetTargetPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, grabMaxDistance, maskGround)) {
            targetPos = hitInfo.point + targetPosY;
        }
    }
	
	void GrabStart(Rigidbody _target , float _distance) {	
		targetRB = _target;
		isHingeJoint = _target.GetComponent<HingeJoint>() != null;		

		//Rigidbody default properties	
		defaultProperties.useGravity = targetRB.useGravity;	
		defaultProperties.drag = targetRB.drag;
		defaultProperties.angularDrag = targetRB.angularDrag;
        defaultProperties.constraints = targetRB.constraints;

		//Grab Properties	
		targetRB.useGravity = grabProperties.useGravity;
		targetRB.drag = grabProperties.drag;
		targetRB.angularDrag = grabProperties.angularDrag;
        targetRB.constraints = isHingeJoint? RigidbodyConstraints.None : grabProperties.constraints;

        hitPointObject.transform.SetParent(_target.transform);

		SetTargetPosition();

        hitPointObject.transform.position = targetPos;
		hitPointObject.transform.LookAt(cameraTransform);
				
	}

	void Reset() {		
		//Grab Properties	
		targetRB.useGravity = defaultProperties.useGravity;
		targetRB.drag = defaultProperties.drag;
		targetRB.angularDrag = defaultProperties.angularDrag;
        targetRB.constraints = defaultProperties.constraints;
        targetRB = null;
		hitPointObject.transform.SetParent(null);

		if(lineRenderer != null)
			lineRenderer.enabled = false;
	}

	void Grab() {
		Vector3 hitPointPos = hitPointObject.transform.position;
		Vector3 dif = targetPos - hitPointPos;

		if (isHingeJoint) {
			targetRB.AddForceAtPosition(grabSpeed * dif * 100, hitPointPos, ForceMode.Force);
		} else {
            targetRB.velocity = grabSpeed * dif;
        }	

		if(lineRenderer != null){
			lineRenderer.enabled = true;
			lineRenderer.SetPositions( new Vector3[]{ targetPos , hitPointPos });
		}
	}
	
	void FixedUpdate() {
		if (!grabbing) { return; }
		Grab();		
	}
}
