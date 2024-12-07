using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D platformCollider;
    public float minHeightDifference = 0.1f;  // Karakter ile platform arasýndaki minimum mesafe

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Karakterin platforma aþaðýdan geldiðini ve aþaðý hareket ettiðini kontrol et
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            // Yalnýzca karakter aþaðýya hareket ediyorsa ve belirli bir yükseklik farký varsa kolideri devre dýþý býrak
            if (playerRb != null && playerRb.velocity.y <= 0 &&
                collision.transform.position.y < transform.position.y - minHeightDifference)
            {
                platformCollider.enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Karakter platformdan uzaklaþtýðýnda kolideri yeniden etkinleþtir
        if (collision.CompareTag("Player"))
        {
            platformCollider.enabled = true;
        }
    }
}
