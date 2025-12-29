using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("Train Movement Settings")]
    public SplineContainer splineContainer;
    public float trainSpeed = 5f;
    public float yOffset = 0.4f;

    [Header("Player Movement Settings")]
    public float moveSpeed = 3f;
    public float minXOffset = 0.1f;
    public float maxXOffset = 5f; // Front/back bounds
    public float maxZOffset = 0.25f; // Left/right bounds
    private float distanceTravelled = 0f;
    private float splineLength;
    private Vector3 currentLocalOffset;

    [Header("Audio")]
    public AudioClip track1;
    public AudioSource trackSource1;
    public AudioClip track2;
    public AudioSource trackSource2;
    public AudioClip track3;
    public AudioSource trackSource3;
    public AudioClip track4;
    public AudioSource trackSource4;
    public AudioClip theme;
    public AudioSource themeSource;

    void Start()
    {
        splineLength = splineContainer.CalculateLength();
        currentLocalOffset = new Vector3(5, yOffset, 0);
        transform.localPosition = currentLocalOffset;
    }

    void Update()
    {
        HandlePlayerMovement();
        UpdateTrainPosition();
    }

    void HandlePlayerMovement()
    {
        Transform cameraTransform = Camera.main.transform;

        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Build direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten vectors (remove vertical tilt)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * vertical + right * horizontal).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            Vector3 movement = moveDir * moveSpeed * Time.deltaTime;

            // Apply movement in local space relative to train
            Vector3 localMovement = transform.parent.InverseTransformDirection(movement);
            currentLocalOffset += localMovement;

            // Clamp offsets to bounds inside train
                currentLocalOffset.x = Mathf.Clamp(currentLocalOffset.x, minXOffset, maxXOffset);
            currentLocalOffset.z = Mathf.Clamp(currentLocalOffset.z, -maxZOffset, maxZOffset);

            transform.localPosition = currentLocalOffset;
        }
    }


    void UpdateTrainPosition()
    {
        distanceTravelled += trainSpeed * Time.deltaTime;
        distanceTravelled = Mathf.Repeat(distanceTravelled, splineLength);
        float normalizedT = distanceTravelled / splineLength;

        Vector3 pos = splineContainer.EvaluatePosition(normalizedT);
        Vector3 tangent = splineContainer.EvaluateTangent(normalizedT);
        Vector3 flattenedDir = new Vector3(tangent.x, 0, tangent.z).normalized;

        transform.parent.position = pos;
        transform.parent.rotation = Quaternion.LookRotation(flattenedDir);
    }

    public void PlayDrillMusicSequential()
    {
        if (track1 != null)
        {
            trackSource1.clip = track1;
            trackSource1.loop = false;
            trackSource1.Play();

            if (theme != null)
            {
                StartCoroutine(PlaySecondTrackAfterFirst());
            }
        }
    }

    public void PlayBookTrack()
    {
        if(track2 != null && trackSource2 != null)
        {
            trackSource2.clip = track2;
            trackSource2.loop = false;
            trackSource2.Play();
        }
    }

    public void PlayTapeTrack()
    {
        if (track3 != null && trackSource3 != null)
        {
            trackSource3.clip = track3;
            trackSource3.loop = false;
            trackSource3.Play();
        }
    }

    public void PlayPlantTrack()
    {
        if (track4 != null && trackSource4 != null)
        {
            trackSource4.clip = track4;
            trackSource4.loop = false;
            trackSource4.Play();
        }
    }

    private IEnumerator PlaySecondTrackAfterFirst()
    {
        yield return new WaitForSeconds(track1.length + 0.25f);
        themeSource.clip = theme;
        themeSource.loop = true;
        themeSource.volume = 0.2f;
        themeSource.Play();
    }
}