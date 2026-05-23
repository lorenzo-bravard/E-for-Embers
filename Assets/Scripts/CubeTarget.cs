using UnityEngine;

public class CubeTarget : MonoBehaviour
{
    public ParticleSystem systemeParticules;

    public GameObject objectToShow;

    public GameObject playerObject;

    public ColorManager colorManager;
    [Header("Environmental Cleanup")]
    public ParticleSystem[] fogSystems;
    public ParticleSystem[] thunderSystems;
    public AudioSource thunderstormAudio;
    public GameObject[] fireParticleParents;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("canPickUp0"))
        {

            var grabbable = other.GetComponent<OVRGrabbable>();
            if (grabbable != null && grabbable.isGrabbed)
            {
                return;
            }

            Debug.Log("Drill détectée et relâchée → activation");
            ExecuteActivation(other.gameObject);
        }
    }

    private void ExecuteActivation(GameObject targetObj)
    {
        if (systemeParticules != null)
        {
            systemeParticules.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        StopEnvironmentEffects();
            
        Debug.Log("Objet 'canPickUp1' détecté → destruction");

        if (objectToShow != null) objectToShow.SetActive(true);
            
        Destroy(targetObj, 0.01f);

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayDrillStep();
        }
        else
        {
   
            var pm = playerObject.GetComponent<PlayerManager>();
            if (pm != null) pm.PlayDrillMusicSequential();
            else {
                var vpm = playerObject.GetComponent<VRPlayerManager>();
                if (vpm != null) vpm.PlayDrillMusicSequential();
            }
        }

        if (colorManager != null)
            colorManager.colorProgress = 0.15f;
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
