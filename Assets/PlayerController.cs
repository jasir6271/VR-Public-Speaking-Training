using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    float xRotation = 0f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
    }

    void Update()
    {
        MouseLook();
        Movement();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;
        transform.parent.GetComponent<CharacterController>().Move(moveSpeed * Time.deltaTime * move);
    }
}
