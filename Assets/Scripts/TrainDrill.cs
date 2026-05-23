using System.Collections;
using UnityEngine;

public class TrainDrill : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Position Settings")]
    public Vector3 initialLocalPosition;
    public Vector3 initialLocalRotation;
    public Vector3 hoverOffset = new Vector3(0, 0.2f, 0.5f);
    public Vector3 cockpitLocalOffset;

    [Header("Settings")]
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    private bool isHeld = false;
    private bool isPlaced = false;
    private Rigidbody rb;
    private Transform trainRoot;
    private Quaternion initialRotation;

    public GameObject playerObject; // 👈 For triggering music

    public ColorManager colorManager;

    void Start()
    {
        trainRoot = transform.parent;

        transform.localPosition = initialLocalPosition;
        transform.localRotation = Quaternion.Euler(initialLocalRotation);
        initialRotation = Quaternion.Euler(initialLocalRotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
    }

    void LateUpdate()
    {
        if (isPlaced || player == null) return;

        float distanceToDrill = Vector3.Distance(player.position, transform.position);
        float distanceToCockpit = Vector3.Distance(player.position, trainRoot.TransformPoint(cockpitLocalOffset));

        if (!isHeld && distanceToDrill <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            PickUpDrill();
        }
        else if (isHeld && distanceToCockpit <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            //StartCoroutine(PlaceDrillInCockpit());
        }

        if (isHeld && !isPlaced)
        {
            Transform cam = Camera.main.transform;
            transform.position = cam.position + cam.forward * hoverOffset.z + cam.up * hoverOffset.y + cam.right * hoverOffset.x;
            transform.rotation = Quaternion.LookRotation(cam.forward) * Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void PickUpDrill()
    {
        isHeld = true;
        rb.isKinematic = true;
        transform.SetParent(null);
    }

    private IEnumerator PlaceDrillInCockpit()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Drill placed");

        isHeld = false;
        isPlaced = true;

        transform.SetParent(trainRoot);
        transform.position = trainRoot.TransformPoint(cockpitLocalOffset);
        transform.rotation = trainRoot.rotation * initialRotation;
        rb.isKinematic = true;

        // 🎵 Optional: Play music
        if (playerObject != null)
        {
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayDrillStep();
            }
            else
            {
                var pm = playerObject.GetComponent<PlayerManager>();
                if (pm != null)
                {
                    pm.PlayDrillMusicSequential();
                }
                else
                {
                    var vpm = playerObject.GetComponent<VRPlayerManager>();
                    if (vpm != null) vpm.PlayDrillMusicSequential();
                }
            }
            
            if (colorManager != null)
                colorManager.colorProgress = 0.25f;
        }
}
}
