using UnityEngine;

public class VRGrabbableExtension : MonoBehaviour
{
    private Transform _originalParent;
    private OVRGrabbable _grabbable;
    private Rigidbody _rb;

    void Start()
    {
        _grabbable = GetComponent<OVRGrabbable>();
        _rb = GetComponent<Rigidbody>();
        _originalParent = transform.parent;
    }

    void Update()
    {
        if (_grabbable != null)
        {
            if (_grabbable.isGrabbed)
            {
                // When grabbed, we might want to ensure it's unparented or parented to hand
                // OVRGrabbable usually handles parenting to the grabber.
            }
            else
            {
                // When released, restore original parent (the Train)
                if (transform.parent != _originalParent)
                {
                    transform.SetParent(_originalParent);
                    
                    // Reset kinematic state if needed
                    if (_rb != null)
                    {
                        _rb.isKinematic = true;
                        _rb.useGravity = false;
                    }
                }
            }
        }
    }
}
