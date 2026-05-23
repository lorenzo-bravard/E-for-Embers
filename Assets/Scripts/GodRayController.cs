using UnityEngine;

[ExecuteAlways]
public class GodRayController : MonoBehaviour
{
    [Header("Réglages")]
    public Transform sun;
    public float maxDensity = 10f;
    public float fadeSpeed = 5f;

    [Header("Référence au Cube (L'enfant)")]
    public Renderer beamRenderer; 

    [Header("Obstacles")]
    public LayerMask obstacleMask;

    private Material mat;
    private int sunDirID;
    private int densityID;
    private float currentDensity = 0f;

    void OnEnable()
    {
        if (beamRenderer != null) mat = beamRenderer.sharedMaterial;

        sunDirID = Shader.PropertyToID("_SunDirection");
        densityID = Shader.PropertyToID("_Density");
    }

    void Update()
    {
        if (sun == null || beamRenderer == null) return;
        if (mat == null) mat = beamRenderer.sharedMaterial;


        if (sun.forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-sun.forward);
        }


        if (mat != null)
        {
            mat.SetVector(sunDirID, Vector3.forward);
        }

        float targetDensity = 0f;
        float sunHeight = -sun.forward.y;

        if (sunHeight > 0)
        {
            bool isBlocked = Physics.Raycast(transform.position, -sun.forward, Mathf.Infinity, obstacleMask);

            if (!isBlocked)
            {
                targetDensity = sunHeight * maxDensity;
            }
        }

        if (Application.isPlaying)
        {
            currentDensity = Mathf.Lerp(currentDensity, targetDensity, Time.deltaTime * fadeSpeed);
        }
        else
        {
            currentDensity = targetDensity;
        }

        if (mat != null) mat.SetFloat(densityID, currentDensity);
    }

    void OnDrawGizmos()
    {
        if (sun != null)
        {
            bool isBlocked = Physics.Raycast(transform.position, -sun.forward, Mathf.Infinity, obstacleMask);
            Gizmos.color = isBlocked ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (-sun.forward * 50f));
        }
    }
}