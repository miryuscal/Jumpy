using UnityEngine;

public class ParallaxLayers : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier; // Katmanýn hareket hýzý
    public float parallay = 0f;
    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
