using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private FirstPersonController firstPersonController;

    [Header("Other")]
    private float lastInteractTime = -1f;
    [SerializeField] private float interactCooldown = 1f;

    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f; 
    public float pickUpRange = 5f; 
    private float rotationSensitivity = 0.5f; 
    private GameObject heldObj; 
    private Rigidbody heldObjRb; 
    private bool canDrop = true; 
    private int LayerNumber; 

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("HoldLayer"); 
    }
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);
        if (playerInputHandler.InteractTriggered && (Time.time - lastInteractTime > interactCooldown)) 
        {
            lastInteractTime = Time.time;

            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {

                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (playerInputHandler.AttackTriggered && canDrop == true) 
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.TryGetComponent(out Rigidbody rb))
        {
            heldObj = pickUpObj;
            heldObjRb = rb;

            heldObjRb.useGravity = false;
            heldObjRb.angularDamping = 10f;   

            heldObj.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;

        heldObjRb.useGravity = true;
        heldObjRb.angularDamping = 0.05f;
        heldObjRb.useGravity = true;

        heldObj.transform.parent = null;
        heldObj = null;
    }
    void MoveObject()
    {
        Vector3 moveDirection = holdPos.position - heldObj.transform.position;
        float moveSpeed = 10f; 
        heldObjRb.linearVelocity = moveDirection * moveSpeed;
    }
    void RotateObject()
    {
        if (playerInputHandler.RotateTriggered)
        {
            canDrop = false; 
            firstPersonController.canRotate = false;

            float XaxisRotation = playerInputHandler.RotationInput.x * rotationSensitivity;
            float YaxisRotation = playerInputHandler.RotationInput.y * rotationSensitivity;
   
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            firstPersonController.canRotate = true;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObjRb.useGravity = true;
        heldObj.transform.parent = null;
        heldObjRb.angularDamping = 0.05f;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() 
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); 
        }
    }
}
