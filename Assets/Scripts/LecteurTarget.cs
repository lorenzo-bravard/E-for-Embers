using UnityEngine;

public class LecteurTarget : MonoBehaviour
{
    public ParticleSystem fog2;

    public ParticleSystem thunder2, thunder3;

    public GameObject objectToShow;

    public GameObject playerObject;

    public ColorManager colorManager;

    public GameObject[] fireParticleParents;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger détecté avec : " + other.name + ", tag : " + other.tag);

        if (other.CompareTag("Tape"))
        {
            Debug.Log("Objet 'Livre' détecté → destruction");

            if (fog2 != null)
            {
                fog2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            Destroy(other.gameObject, 0.01f);

            if (playerObject != null)
            {
                playerObject.GetComponent<PlayerManager>().PlayTapeTrack();
                colorManager.GetComponent<ColorManager>().colorProgress = 0.60f;
            }

            objectToShow.SetActive(true);
            fog2.Stop();
            thunder2.Stop();
            thunder3.Stop();

            for (int i = 0; i < fireParticleParents.Length; i++)
            {
                foreach (ParticleSystem ps in fireParticleParents[i].GetComponentsInChildren<ParticleSystem>(true))
                {
                    ps.Stop();
                }
            }
        }
    }
}

