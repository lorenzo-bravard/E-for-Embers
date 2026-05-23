using UnityEngine;

[ExecuteAlways]
public class GodRayController : MonoBehaviour
{
    [Header("Réglages")]
    public Transform sun;
    public float maxDensity = 10f;
    public float fadeSpeed = 5f;

    [Header("Référence au Cube (L'enfant)")]
    public Renderer beamRenderer; // Glisse le Cube "GodRay_Volume" ici !

    [Header("Obstacles")]
    public LayerMask obstacleMask;

    private Material mat;
    private int sunDirID;
    private int densityID;
    private float currentDensity = 0f;

    void OnEnable()
    {
        // On récupère le material depuis l'enfant qu'on a assigné
        if (beamRenderer != null) mat = beamRenderer.sharedMaterial; // Shared pour voir en edit mode

        sunDirID = Shader.PropertyToID("_SunDirection");
        densityID = Shader.PropertyToID("_Density");
    }

    void Update()
    {
        if (sun == null || beamRenderer == null) return;
        if (mat == null) mat = beamRenderer.sharedMaterial;

        // --- 1. ROTATION PHYSIQUE (Le Télescope) ---
        // On aligne l'axe Z (bleu) du pivot avec les rayons du soleil
        // -sun.forward donne la direction D'OU vient la lumière
        if (sun.forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-sun.forward);
        }

        // --- 2. CONFIG DU SHADER ---
        // Maintenant que la boite est alignée physiquement avec le soleil,
        // la lumière vient toujours de "Devant" (Z) par rapport à la boite !
        // On envoie donc une direction locale fixe (0, 0, 1).
        if (mat != null)
        {
            mat.SetVector(sunDirID, Vector3.forward);
        }

        // --- 3. GESTION DES OBSTACLES (Raycast) ---
        float targetDensity = 0f;
        float sunHeight = -sun.forward.y; // > 0 le jour

        if (sunHeight > 0)
        {
            // Le Raycast part du pivot (le sol) vers le soleil
            bool isBlocked = Physics.Raycast(transform.position, -sun.forward, Mathf.Infinity, obstacleMask);

            if (!isBlocked)
            {
                targetDensity = sunHeight * maxDensity;
            }
        }

        // --- 4. TRANSITION DOUCE ---
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
            // Ligne de détection (Debug)
            bool isBlocked = Physics.Raycast(transform.position, -sun.forward, Mathf.Infinity, obstacleMask);
            Gizmos.color = isBlocked ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (-sun.forward * 50f));
        }
    }
}