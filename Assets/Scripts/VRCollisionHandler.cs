using UnityEngine;

public class VRCollisionHandler : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public Transform cameraRig;
    public Transform centerEyeAnchor;

    [Header("Settings")]
    public float maxDistance = 0.5f;
    public bool pushBackEnabled = true;

    private void Start()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (cameraRig == null) cameraRig = transform.Find("OVRCameraRig");
    }

    private void LateUpdate()
    {
        if (!pushBackEnabled || characterController == null || centerEyeAnchor == null || cameraRig == null)
            return;

        // Calculate horizontal offset between HMD and CharacterController
        Vector3 hmdPosition = centerEyeAnchor.position;
        Vector3 ccPosition = transform.position; // CharacterController is on this GameObject
        
        // Flatten to horizontal plane (XZ)
        Vector2 hmdPosXZ = new Vector2(hmdPosition.x, hmdPosition.z);
        Vector2 ccPosXZ = new Vector2(ccPosition.x, ccPosition.z);

        float distance = Vector2.Distance(hmdPosXZ, ccPosXZ);

        if (distance > maxDistance)
        {
            // The player has walked physically too far from the virtual body (which is blocked by walls)
            // We need to shift the CameraRig to bring the HMD back towards the CC
            Vector2 direction = (hmdPosXZ - ccPosXZ).normalized;
            Vector2 correctionXZ = direction * (distance - maxDistance);
            
            Vector3 correction = new Vector3(correctionXZ.x, 0, correctionXZ.y);
            
            // Apply correction to the Rig to offset the physical movement
            cameraRig.position -= correction;
        }
    }
}
