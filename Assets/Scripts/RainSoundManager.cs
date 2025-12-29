using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource rainSound;

    void Start()
    {
        rainSound.volume = 1f;
        rainSound.Play();
    }
}
