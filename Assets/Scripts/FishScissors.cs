using UnityEngine;

public class FishScissors : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxHeight = 5f; // Zirve yüksekliði
    public float speed = 2f; // Hareket hýzý
    public bool startAtTop = false; // Baþlangýç pozisyonu zirvede mi?

    private Vector3 startPosition; // Baþlangýç konumu
    private float time; // Sin fonksiyonu için zaman
    private float previousDisplacement; // Önceki displacement deðeri

    private SpriteRenderer sprite;

    void Start()
    {
        startPosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();

        // Baþlangýç pozisyonunu ayarla
        if (startAtTop)
        {
            time = Mathf.PI / 2; // Zirvede baþlamak için sin fonksiyonunu baþlat
        }
        else
        {
            time = 0; // Zeminden baþlamak için sin fonksiyonunu baþlat
        }
    }

    void Update()
    {
        // Sin fonksiyonu ile hareketi hesapla
        time += Time.deltaTime * speed;
        float displacement = Mathf.Sin(time) * maxHeight;

        // Hareket pozisyonunu güncelle
        transform.position = startPosition + new Vector3(0, displacement, 0);

        // Ývme kontrolü
        if (displacement < previousDisplacement && !sprite.flipX)
        {
            sprite.flipX = true; // Negatif ivmeye geçerken flip yap
        }
        else if (displacement > previousDisplacement && sprite.flipX)
        {
            sprite.flipX = false; // Pozitif ivmeye geçerken flip yap
        }

        // Önceki displacement'i güncelle
        previousDisplacement = displacement;
    }
}
