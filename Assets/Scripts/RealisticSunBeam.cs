using UnityEngine;

public class RealisticSunBeam : MonoBehaviour
{
    [Header("Liens")]
    public Transform sunTransform;      
    public Transform trainWindowFrame;  

    [Header("Apparence")]
    public Renderer beamRenderer;       

    [Header("Couleurs (Gérées par le ColorManager)")]
    public Color colorNight = new Color(0, 0, 0, 0);
    public Color colorDepression = new Color(0.5f, 0.5f, 0.6f, 0.05f);
    public Color colorHealing = new Color(1f, 0.9f, 0.6f, 0.2f);

    private Material mat;
    private float globalProgress = 0f;

    void Start()
    {
        if (beamRenderer != null) mat = beamRenderer.material;

        if (trainWindowFrame == null) trainWindowFrame = transform.parent;
    }

    void Update()
    {
        if (sunTransform == null) return;

        transform.rotation = sunTransform.rotation;

        float dot = Vector3.Dot(trainWindowFrame.forward, -sunTransform.forward);

        float visibility = Mathf.Clamp01(dot);

        Color targetColor = Color.Lerp(colorDepression, colorHealing, globalProgress);

        if (globalProgress < 0.1f) targetColor = Color.Lerp(colorNight, targetColor, globalProgress * 10);

        targetColor.a *= visibility;

        if (mat != null) mat.SetColor("_LightColor", targetColor);
    }

    public void SetProgress(float progress)
    {
        globalProgress = progress;
    }
}