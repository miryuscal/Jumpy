using UnityEngine;

public class FishScissors : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxHeight = 5f; // Zirve y�ksekli�i
    public float speed = 2f; // Hareket h�z�
    public bool startAtTop = false; // Ba�lang�� pozisyonu zirvede mi?

    private Vector3 startPosition; // Ba�lang�� konumu
    private float time; // Sin fonksiyonu i�in zaman
    private float previousDisplacement; // �nceki displacement de�eri

    private SpriteRenderer sprite;

    void Start()
    {
        startPosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();

        // Ba�lang�� pozisyonunu ayarla
        if (startAtTop)
        {
            time = Mathf.PI / 2; // Zirvede ba�lamak i�in sin fonksiyonunu ba�lat
        }
        else
        {
            time = 0; // Zeminden ba�lamak i�in sin fonksiyonunu ba�lat
        }
    }

    void Update()
    {
        // Sin fonksiyonu ile hareketi hesapla
        time += Time.deltaTime * speed;
        float displacement = Mathf.Sin(time) * maxHeight;

        // Hareket pozisyonunu g�ncelle
        transform.position = startPosition + new Vector3(0, displacement, 0);

        // �vme kontrol�
        if (displacement < previousDisplacement && !sprite.flipX)
        {
            sprite.flipX = true; // Negatif ivmeye ge�erken flip yap
        }
        else if (displacement > previousDisplacement && sprite.flipX)
        {
            sprite.flipX = false; // Pozitif ivmeye ge�erken flip yap
        }

        // �nceki displacement'i g�ncelle
        previousDisplacement = displacement;
    }
}
