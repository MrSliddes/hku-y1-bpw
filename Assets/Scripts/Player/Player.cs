using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player vars")]
    public int currentHp = 100;
    public int maxHp = 100;

    [Header("Camera Stuff")]
    public float mouseSensitivity = 100f;
    public Transform player;
    public Transform playerCamera;
    float rotationX = 0f;

    // Movement
    Vector3 movementInputVector;
    Rigidbody rb;
    public float playerMovementSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCameraMovement();
        PlayerMovementInput();
    }

    private void FixedUpdate()
    {
        FixedPlayerMovement();
    }

    void PlayerCameraMovement()
    {
        // Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        // Clamp the camera rotation
        rotationX = Mathf.Clamp(rotationX, -90, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    void PlayerMovementInput()
    {
        movementInputVector = new Vector3(Input.GetAxis("Horizontal") * playerMovementSpeed, rb.velocity.y, Input.GetAxis("Vertical") * playerMovementSpeed);
    }

    void FixedPlayerMovement()
    {

        Vector3 fm = (rb.transform.forward * movementInputVector.z);
        Vector3 sm = (rb.transform.right * movementInputVector.x);
        rb.MovePosition(transform.position + (fm + sm) * Time.fixedDeltaTime);
    }

    public void PlayerRecieveDamage(int damage)
    {
        currentHp -= damage;
    }
}

public enum PlayerState
{
    idle,
    walking,
    running,
    dying
}
