using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class VRPlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    public float yOffset = 0.8f; // Match Remy's height

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

    private CharacterController _cc;
    private int _framesToGlue = 5; // Glue to train for first few frames

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Start()
    {
        // Force initial local height and position
        StartCoroutine(InitialGlueRoutine());
    }

    private IEnumerator InitialGlueRoutine()
    {
        // Temporarily disable CC to allow parent teleportation to propagate correctly
        if (_cc != null) _cc.enabled = false;

        // Force local position for a few frames to ensure we follow the train's initial jump
        for (int i = 0; i < _framesToGlue; i++)
        {
            // Recenter HMD to align physical position with virtual rig origin
            // Use displaying status to avoid errors in Editor without headset
            if (i == 0 && OVRManager.display != null) OVRManager.display.RecenterPose();

            Vector3 pos = transform.localPosition;
            pos.y = yOffset;
            transform.localPosition = pos;
            
            Physics.SyncTransforms();
            yield return null;
        }

        if (_cc != null) _cc.enabled = true;
        Debug.Log($"[VRPlayerManager] Player successfully glued to train at launch. Local Y: {transform.localPosition.y}");
    }

    void LateUpdate()
    {
        // Safety height check to prevent falling through floor
        Vector3 localPos = transform.localPosition;
        if (Mathf.Abs(localPos.y - yOffset) > 0.1f)
        {
            localPos.y = yOffset;
            transform.localPosition = localPos;
        }
    }

    // Music methods maintained for compatibility
    public void PlayDrillMusicSequential()
    {
        if (track1 != null && trackSource1 != null)
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
        if (themeSource != null)
        {
            themeSource.clip = theme;
            themeSource.loop = true;
            themeSource.volume = 0.2f;
            themeSource.Play();
        }
    }
}
