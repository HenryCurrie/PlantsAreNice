using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Makes the UI face the camera every frame
        transform.LookAt(transform.position + mainCamera.forward);
    }
}