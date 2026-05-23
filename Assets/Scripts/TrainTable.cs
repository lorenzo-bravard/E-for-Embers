using UnityEngine;

public class TrainTable : MonoBehaviour
{
    [Header("Table Settings")]
    public Vector3 tableOffset = new Vector3(0, 0.7f, -3f);
    public float maxTiltAngle = 5f;
    public Material highlightMaterial;

    private Material originalMaterial;
    private Renderer tableRenderer;
    private Transform trainParent;
    private TrainManager trainManager;

    void Start()
    {
        trainParent = transform.parent;
        if (trainParent == null)
        {
            Debug.LogError("Table must be a child of the train!");
            return;
        }

        trainManager = trainParent.GetComponent<TrainManager>();
        transform.localPosition = tableOffset;
        transform.localRotation = Quaternion.identity;

        tableRenderer = GetComponent<Renderer>();
        if (tableRenderer) originalMaterial = tableRenderer.material;
    }

    void Update()
    {
        if (trainManager == null) return;

        float tiltAmount = -trainManager.GetCurrentCurvature() * maxTiltAngle;
        transform.localRotation = Quaternion.Euler(0, 0, tiltAmount);

        transform.localPosition = tableOffset;
    }

    public void SetHighlight(bool highlight)
    {
        if (tableRenderer && highlightMaterial)
        {
            tableRenderer.material = highlight ? highlightMaterial : originalMaterial;
        }
    }
}