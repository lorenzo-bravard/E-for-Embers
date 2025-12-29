using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float colorProgress = 0f;

    public Volume postProcessingVolume;

    private ColorAdjustments colorAdjustments;
    private SplitToning splitToning;

    public SunCycleManager sunManager;

    public SmartVolumetricBeam[] particleBeams;

    public Renderer[] volumetricBeams;
    public Color beamColorStart = new Color(0.3f, 0.3f, 0.35f);
    public Color beamColorEnd = new Color(1f, 0.8f, 0.4f);

    void Start()
    {
        if (!postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments not found in Volume.");
        }

        if (!postProcessingVolume.profile.TryGet(out splitToning))
        {
            Debug.LogError("Split Toning not found in Volume.");
        }
        if (sunManager != null)
        {
            sunManager.SetSunTime(colorProgress);
        }
    }

    void Update()
    {
        if (sunManager != null)
        {
            sunManager.SetSunTime(colorProgress);
        }
        foreach (var beam in particleBeams)
        {
            if (beam != null) beam.UpdateBeamProgress(colorProgress);
        }

        Color currentBeamColor = Color.Lerp(beamColorStart, beamColorEnd, colorProgress);
        foreach (var beam in volumetricBeams)
        {
            if (beam != null)
            {
                beam.material.SetColor("_BaseColor", currentBeamColor);
            }
        }

        if (colorAdjustments == null || splitToning == null) return;

        if (colorProgress < 0.5f)
        {
            float t = colorProgress / 0.5f;

            // Black & white to vintage
            colorAdjustments.saturation.value = Mathf.Lerp(-100f, -40f, t);

            // Strong warm vintage tone
            Color vintageShadows = new Color(0.6f, 0.25f, 0.1f);    // reddish-brown
            Color vintageHighlights = new Color(1.0f, 0.75f, 0.5f); // orange-yellow glow

            splitToning.shadows.value = Color.Lerp(Color.gray, vintageShadows, t);
            splitToning.highlights.value = Color.Lerp(Color.gray, vintageHighlights, t);
        }
        else
        {
            float t = (colorProgress - 0.5f) / 0.5f;

            // Vintage to color
            colorAdjustments.saturation.value = Mathf.Lerp(-40f, 0f, t);

            Color vintageShadows = new Color(0.6f, 0.25f, 0.1f);
            Color vintageHighlights = new Color(1.0f, 0.75f, 0.5f);

            splitToning.shadows.value = Color.Lerp(vintageShadows, Color.white, t);
            splitToning.highlights.value = Color.Lerp(vintageHighlights, Color.white, t);
        }
    }
}