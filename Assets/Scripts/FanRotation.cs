using UnityEngine;

public class FanRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 1000f; // degrees per second

    void Update()
    {
        // Rotate around X-axis
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}
