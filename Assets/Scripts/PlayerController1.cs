//using UnityEngine;

//// Controls player movement and rotation.
//public class PlayerController : MonoBehaviour
//{
//    public float speed = 5.0f; // Set player's movement speed.
//    public float rotationSpeed = 120.0f; // Set player's rotation speed.    
//    public float jumpForce = 5.0f;


//    private Rigidbody rb; // Reference to player's Rigidbody.

//    // Start is called before the first frame update
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetButtonDown("Jump"))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
//        }
//    }


//    // Handle physics-based movement and rotation.
//    private void FixedUpdate()
//    {
//        // Move player based on vertical input.
//        float moveVertical = Input.GetAxis("Vertical");
//        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
//        rb.MovePosition(rb.position + movement);

//        // Rotate player based on horizontal input.
//        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
//        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
//        rb.MoveRotation(rb.rotation * turnRotation);
//    }
//}

//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public float speed = 5.0f;
//    public float mouseSensitivity = 100.0f;
//    public Transform cameraTransform;

//    private Rigidbody rb;
//    private float xRotation = 0f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        Cursor.lockState = CursorLockMode.Locked;

//        if (!cameraTransform)
//            Debug.LogError("Assignez la caméra dans l'inspecteur !");
//    }

//    void Update()
//    {
//        // Rotation verticale (haut/bas)
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
//        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//    }

//    void FixedUpdate()
//    {
//        // Déplacement avant/arrière
//        float moveVertical = Input.GetAxis("Vertical");
//        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
//        rb.MovePosition(rb.position + movement);

//        // Rotation horizontale (gauche/droite)
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
//        Quaternion rotation = Quaternion.Euler(0f, mouseX, 0f);
//        rb.MoveRotation(rb.rotation * rotation);
//    }
//}

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public float cameraPitch = 0f;
    public float maxPitch = 80f;
    public float minPitch = -100f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Verrouille et masque le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    /// <summary>
    /// Gère le mouvement de la souris pour faire pivoter la vue
    /// </summary>
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation horizontale du joueur
        transform.Rotate(Vector3.up * mouseX);

        // Rotation verticale de la caméra
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    /// <summary>
    /// Gère les déplacements du joueur (avant/arrière, gauche/droite) sans saut,
    /// supportant les claviers QWERTY et AZERTY (ZQSD)
    /// </summary>
    private void HandleMovement()
    {
        // Lecture du déplacement horizontal (D/Q pour AZERTY, D/A pour QWERTY)
        float x = 0f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A)) x -= 1f;

        // Lecture du déplacement vertical (Z/S pour AZERTY, W/S pour QWERTY)
        float z = 0f;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        // Applique la gravité
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Maintient le joueur au sol
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

//using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float moveSpeed = 5f;
//    public float gravity = -9.81f;

//    [Header("Mouse Settings")]
//    public float mouseSensitivity = 100f;
//    public Transform playerCamera;
//    public float cameraPitch = 0f;
//    public float maxPitch = 80f;
//    public float minPitch = -80f;

//    private CharacterController controller;
//    private Vector3 velocity;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//        // Verrouille et masque le curseur
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    void Update()
//    {
//        HandleMouseLook();
//        HandleMovement();
//    }

//    /// <summary>
//    /// Gère le mouvement de la souris pour faire pivoter la vue
//    /// </summary>
//    private void HandleMouseLook()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        // Rotation horizontale du joueur
//        transform.Rotate(Vector3.up * mouseX);

//        // Rotation verticale de la caméra
//        cameraPitch -= mouseY;
//        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
//        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
//    }

//    /// <summary>
//    /// Gère les déplacements du joueur (avant/arrière, gauche/droite) sans saut,
//    /// en utilisant les axes Unity (Horizontal/Vertical) pour supporter toutes dispositions de clavier.
//    /// </summary>
//    private void HandleMovement()
//    {
//        // Récupère les axes "Horizontal" (A/D ou Q/D) et "Vertical" (W/S ou Z/S) automatiquement
//        float x = Input.GetAxisRaw("Horizontal");
//        float z = Input.GetAxisRaw("Vertical");

//        // Calcule le vecteur de déplacement par rapport à l'orientation du joueur
//        Vector3 move = transform.right * x + transform.forward * z;

//        // Applique le déplacement
//        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

//        // Applique la gravité
//        if (controller.isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // Maintient le joueur au sol
//        }

//        velocity.y += gravity * Time.deltaTime;
//        controller.Move(velocity * Time.deltaTime);
//    }
//}
