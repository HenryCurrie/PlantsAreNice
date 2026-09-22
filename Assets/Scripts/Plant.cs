using UnityEngine;

public class Plant : MonoBehaviour
{
    // These MUST be public to show up in the Inspector
    public string plantName = "New Plant";
    public float price = 10.0f;

    // These are what the CustomerAI uses
    public Vector3 holdPositionOffset = new Vector3(0f, 0.2f, 0.35f);
    public Vector3 holdRotationOffset = new Vector3(0f, 0f, 0f);
}


