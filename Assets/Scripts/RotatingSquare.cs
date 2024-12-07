using UnityEngine;

public class RotatingSquare : MonoBehaviour
{
    public Transform[] laserPoints;  // Lazer çýkýþ noktalarý
    public GameObject laserPrefab;   // Lazer prefab'ý
    public float rotationSpeed = 50f; // Kare nesnesinin dönüþ hýzý

    void Start()
    {
        // Oyunun baþlangýcýnda lazerleri oluþtur
        SpawnLasers();
    }

    void Update()
    {
        // Kareyi z ekseni etrafýnda döndür
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Lazerlerin rotasyonlarýný kareyle birlikte güncelle
        UpdateLaserDirections();
    }

    void UpdateLaserDirections()
    {
        foreach (Transform laserPoint in laserPoints)
        {
            // Lazerin çýkýþ noktasýnda baðlý olan lazer prefab'larýný döndür
            if (laserPoint.childCount > 0)
            {
                Transform laser = laserPoint.GetChild(0);
                laser.rotation = laserPoint.rotation; // Lazerin rotasyonu çýkýþ noktasýna göre güncellenir
            }
        }
    }

    public void SpawnLasers()
    {
        foreach (Transform laserPoint in laserPoints)
        {
            // Lazer çýkýþ noktalarýnda lazerleri oluþtur
            if (laserPoint.childCount == 0)
            {
                GameObject laser = Instantiate(laserPrefab, laserPoint.position, laserPoint.rotation);
                laser.transform.SetParent(laserPoint); // Lazer, çýkýþ noktasýna baðlanýr
            }
        }
    }
}
