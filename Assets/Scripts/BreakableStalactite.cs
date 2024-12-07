using UnityEngine;

public class BreakableStalactite : MonoBehaviour
{

    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;
    public float fallDelay = 0.5f;

    public Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isShaking)
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private System.Collections.IEnumerator ShakeAndFall()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while(elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        yield return new WaitForSeconds(fallDelay);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<SpringCharacterController>().StartCoroutine("BurnEffect");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
