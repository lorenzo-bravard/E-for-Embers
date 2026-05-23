using System.Collections;
using UnityEngine;

public class TrainBook : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Position Settings")]
    public Vector3 initialLocalPosition;        
    public Vector3 initialLocalRotation;      
    public Vector3 hoverOffset = new Vector3(0, 1.5f, 0.5f);   
    public Vector3 tableLocalOffset;            

    [Header("Settings")]
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    private bool isHeld = false;
    private bool isPlaced = false;
    private Rigidbody rb;
    private Transform trainRoot;

    private Quaternion initialRotation;

    public GameObject objectToShow;

    public GameObject playerObject;

    public ColorManager colorManager;

    public ParticleSystem fog1;

    public ParticleSystem thunder1;

    public GameObject[] fireParticleParents;


    private void Start()
    {
        trainRoot = transform.parent;

        transform.localPosition = initialLocalPosition;
        transform.localRotation = Quaternion.Euler(initialLocalRotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("BookInteraction: No Rigidbody found on the book. Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;

        initialRotation = Quaternion.Euler(initialLocalRotation);
    }

    private void LateUpdate()
    {
        if (isPlaced || player == null) return;

        float distanceToBook = Vector3.Distance(player.position, transform.position);
        float distanceToTable = Vector3.Distance(player.position, trainRoot.TransformPoint(tableLocalOffset));

        if (!isHeld && distanceToBook <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            PickUpBook();
        }
        else if (isHeld && distanceToTable <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(PlaceBookOnTable());
        }

        if (isHeld && !isPlaced)
        {
            Transform cam = Camera.main.transform;
            transform.position = cam.position + cam.forward * hoverOffset.z + cam.up * hoverOffset.y + cam.right * hoverOffset.x;
            transform.rotation = Quaternion.LookRotation(cam.forward) * Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void PickUpBook()
    {
        isHeld = true;
        rb.isKinematic = true;
        transform.SetParent(null);
    }

    private IEnumerator PlaceBookOnTable()
    {
        yield return new WaitForSeconds(1f);

        isHeld = false;
        isPlaced = true;

        transform.SetParent(trainRoot);
        transform.position = trainRoot.TransformPoint(tableLocalOffset);
        transform.rotation = trainRoot.rotation * initialRotation;
        objectToShow.SetActive(true);
        rb.isKinematic = true;

        // 🎵 Optional: Play music
        if (playerObject != null)
        {
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayBookStep();
            }
            else
            {
                var pm = playerObject.GetComponent<PlayerManager>();
                if (pm != null)
                {
                    pm.PlayBookTrack();
                }
                else
                {
                    var vpm = playerObject.GetComponent<VRPlayerManager>();
                    if (vpm != null) vpm.PlayBookTrack();
                }
            }
            
            if (colorManager != null)
                colorManager.colorProgress = 0.40f;
        }

        fog1.Stop();
        thunder1.Stop();

        for (int i = 0; i < fireParticleParents.Length; i++)
        {
            foreach (ParticleSystem ps in fireParticleParents[i].GetComponentsInChildren<ParticleSystem>(true))
            {
                ps.Stop();
            }
        }
    }

}
