using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D platformCollider;
    public float minHeightDifference = 0.1f;  // Karakter ile platform aras�ndaki minimum mesafe

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Karakterin platforma a�a��dan geldi�ini ve a�a�� hareket etti�ini kontrol et
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            // Yaln�zca karakter a�a��ya hareket ediyorsa ve belirli bir y�kseklik fark� varsa kolideri devre d��� b�rak
            if (playerRb != null && playerRb.velocity.y <= 0 &&
                collision.transform.position.y < transform.position.y - minHeightDifference)
            {
                platformCollider.enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Karakter platformdan uzakla�t���nda kolideri yeniden etkinle�tir
        if (collision.CompareTag("Player"))
        {
            platformCollider.enabled = true;
        }
    }
}
