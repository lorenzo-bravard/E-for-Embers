using UnityEngine;

public class CubeTarget : MonoBehaviour
{
    public ParticleSystem systemeParticules;

    public GameObject objectToShow;

    public GameObject playerObject;

    public ColorManager colorManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp0"))
        {
            if (systemeParticules != null)
            {
                systemeParticules.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            
            Debug.Log("Objet 'canPickUp1' détecté → destruction");

            objectToShow.SetActive(true);
                
            Destroy(other.gameObject, 0.01f);

            playerObject.GetComponent<PlayerManager>().PlayDrillMusicSequential();
            colorManager.GetComponent<ColorManager>().colorProgress = 0.15f;


            Debug.Log("STITUI Trigger détecté avec : " + other.name + ", tag : " + other.tag);

        }
    }
}
