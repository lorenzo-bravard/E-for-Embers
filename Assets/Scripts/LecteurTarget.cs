using UnityEngine;

public class LecteurTarget : MonoBehaviour
{
    public ParticleSystem fog2;

    public ParticleSystem thunder2, thunder3;

    public GameObject objectToShow;

    public GameObject playerObject;

    public ColorManager colorManager;

    public GameObject[] fireParticleParents;

    [Header("Environmental Cleanup")]
    public ParticleSystem[] fogSystems;
    public ParticleSystem[] thunderSystems;
    public AudioSource thunderstormAudio;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tape"))
        {
            var grabbable = other.GetComponent<OVRGrabbable>();
            if (grabbable != null && grabbable.isGrabbed)
            {
                return;
            }

            Debug.Log("Objet 'Tape' détecté et relâché → activation");
            ExecuteActivation(other.gameObject);
        }
    }

    private void ExecuteActivation(GameObject targetObj)
    {
        if (fog2 != null)
        {
            fog2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        StopEnvironmentEffects();

        Destroy(targetObj, 0.01f);

        if (playerObject != null)
        {
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayTapeStep();
            }
            else
            {
                var pm = playerObject.GetComponent<PlayerManager>();
                if (pm != null)
                {
                    pm.PlayTapeTrack();
                }
                else
                {
                    var vpm = playerObject.GetComponent<VRPlayerManager>();
                    if (vpm != null) vpm.PlayTapeTrack();
                }
            }
            
            if (colorManager != null)
                colorManager.colorProgress = 0.60f;
        }

        if (objectToShow != null) objectToShow.SetActive(true);
        
        if (fog2 != null) fog2.Stop();
        if (thunder2 != null) thunder2.Stop();
        if (thunder3 != null) thunder3.Stop();

        for (int i = 0; i < fireParticleParents.Length; i++)
        {
            if (fireParticleParents[i] == null) continue;
            foreach (ParticleSystem ps in fireParticleParents[i].GetComponentsInChildren<ParticleSystem>(true))
            {
                ps.Stop();
            }
        }
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
    }
}

