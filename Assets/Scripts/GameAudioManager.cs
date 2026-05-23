using UnityEngine;
using System.Collections;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance { get; private set; }

    [Header("Audio Tracks")]
    public AudioClip drillTrack;
    public AudioClip bookTrack;
    public AudioClip tapeTrack;
    public AudioClip plantTrack;
    public AudioClip themeTrack;

    [Header("Audio Sources")]
    public AudioSource drillSource;
    public AudioSource bookSource;
    public AudioSource tapeSource;
    public AudioSource plantSource;
    public AudioSource themeSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize sources if not assigned
        SetupSource(ref drillSource, "DrillSource");
        SetupSource(ref bookSource, "BookSource");
        SetupSource(ref tapeSource, "TapeSource");
        SetupSource(ref plantSource, "PlantSource");
        SetupSource(ref themeSource, "ThemeSource");
        
        // Setup clips
        if (drillSource) drillSource.clip = drillTrack;
        if (bookSource) bookSource.clip = bookTrack;
        if (tapeSource) tapeSource.clip = tapeTrack;
        if (plantSource) plantSource.clip = plantTrack;
        if (themeSource) themeSource.clip = themeTrack;
    }

    private void SetupSource(ref AudioSource source, string name)
    {
        if (source == null)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(transform);
            source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
    }

    public void PlayDrillStep()
    {
        if (drillSource) drillSource.Play();
        if (themeSource) StartCoroutine(PlayThemeAfterDelay(drillTrack ? drillTrack.length : 0));
        Debug.Log("[GameAudioManager] Playing Drill Step");
    }

    private IEnumerator PlayThemeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay + 0.25f);
        if (themeSource)
        {
            themeSource.loop = true;
            themeSource.volume = 0.2f;
            themeSource.Play();
        }
    }

    public void PlayBookStep()
    {
        if (bookSource) bookSource.Play();
        Debug.Log("[GameAudioManager] Playing Book Step");
    }

    public void PlayTapeStep()
    {
        if (tapeSource) tapeSource.Play();
        Debug.Log("[GameAudioManager] Playing Tape Step");
    }

    public void PlayPlantStep()
    {
        if (plantSource) plantSource.Play();
        Debug.Log("[GameAudioManager] Playing Plant Step");
    }
}
