using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Sallantýnýn süresi
    public float shakeIntensity = 0.1f; // Sallantýnýn yoðunluðu
    public float fallDelay = 0.5f; // Sallantý sonrasý düþme gecikmesi

    private Vector3 originalPosition; // Bloðun baþlangýç pozisyonu
    private bool isShaking = false; // Sallanma durumu kontrolü

    private void Start()
    {
        // Bloðun baþlangýç pozisyonunu kaydediyoruz
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Karakter bloða temas ettiðinde
        if (collision.gameObject.CompareTag("Player") && !isShaking)
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private System.Collections.IEnumerator ShakeAndFall()
    {
        isShaking = true;

        // Sallantý animasyonu
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            // Bloðu rastgele bir pozisyonda sallýyoruz
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Sallantý bittiðinde bloðu eski pozisyonuna geri al
        transform.position = originalPosition;

        // Sallantý sonrasý bekleme süresi
        yield return new WaitForSeconds(fallDelay);

        // Bloðu düþür
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Ýsteðe baðlý: Belirli bir süre sonra bloðu yok et
        Destroy(gameObject, 2f);
    }
}
