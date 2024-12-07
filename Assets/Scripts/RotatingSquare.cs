using UnityEngine;

public class RotatingSquare : MonoBehaviour
{
    public Transform[] laserPoints;  // Lazer ��k�� noktalar�
    public GameObject laserPrefab;   // Lazer prefab'�
    public float rotationSpeed = 50f; // Kare nesnesinin d�n�� h�z�

    void Start()
    {
        // Oyunun ba�lang�c�nda lazerleri olu�tur
        SpawnLasers();
    }

    void Update()
    {
        // Kareyi z ekseni etraf�nda d�nd�r
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Lazerlerin rotasyonlar�n� kareyle birlikte g�ncelle
        UpdateLaserDirections();
    }

    void UpdateLaserDirections()
    {
        foreach (Transform laserPoint in laserPoints)
        {
            // Lazerin ��k�� noktas�nda ba�l� olan lazer prefab'lar�n� d�nd�r
            if (laserPoint.childCount > 0)
            {
                Transform laser = laserPoint.GetChild(0);
                laser.rotation = laserPoint.rotation; // Lazerin rotasyonu ��k�� noktas�na g�re g�ncellenir
            }
        }
    }

    public void SpawnLasers()
    {
        foreach (Transform laserPoint in laserPoints)
        {
            // Lazer ��k�� noktalar�nda lazerleri olu�tur
            if (laserPoint.childCount == 0)
            {
                GameObject laser = Instantiate(laserPrefab, laserPoint.position, laserPoint.rotation);
                laser.transform.SetParent(laserPoint); // Lazer, ��k�� noktas�na ba�lan�r
            }
        }
    }
}
