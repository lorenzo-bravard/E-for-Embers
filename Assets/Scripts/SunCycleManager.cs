using UnityEngine;

public class SunCycleManager : MonoBehaviour
{
    public Light sunLight;

    [Header("Reglages angles")]
    public float angleStart = -10f; // Sous l'horizon donc il fait nuit
    public float angleEnd = 50f;    // Haut dans le ciel

    [Header("Intensité Soleil")]
    public float intensityStart = 0f;   // Pas de lumiere
    public float intensityEnd = 1.5f;   // Eclatant

    [Header("Intensité Ambiante")]
    public float ambientStart = 0f;     // Noir total
    public float ambientEnd = 1f;       // Normal

    public void SetSunTime(float val)
    {
        float newAngleX = Mathf.Lerp(angleStart, angleEnd, val);
        float currentY = sunLight.transform.rotation.eulerAngles.y;

        sunLight.transform.rotation = Quaternion.Euler(newAngleX, currentY, 0);

        sunLight.intensity = Mathf.Lerp(intensityStart, intensityEnd, val);

        RenderSettings.ambientIntensity = Mathf.Lerp(ambientStart, ambientEnd, val);
    }
}