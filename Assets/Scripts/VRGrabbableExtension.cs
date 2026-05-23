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

            }
            else
            {

                if (transform.parent != _originalParent)
                {
                    transform.SetParent(_originalParent);
                    

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
