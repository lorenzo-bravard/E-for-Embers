using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSound : MonoBehaviour
{
    public AudioSource audioSource;

    ParticleSystem ps;
    int lastParticleCount;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        lastParticleCount = ps.particleCount;
    }

    void Update()
    {
        int currentCount = ps.particleCount;

        // If particles increased, something was emitted
        if (currentCount > lastParticleCount)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        lastParticleCount = currentCount;
    }
}
