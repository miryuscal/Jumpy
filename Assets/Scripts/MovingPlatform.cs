using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;    
    Vector3 startPosition;
    public float movementDistance = 5f;
    public bool direction = true;
    public float minus = -1f;

    private void Start()
    {
        startPosition = transform.position;

        if (direction == false)
        {
            minus = minus * -1f;
        }
    }

    private void Update()
    {
        // Sin dalgasý kullanarak platformun pozisyonunu hesaplýyoruz
        float movement = Mathf.Sin(Time.time * speed) * movementDistance;
        movement = movement * minus;

        // Platformun pozisyonunu sadece x ekseninde deðiþtiriyoruz
        transform.position = new Vector3(startPosition.x + movement, startPosition.y, startPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer Player platforma temas ederse
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player'ý platformun child'ý yap
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Eðer Player platformdan ayrýlýrsa
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player'ý platformdan ayýr
            collision.transform.SetParent(null);
        }
    }
}
