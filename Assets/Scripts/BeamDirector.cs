using UnityEngine;

public class BeamDirector : MonoBehaviour
{
    [Header("Réglages")]
    public Transform sunTransform; 
    public Transform windowFrame;  

    [Header("Ajustements")]
    [Range(0f, 1f)] public float maxIntensity = 1f;

    private Material mat;
    private int colorID;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        colorID = Shader.PropertyToID("_BaseColor");

        if (windowFrame == null) windowFrame = transform.parent;
    }

    void Update()
    {
        if (sunTransform == null) return;

        transform.rotation = sunTransform.rotation;

        Vector3 windowDir = windowFrame.forward;
        Vector3 sunDir = -sunTransform.forward; 

        float facingFactor = Vector3.Dot(windowDir, sunDir);

        float intensity = Mathf.Clamp01(facingFactor);

        Color col = mat.GetColor(colorID);
        col.a = intensity * maxIntensity;
        mat.SetColor(colorID, col); 
    }
}