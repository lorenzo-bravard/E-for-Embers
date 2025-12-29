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

    private void Start()
    {
        trainRoot = transform.parent;

        // Set initial position and rotation in local train space
        transform.localPosition = initialLocalPosition;
        transform.localRotation = Quaternion.Euler(initialLocalRotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("TrainPlant: No Rigidbody found. Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
    }

    private void LateUpdate()
    {
        if (player == null || Camera.main == null) return;

        Transform cam = Camera.main.transform;
        float distanceToPlant = Vector3.Distance(player.position, transform.position);

        // Pick up
        if (!isHeld && !isPlaced && distanceToPlant <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            PickUp(cam);
        }
        // Place
        else if (isHeld && Input.GetKeyDown(interactKey))
        {
            PlaceOnGround(cam);
        }

        // Keep it in front of camera
        if (isHeld)
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
        rb.isKinematic = false;

        Vector3 forwardTargetWorld = cam.position + cam.forward * placeDistance;

        Vector3 localTarget = trainRoot.InverseTransformPoint(forwardTargetWorld);

        localTarget.x = Mathf.Clamp(localTarget.x, clampX.x, clampX.y);
        localTarget.z = Mathf.Clamp(localTarget.z, clampZ.x, clampZ.y);
        localTarget.y = floorHeight;

        transform.position = trainRoot.TransformPoint(localTarget);
        transform.rotation = Quaternion.Euler(initialLocalRotation);
        transform.SetParent(trainRoot);

        if (playerObject != null)
        {
            playerObject.GetComponent<PlayerManager>().PlayPlantTrack();
            colorManager.GetComponent<ColorManager>().colorProgress = 0.80f;
        }
        fog3.Stop();
        fog4.Stop();
        thunder4.Stop();
        thunder5.Stop();
        thunderstromAudio.Stop();

        for (int i = 0; i < fireParticleParents.Length; i++)
        {
            foreach (ParticleSystem ps in fireParticleParents[i].GetComponentsInChildren<ParticleSystem>(true))
            {
                ps.Stop();
            }
        }
    }


}
