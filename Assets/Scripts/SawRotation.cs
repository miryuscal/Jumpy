using UnityEngine;

public class SawRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Dönüþ hýzý, Inspector'dan ayarlanabilir

    void Update()
    {
        // Nesneyi kendi orijini etrafýnda döndür
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
