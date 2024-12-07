using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Sallant�n�n s�resi
    public float shakeIntensity = 0.1f; // Sallant�n�n yo�unlu�u
    public float fallDelay = 0.5f; // Sallant� sonras� d��me gecikmesi

    private Vector3 originalPosition; // Blo�un ba�lang�� pozisyonu
    private bool isShaking = false; // Sallanma durumu kontrol�

    private void Start()
    {
        // Blo�un ba�lang�� pozisyonunu kaydediyoruz
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Karakter blo�a temas etti�inde
        if (collision.gameObject.CompareTag("Player") && !isShaking)
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private System.Collections.IEnumerator ShakeAndFall()
    {
        isShaking = true;

        // Sallant� animasyonu
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            // Blo�u rastgele bir pozisyonda sall�yoruz
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Sallant� bitti�inde blo�u eski pozisyonuna geri al
        transform.position = originalPosition;

        // Sallant� sonras� bekleme s�resi
        yield return new WaitForSeconds(fallDelay);

        // Blo�u d���r
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // �ste�e ba�l�: Belirli bir s�re sonra blo�u yok et
        Destroy(gameObject, 2f);
    }
}
