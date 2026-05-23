using UnityEngine;

public class TriggerTarget : MonoBehaviour
{
    [Header("Detection Settings")]
    public string targetTag = "Livre";
    public ParticleSystem successEffect;

    [Header("Activation Settings")]
    public GameObject objectToShow;
    public GameObject playerObject;
    public float colorProgressValue = 0.40f;

    [Header("Environmental Cleanup")]
    public ParticleSystem[] fogSystems;
    public ParticleSystem[] thunderSystems;
    public AudioSource thunderstormAudio;
    public GameObject[] fireParticleParents;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Check if the object is being held by OVRGrabber
            var grabbable = other.GetComponent<OVRGrabbable>();
            if (grabbable != null && grabbable.isGrabbed)
            {
                return; // Don't trigger while the player is holding it
            }

            Debug.Log($"[TriggerTarget] {targetTag} detected and released on {gameObject.name}");
            ExecuteActivation(other.gameObject);
        }
    }

    private void ExecuteActivation(GameObject targetObj)
    {
        if (successEffect != null)
        {
            successEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        StopEnvironmentEffects();

        // Show next object
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
            Debug.Log($"[TriggerTarget] Activated {objectToShow.name}");
        }

        // Update progress and play music
        if (playerObject != null)
        {
            var colorManager = Object.FindAnyObjectByType<ColorManager>();
            if (colorManager != null) colorManager.colorProgress = colorProgressValue;

            if (GameAudioManager.Instance != null)
            {
                if (targetTag == "Livre") GameAudioManager.Instance.PlayBookStep();
                else if (targetTag == "Plant") GameAudioManager.Instance.PlayPlantStep();
                else if (targetTag == "Tape") GameAudioManager.Instance.PlayTapeStep();
            }
            else
            {
                // Fallback to old manager logic
                var manager = playerObject.GetComponent<PlayerManager>();
                if (manager != null)
                {
                    manager.PlayBookTrack();
                }
                else
                {
                    // Try VR manager
                    var vpm = playerObject.GetComponent<VRPlayerManager>();
                    if (vpm != null) vpm.PlayBookTrack();
                }
            }
        }

        // Destroy the placed object
        Destroy(targetObj, 0.1f);
    }

    private void StopEnvironmentEffects()
    {
        if (fogSystems != null)
        {
            foreach (var ps in fogSystems) if (ps != null) ps.Stop();
        }

        if (thunderSystems != null)
        {
            foreach (var ps in thunderSystems) if (ps != null) ps.Stop();
        }

        if (thunderstormAudio != null)
        {
            thunderstormAudio.Stop();
        }

        if (fireParticleParents != null)
        {
            foreach (var parent in fireParticleParents)
            {
                if (parent == null) continue;
                foreach (var ps in parent.GetComponentsInChildren<ParticleSystem>(true))
                {
                    ps.Stop();
                }
            }
        }
    }
}
