using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxVerticalAngle = 80f;
    public bool invertY = false;

    private float xRotation = 0f;
    private Transform playerBody;

    [Header("Camera Layer Settings")]
    public string excludedLayerName = "TrainInterior";

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent;

        // Exclude the specified layer from camera rendering
        int excludedLayer = LayerMask.NameToLayer(excludedLayerName);
        if (excludedLayer >= 0)
        {
            Camera cam = GetComponent<Camera>();
            cam.cullingMask &= ~(1 << excludedLayer);
        }
        else
        {
            Debug.LogWarning($"Layer \"{excludedLayerName}\" not found. Did you create it?");
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1 : -1);

        // Vertical rotation (up/down)
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Horizontal rotation (left/right) - rotates entire player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
