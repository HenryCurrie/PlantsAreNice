using UnityEngine;
using UnityEngine.InputSystem;
public class CameraLook : MonoBehaviour

{
    public float mouseSensitivity = 0.5f; // New system values are different
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
    // If the game is paused or the start screen is up, don't move the camera
    if (Time.timeScale == 0) 
    {
        // Optional: Ensure the cursor stays visible and unlocked while paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        return; 
    }
        // Get mouse delta from the new system
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

