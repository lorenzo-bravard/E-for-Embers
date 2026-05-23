using UnityEngine;

[DefaultExecutionOrder(-100)]
public class VRInteractionInitializer : MonoBehaviour
{
    public Transform rightHandAnchor;
    
    void Awake()
    {
        if (rightHandAnchor == null)
        {
            var anchors = GetComponentsInChildren<Transform>(true);
            foreach (var a in anchors) if (a.name == "RightHandAnchor") { rightHandAnchor = a; break; }
        }

        if (rightHandAnchor != null)
        {
            var grabber = rightHandAnchor.GetComponent<OVRGrabber>();
            var trigger = rightHandAnchor.GetComponent<SphereCollider>();

            if (grabber != null && trigger != null)
            {

                Debug.Log("[VRInitializer] OVRGrabber and Trigger detected.");
            }
        }
    }
}
