using UnityEngine;

public class TrainPlant : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Initial Position Settings")]
    public Vector3 initialLocalPosition;
    public Vector3 initialLocalRotation;

    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Hover & Placement Settings")]
    public Vector3 hoverOffset = new Vector3(0f, -0.3f, 0.6f);
    public float placeDistance = 1f;
    public float floorHeight = 0.49f;

    [Header("Train Boundaries (Local Space)")]
    public Vector2 clampX = new Vector2(-1.5f, 1.5f);  // Left/Right limit
    public Vector2 clampZ = new Vector2(-4f, 4f);      // Front/Back limit


    private bool isHeld = false;
    private bool isPlaced = false;
    private Rigidbody rb;
    private Transform trainRoot;

    public GameObject playerObject;

    public ColorManager colorManager;

    public ParticleSystem fog3, fog4, thunder4, thunder5;

    public AudioSource thunderstromAudio;

    public GameObject[] fireParticleParents;

    private bool wasGrabbed = false;
    private OVRGrabbable grabbable;

    private void Start()
    {
        trainRoot = transform.parent;
        grabbable = GetComponent<OVRGrabbable>();

        // Set initial position and rotation in local train space
        transform.localPosition = initialLocalPosition;
        transform.localRotation = Quaternion.Euler(initialLocalRotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("TrainPlant: No Rigidbody found. Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
    }

    private void LateUpdate()
    {
        // VR Release Detection
        if (grabbable != null)
        {
            if (grabbable.isGrabbed)
            {
                wasGrabbed = true;
                isHeld = true; // Sync for consistency
            }
            else if (wasGrabbed)
            {
                wasGrabbed = false;
                isHeld = false;
                Debug.Log("[TrainPlant] VR Release detected. Triggering cleanup.");
                // For VR, we assume releasing it means it's "placed" if we want it to work like before
                // Alternatively, we could check if it's near the ground, but let's keep it simple as requested
                PlaceOnGround(null); 
            }
        }

        if (player == null || Camera.main == null) return;

        Transform cam = Camera.main.transform;
        float distanceToPlant = Vector3.Distance(player.position, transform.position);

        // Pick up (Desktop)
        if (!isHeld && !isPlaced && distanceToPlant <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            PickUp(cam);
        }
        // Place (Desktop)
        else if (isHeld && !wasGrabbed && Input.GetKeyDown(interactKey))
        {
            PlaceOnGround(cam);
        }

        // Keep it in front of camera (Desktop only)
        if (isHeld && !wasGrabbed)
        {
            Vector3 hoverPosition = cam.position + cam.forward * hoverOffset.z + cam.up * hoverOffset.y + cam.right * hoverOffset.x;
            transform.position = hoverPosition;
            transform.rotation = Quaternion.Euler(initialLocalRotation);
        }
    }

    private void PickUp(Transform cam)
    {
        isHeld = true;
        rb.isKinematic = true;
        isPlaced = false;
        transform.SetParent(null);
        transform.position = cam.position + cam.forward * hoverOffset.z;
    }

    private void PlaceOnGround(Transform cam)
    {
        isHeld = false;
        isPlaced = true;
        rb.isKinematic = true; // Keep kinematic as requested earlier

        if (cam != null) // Desktop specific placement
        {
            Vector3 forwardTargetWorld = cam.position + cam.forward * placeDistance;
            Vector3 localTarget = trainRoot.InverseTransformPoint(forwardTargetWorld);

            localTarget.x = Mathf.Clamp(localTarget.x, clampX.x, clampX.y);
            localTarget.z = Mathf.Clamp(localTarget.z, clampZ.x, clampZ.y);
            localTarget.y = floorHeight;

            transform.position = trainRoot.TransformPoint(localTarget);
            transform.rotation = Quaternion.Euler(initialLocalRotation);
            transform.SetParent(trainRoot);
        }
        else // VR specific or generic release
        {
            // Just ensure it's parented back to train if it wasn't
            if (transform.parent != trainRoot) transform.SetParent(trainRoot);
        }

        if (playerObject != null)
        {
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayPlantStep();
            }
            else
            {
                var pm = playerObject.GetComponent<PlayerManager>();
                if (pm != null)
                {
                    pm.PlayPlantTrack();
                }
                else
                {
                    var vpm = playerObject.GetComponent<VRPlayerManager>();
                    if (vpm != null) vpm.PlayPlantTrack();
                }
            }
            
            if (colorManager != null)
                colorManager.colorProgress = 0.80f;
        }

        if (fog3 != null) fog3.Stop();
        if (fog4 != null) fog4.Stop();
        if (thunder4 != null) thunder4.Stop();
        if (thunder5 != null) thunder5.Stop();
        if (thunderstromAudio != null) thunderstromAudio.Stop();

        if (fireParticleParents != null)
        {
            for (int i = 0; i < fireParticleParents.Length; i++)
            {
                if (fireParticleParents[i] == null) continue;
                foreach (ParticleSystem ps in fireParticleParents[i].GetComponentsInChildren<ParticleSystem>(true))
                {
                    ps.Stop();
                }
            }
        }

        // Fallback: Stop all PS with 'fire', 'flame', or 'burn' in their name in the scene
        foreach (var ps in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include))
        {
            string psName = ps.name.ToLower();
            if (psName.Contains("fire") || psName.Contains("flame") || psName.Contains("burn")) 
                ps.Stop();
        }
    }


}
