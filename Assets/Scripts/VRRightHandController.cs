using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class VRRightHandController : MonoBehaviour
{
    public LineRenderer laserLine;
    public float laserRange = 15f;
    public LayerMask interactableLayer;
    
    private GameObject _currentHover;
    private bool _isGrabbing = false;
    private OVRGrabber _grabber;
    private MethodInfo _grabBeginMethod;
    private MethodInfo _grabEndMethod;
    private FieldInfo _grabCandidatesField;

    void Awake()
    {
        _grabber = GetComponent<OVRGrabber>();
        
        if (_grabber != null)
        {

            FieldInfo gripField = typeof(OVRGrabber).GetField("m_gripTransform", BindingFlags.Instance | BindingFlags.NonPublic);
            if (gripField != null && gripField.GetValue(_grabber) == null)
            {
                gripField.SetValue(_grabber, transform);
                Debug.Log("[VRGrab] Auto-assigned missing m_gripTransform via reflection.");
            }

   
            _grabBeginMethod = typeof(OVRGrabber).GetMethod("GrabBegin", BindingFlags.Instance | BindingFlags.NonPublic);
            _grabEndMethod = typeof(OVRGrabber).GetMethod("GrabEnd", BindingFlags.Instance | BindingFlags.NonPublic);
            _grabCandidatesField = typeof(OVRGrabber).GetField("m_grabCandidates", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }

    void Start()
    {
        if (laserLine == null) laserLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLaser();
        HandleInput();
    }

    void UpdateLaser()
    {
        if (_isGrabbing || (_grabber != null && _grabber.grabbedObject != null))
        {
            laserLine.enabled = false;
            return;
        }

        laserLine.enabled = true;

        Vector3 startPos = transform.position - transform.forward * 0.1f;
        laserLine.SetPosition(0, transform.position);
        
        RaycastHit hit;

        float sphereRadius = 0.045f;
        if (Physics.SphereCast(startPos, sphereRadius, transform.forward, out hit, laserRange, interactableLayer))
        {
            laserLine.SetPosition(1, hit.point);
            
            if (_currentHover != hit.collider.gameObject)
            {
                _currentHover = hit.collider.gameObject;
                bool isGrabbable = _currentHover.GetComponent<OVRGrabbable>() != null || _currentHover.GetComponentInParent<OVRGrabbable>() != null;
                Debug.Log("[Laser] Hit: " + _currentHover.name + (isGrabbable ? " (Grabbable)" : " (Not Grabbable)"));
                TriggerHaptic(0.1f);
            }
        }
        else
        {
            laserLine.SetPosition(1, transform.position + transform.forward * laserRange);
            _currentHover = null;
        }
    }

    void HandleInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("[Input] Index Trigger Pressed");
            TryGrab();
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("[Input] Button A Pressed");
            TryGrab();
        }
        
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("[Input] Index Trigger Released");
            TryRelease();
        }
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            Debug.Log("[Input] Button A Released");
            TryRelease();
        }
    }

    void TryGrab()
    {
        Debug.Log("[VRGrab] TryGrab triggered.");
        if (_grabber == null) return;
        if (_grabber.grabbedObject != null) return;

        
        var candidates = (Dictionary<OVRGrabbable, int>)_grabCandidatesField.GetValue(_grabber);
        if (candidates != null)
        {
            var keysToRemove = new List<OVRGrabbable>();
            foreach (var key in candidates.Keys)
            {
               
                if (key == null || !key) 
                {
                    keysToRemove.Add(key);
                    continue;
                }
                
                
                try {
                    var test = key.gameObject.activeInHierarchy;
                } catch {
                    keysToRemove.Add(key);
                }
            }
            foreach (var key in keysToRemove)
            {
                candidates.Remove(key);
                Debug.Log("[VRGrab] Cleaned up a dead candidate reference.");
            }
        }

        if (_currentHover != null)
        {
            OVRGrabbable grabbable = _currentHover.GetComponent<OVRGrabbable>();
            if (grabbable == null) grabbable = _currentHover.GetComponentInParent<OVRGrabbable>();
            
            if (grabbable != null)
            {
                Debug.Log("[VRGrab] Laser Grab attempt on: " + grabbable.name);
                
                Vector3 originalPos = grabbable.transform.position;
                Quaternion originalRot = grabbable.transform.rotation;

                
                grabbable.transform.position = transform.position;
                grabbable.transform.rotation = transform.rotation;

                if (candidates != null && !candidates.ContainsKey(grabbable)) 
                    candidates.Add(grabbable, 1);
                
                _grabBeginMethod.Invoke(_grabber, null);
                
                if (_grabber.grabbedObject != null)
                {
                    Debug.Log("[VRGrab] Grab SUCCESS: " + _grabber.grabbedObject.name);
                    
                    
                    Rigidbody heldRb = _grabber.grabbedObject.GetComponent<Rigidbody>();
                    if (heldRb != null)
                    {
                        heldRb.isKinematic = true;
                        heldRb.useGravity = false;
                    }

                    _isGrabbing = true;
                    TriggerHaptic(0.3f);
                    return;
                }
                else
                {
                    grabbable.transform.position = originalPos;
                    grabbable.transform.rotation = originalRot;
                }
            }
        }

        _grabBeginMethod.Invoke(_grabber, null);
        if (_grabber.grabbedObject != null)
        {
            Rigidbody heldRb = _grabber.grabbedObject.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldRb.isKinematic = true;
                heldRb.useGravity = false;
            }
            _isGrabbing = true;
            TriggerHaptic(0.3f);
        }
    }

    void TryRelease()
    {
        if (_grabber != null && _isGrabbing)
        {
            OVRGrabbable releasedObject = _grabber.grabbedObject;
            _grabEndMethod.Invoke(_grabber, null);
            _isGrabbing = false;

            
            if (releasedObject != null)
            {
                Rigidbody rb = releasedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    Debug.Log("[VRGrab] Released " + releasedObject.name + ". Gravity restored.");
                }
            }
        }
    }

    void TriggerHaptic(float amplitude)
    {
        OVRInput.SetControllerVibration(0.1f, amplitude, OVRInput.Controller.RTouch);
        Invoke("StopHaptic", 0.05f);
    }

    void StopHaptic()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
