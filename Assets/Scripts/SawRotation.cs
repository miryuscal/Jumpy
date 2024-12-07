using UnityEngine;

public class SawRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // D�n�� h�z�, Inspector'dan ayarlanabilir

    void Update()
    {
        // Nesneyi kendi orijini etraf�nda d�nd�r
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
