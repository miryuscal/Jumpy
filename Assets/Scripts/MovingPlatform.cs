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
        // Sin dalgas� kullanarak platformun pozisyonunu hesapl�yoruz
        float movement = Mathf.Sin(Time.time * speed) * movementDistance;
        movement = movement * minus;

        // Platformun pozisyonunu sadece x ekseninde de�i�tiriyoruz
        transform.position = new Vector3(startPosition.x + movement, startPosition.y, startPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // E�er Player platforma temas ederse
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player'� platformun child'� yap
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // E�er Player platformdan ayr�l�rsa
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player'� platformdan ay�r
            collision.transform.SetParent(null);
        }
    }
}
