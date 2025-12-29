using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SmartVolumetricBeam : MonoBehaviour
{
    [Header("Références")]
    public Transform sunTransform; 
    public Transform windowFrame;

    [Header("Couleurs")]
    public Color colorNight = new Color(0, 0, 0, 0); 
    public Color colorDepression = new Color(0.3f, 0.4f, 0.5f, 0.1f); 
    public Color colorHealing = new Color(1f, 0.9f, 0.6f, 0.3f); 

    private ParticleSystem ps;
    private ParticleSystem.MainModule main;

    private float currentProgress = 0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        if (windowFrame == null) windowFrame = transform;
    }

    void Update()
    {
        if (sunTransform == null) return;

       
        float sunFactor = Vector3.Dot(-windowFrame.forward, -sunTransform.forward);

        float shadowIntensity = Mathf.Clamp01(sunFactor);

        Color targetColor = Color.Lerp(colorDepression, colorHealing, currentProgress);

        if (currentProgress < 0.1f) targetColor = Color.Lerp(colorNight, targetColor, currentProgress * 10f);

        targetColor.a *= shadowIntensity;

        main.startColor = targetColor;
    }

    public void UpdateBeamProgress(float progress)
    {
        currentProgress = progress;
    }
}