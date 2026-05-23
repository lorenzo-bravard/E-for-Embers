////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class PickUpScript : MonoBehaviour
////{
////    public GameObject player;
////    public Transform holdPos;
////    //if you copy from below this point, you are legally required to like the video
////    public float throwForce = 500f; //force at which the object is thrown at
////    public float pickUpRange = 5f; //how far the player can pickup the object from
////    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
////    private GameObject heldObj; //object which we pick up
////    private Rigidbody heldObjRb; //rigidbody of object we pick up
////    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
////    private int LayerNumber; //layer index

////    [Tooltip("Ne doivent pouvoir �tre touch�s que les objets sur ce layer")]
////    public LayerMask pickUpLayerMask;

////    //Reference to script which includes mouse movement of player (looking around)
////    //we want to disable the player looking around when rotating the object
////    //example below 
////    //MouseLookScript mouseLookScript;
////    void Start()
////    {
////        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""

////        //mouseLookScript = player.GetComponent<MouseLookScript>();
////    }
////    void Update()
////    {
////        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
////        {
////            if (heldObj == null) //if currently not holding anything
////            {
////                //perform raycast to check if player is looking at object within pickuprange
////                RaycastHit hit;
////                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
////                {
////                    //make sure pickup tag is attached
////                    if (hit.transform.gameObject.tag == "canPickUp0")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);
////                    }
////                    if (hit.transform.gameObject.tag == "canPickUp1")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);
////                    }
////                    if (hit.transform.gameObject.tag == "canPickUp2")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);
////                    }
////                    if (hit.transform.gameObject.tag == "canPickUp3")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);
////                    }
////                    if (hit.transform.gameObject.tag == "canPickUp4")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);
////                    }
////                    if (hit.transform.gameObject.tag == "canPickUp5")
////                    {
////                        //pass in object hit into the PickUpObject function
////                        PickUpObject(hit.transform.gameObject);

////                    }
////                }
////            }
////            else
////            {
////                if (canDrop == true)
////                {
////                    StopClipping(); //prevents object from clipping through walls
////                    DropObject();
////                }
////            }
////        }
////        if (heldObj != null) //if player is holding object
////        {
////            MoveObject(); //keep object position at holdPos
////            RotateObject();
////            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
////            {
////                StopClipping();
////                ThrowObject();
////            }

////        }
////    }
////    void PickUpObject(GameObject pickUpObj)
////    {
////        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
////        {
////            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
////            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
////            heldObjRb.isKinematic = true;
////            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
////            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
////            //make sure object doesnt collide with player, it can cause weird bugs
////            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
////        }
////    }
////    void DropObject()
////    {
////        //re-enable collision with player
////        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
////        heldObj.layer = 0; //object assigned back to default layer
////        heldObjRb.isKinematic = false;
////        heldObj.transform.parent = null; //unparent object
////        heldObj = null; //undefine game object
////    }
////    void MoveObject()
////    {
////        //keep object position the same as the holdPosition position
////        heldObj.transform.position = holdPos.transform.position;
////    }
////    void RotateObject()
////    {
////        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
////        {
////            canDrop = false; //make sure throwing can't occur during rotating

////            //disable player being able to look around
////            //mouseLookScript.verticalSensitivity = 0f;
////            //mouseLookScript.lateralSensitivity = 0f;

////            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
////            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
////            //rotate the object depending on mouse X-Y Axis
////            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
////            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
////        }
////        else
////        {
////            //re-enable player being able to look around
////            //mouseLookScript.verticalSensitivity = originalvalue;
////            //mouseLookScript.lateralSensitivity = originalvalue;
////            canDrop = true;
////        }
////    }
////    void ThrowObject()
////    {
////        //same as drop function, but add force to object before undefining it
////        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
////        heldObj.layer = 0;
////        heldObjRb.isKinematic = false;
////        heldObj.transform.parent = null;
////        heldObjRb.AddForce(transform.forward * throwForce);
////        heldObj = null;
////    }
////    void StopClipping() //function only called when dropping/throwing
////    {
////        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
////        //have to use RaycastAll as object blocks raycast in center screen
////        //RaycastAll returns array of all colliders hit within the cliprange
////        RaycastHit[] hits;
////        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
////        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
////        if (hits.Length > 1)
////        {
////            //change object position to camera position 
////            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
////            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
////        }
////    }
////}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PickUpScript : MonoBehaviour
//{
//    [Header("R�f�rences")]
//    public GameObject player;
//    public Transform holdPos;               // Doit �tre enfant de la cam�ra ou du player

//    [Header("Param�tres Pickup / Throw")]
//    public float pickUpRange = 5f;          // Port�e du pickup
//    public float throwForce = 500f;       // Force de lancement
//    public LayerMask pickUpLayerMask;       // Coche uniquement le layer "PickUp" dans l�inspector

//    [Header("Rotation de l'objet")]
//    [SerializeField]
//    private float rotationSensitivity = 1f;

//    // �tat interne
//    private GameObject heldObj;
//    private Rigidbody heldObjRb;
//    private int holdLayer;
//    private Transform originalParent;      // <� On stocke ici le parent d�origine

//    private bool canDrop = true;

//    void Start()
//    {
//        holdLayer = LayerMask.NameToLayer("holdLayer");
//    }

//    void Update()
//    {
//        // PICKUP / DROP
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            if (heldObj == null)
//            {
//                TryPickUp();
//            }
//            else if (canDrop)
//            {
//                StopClipping();
//                DropObject();
//            }
//        }

//        // MAINTIEN + ROTATION + THROW
//        if (heldObj != null)
//        {
//            MoveObject();
//            RotateObject();

//            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
//            {
//                StopClipping();
//                ThrowObject();
//            }
//        }
//    }

//    private void TryPickUp()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange, pickUpLayerMask))
//        {
//            if (hit.collider.CompareTag("canPickUp0"))
//            {
//                PickUpObject(hit.collider.gameObject);
//            }
//        }
//    }

//    private void PickUpObject(GameObject pickUpObj)
//    {
//        // 1) On sauvegarde le parent existant (souvent le train, ou d�cor mobile)
//        originalParent = pickUpObj.transform.parent;

//        // 2) On d�tache de ce parent
//        pickUpObj.transform.SetParent(null);

//        // 3) On r�cup�re le Rigidbody
//        Rigidbody rb = pickUpObj.GetComponent<Rigidbody>();
//        if (rb == null) return;

//        heldObj = pickUpObj;
//        heldObjRb = rb;

//        // 4) On passe en kinematic pour d�sactiver la physique
//        heldObjRb.isKinematic = true;

//        // 5) On rattache � holdPos et on recentre localement
//        heldObj.transform.SetParent(holdPos);
//        heldObj.transform.localPosition = Vector3.zero;
//        heldObj.transform.localRotation = Quaternion.identity;

//        // 6) On change de layer pour ignorer collision avec le joueur
//        heldObj.layer = holdLayer;
//        Physics.IgnoreCollision(
//            heldObj.GetComponent<Collider>(),
//            player.GetComponent<Collider>(),
//            true
//        );
//    }

//    private void DropObject()
//    {
//        // 1) R�active collision joueur / objet
//        Physics.IgnoreCollision(
//            heldObj.GetComponent<Collider>(),
//            player.GetComponent<Collider>(),
//            false
//        );

//        // 2) On restaure le layer et la physique
//        heldObj.layer = 0;
//        heldObjRb.isKinematic = false;

//        // 3) On rattache � son ancien parent (le train)
//        if (originalParent != null)
//            heldObj.transform.SetParent(originalParent);
//        else
//            heldObj.transform.SetParent(null);

//        // 4) Remise � z�ro de l��tat
//        heldObj = null;
//        heldObjRb = null;
//        originalParent = null;
//    }

//    private void MoveObject()
//    {
//        heldObj.transform.position = holdPos.position;
//    }

//    private void RotateObject()
//    {
//        if (Input.GetKey(KeyCode.R))
//        {
//            canDrop = false;
//            float rotX = Input.GetAxis("Mouse X") * rotationSensitivity;
//            float rotY = Input.GetAxis("Mouse Y") * rotationSensitivity;
//            heldObj.transform.Rotate(Vector3.down, rotX, Space.World);
//            heldObj.transform.Rotate(Vector3.right, rotY, Space.World);
//        }
//        else
//        {
//            canDrop = true;
//        }
//    }

//    private void ThrowObject()
//    {
//        // 1) R�active collision joueur / objet
//        Physics.IgnoreCollision(
//            heldObj.GetComponent<Collider>(),
//            player.GetComponent<Collider>(),
//            false
//        );

//        heldObj.layer = 0;
//        heldObjRb.isKinematic = false;

//        // 2) On d�tache compl�tement pour lancer
//        heldObj.transform.SetParent(null);

//        // 3) On applique la force
//        heldObjRb.AddForce(transform.forward * throwForce);

//        // 4) Reset
//        heldObj = null;
//        heldObjRb = null;
//        originalParent = null;
//    }

//    private void StopClipping()
//    {
//        float clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
//        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, clipRange);

//        if (hits.Length > 1)
//        {
//            heldObj.transform.position = transform.position + Vector3.down * 0.5f;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("R�f�rences")]
    public GameObject player;
    public Transform holdPos;             

    [Header("Param�tres Pickup / Throw")]
    public float pickUpRange = 5f;         
    public float throwForce = 500f;       
    public LayerMask pickUpLayerMask;       

    [Header("Rotation de l'objet")]
    [SerializeField]
    private float rotationSensitivity = 1f;

    // �tats internes
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private Transform originalParent;
    private int holdLayer;
    private int originalLayer;
    private bool canDrop = true;

    void Start()
    {
        holdLayer = LayerMask.NameToLayer("holdLayer");
        if (holdLayer < 0)
        {
            Debug.LogWarning("Le layer 'holdLayer' n'existe pas, on utilisera le layer Default (0).");
            holdLayer = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
                TryPickUp();
            else if (canDrop)
            {
                StopClipping();
                DropObject();
            }
        }


        if (heldObj != null)
        {
            MoveObject();
            RotateObject();
                
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    private void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange, pickUpLayerMask))
        {
            if (hit.collider.CompareTag("canPickUp0"))
                PickUpObject(hit.collider.gameObject);
            if (hit.collider.CompareTag("canPickUp1"))
                PickUpObject(hit.collider.gameObject);
        }
    }

    private void PickUpObject(GameObject pickUpObj)
    {
        originalParent = pickUpObj.transform.parent;
        pickUpObj.transform.SetParent(null);

        originalLayer = pickUpObj.layer;

        foreach (var mc in pickUpObj.GetComponentsInChildren<MeshCollider>())
            mc.convex = true;

        heldObjRb = pickUpObj.GetComponent<Rigidbody>();
        if (heldObjRb == null) return;

        heldObj = pickUpObj;

        heldObjRb.isKinematic = true;


        heldObj.transform.SetParent(holdPos);
        heldObj.transform.localPosition = Vector3.zero;
        heldObj.transform.localRotation = Quaternion.identity;

        heldObj.layer = holdLayer;
        Physics.IgnoreCollision(
            heldObj.GetComponent<Collider>(),
            player.GetComponent<Collider>(),
            true
        );
    }

    private void DropObject()
    {
        Physics.IgnoreCollision(
            heldObj.GetComponent<Collider>(),
            player.GetComponent<Collider>(),
            false
        );

        heldObj.layer = originalLayer;
        heldObjRb.isKinematic = true;

        Collider col = heldObj.GetComponent<Collider>();
        if (col != null)
        {
            // float halfHeight = col.bounds.extents.y;
            // Vector3 pos = heldObj.transform.position;
            // pos.y = Mathf.Max(pos.y, 2f + halfHeight);
            // heldObj.transform.position = pos;
        }

        heldObj.transform.SetParent(originalParent);

        heldObj = null;
        heldObjRb = null;
        originalParent = null;
    }

    private void MoveObject()
    {
        heldObj.transform.position = holdPos.position;
    }

    private void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;
            float rotX = Input.GetAxis("Mouse X") * rotationSensitivity;
            float rotY = Input.GetAxis("Mouse Y") * rotationSensitivity;
            heldObj.transform.Rotate(Vector3.down, rotX, Space.World);
            heldObj.transform.Rotate(Vector3.right, rotY, Space.World);
        }
        else
        {
            canDrop = true;
        }
    }

    private void ThrowObject()
    {
        Physics.IgnoreCollision(
            heldObj.GetComponent<Collider>(),
            player.GetComponent<Collider>(),
            false
        );

        heldObj.layer = originalLayer;
        heldObjRb.isKinematic = false;

        heldObj.transform.SetParent(null);
        heldObjRb.AddForce(transform.forward * throwForce);

        heldObj = null;
        heldObjRb = null;
        originalParent = null;
    }

    private void StopClipping()
    {
        float clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, clipRange);
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + Vector3.down * 0.5f;
        }
    }
}
