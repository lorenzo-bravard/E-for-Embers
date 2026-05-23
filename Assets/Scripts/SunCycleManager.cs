using UnityEngine;

public class SunCycleManager : MonoBehaviour
{
    public Light sunLight;

    [Header("Reglages angles")]
    public float angleStart = -10f; 
    public float angleEnd = 50f;   

    [Header("Intensité Soleil")]
    public float intensityStart = 0f;   
    public float intensityEnd = 1.5f;   

    [Header("Intensité Ambiante")]
    public float ambientStart = 0f;     
    public float ambientEnd = 1f;       

    public void SetSunTime(float val)
    {
        float newAngleX = Mathf.Lerp(angleStart, angleEnd, val);
        float currentY = sunLight.transform.rotation.eulerAngles.y;

        sunLight.transform.rotation = Quaternion.Euler(newAngleX, currentY, 0);

        sunLight.intensity = Mathf.Lerp(intensityStart, intensityEnd, val);

        RenderSettings.ambientIntensity = Mathf.Lerp(ambientStart, ambientEnd, val);
    }
}