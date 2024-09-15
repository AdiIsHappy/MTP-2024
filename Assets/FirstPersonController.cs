using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 6f; // Movement speed of the player
    public float jumpForce = 5f; // Jump force applied to the Rigidbody
    public float groundDistance = 0.4f; // Distance to check if player is grounded
    public LayerMask groundMask; // LayerMask to identify ground objects
    public Transform groundCheck; // Transform for ground checking
    public Transform playerCamera; // Reference to the player's camera

    public float mouseSensitivity = 100f; // Mouse sensitivity for looking around
    private float xRotation = 0f; // Rotation around the X-axis for looking up and down

    private Rigidbody rb; // Rigidbody reference
    private bool isGrounded; // Is the player grounded?
    private Vector3 moveDirection; // Player's movement direction

    // Torque parameters for upright stabilization
    public float uprightTorque = 10f; // Force used to keep the player upright
    public float maxRotationAngle = 10f; // Maximum angle the player can tilt before correcting

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
    }

    void Update()
    {
        // Ground check to see if the player is standing on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Get input for movement along X and Z axes
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction based on input and camera direction
        moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Handle mouse input for looking around
        HandleMouseLook();

        // Jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Apply movement based on the calculated moveDirection
        MovePlayer();

        // Apply torque to keep the player upright with slight tilting
        ApplyUprightTorque();
    }

    void MovePlayer()
    {
        // Move the player by adding velocity in the movement direction
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y; // Preserve the player's Y velocity (gravity)
        rb.velocity = velocity;
    }

    void Jump()
    {
        // Apply upward force to make the player jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ApplyUprightTorque()
    {
        // Calculate the current tilt angle of the player relative to the world up direction
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * currentRotation;

        // Calculate the angular difference between the current rotation and the target upright rotation
        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(currentRotation);

        // Convert the rotation difference into angular velocity
        Vector3 angularVelocity = new Vector3(rotationDifference.x, rotationDifference.y, rotationDifference.z) * uprightTorque;

        // Stronger damping to prevent oscillation
        float dampingFactor = 0.8f;  // Increase damping to quickly stabilize

        // Apply torque proportional to the angle difference and dampen angular velocity
        rb.AddTorque(angularVelocity - rb.angularVelocity * dampingFactor, ForceMode.VelocityChange);
    }


    void HandleMouseLook()
    {
        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player (yaw) around the Y axis
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera (pitch) around the X axis (looking up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical camera rotation

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply the vertical rotation to the camera
    }
}
