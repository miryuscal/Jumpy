using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Laser : MonoBehaviour
{
    public LayerMask obstacleMask; // Lazerin çarpacaðý engellerin katmaný
    public float maxLength = 10f; // Lazerin maksimum uzunluðu

    private LineRenderer lineRenderer;
    private BoxCollider2D laserCollider;

    void Start()
    {
        // Lazerin görselini çizecek LineRenderer bileþenini alýyoruz
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.isTrigger = true; // Collider'ý Trigger yap
    }

    void Update()
    {
        DrawLaser();
    }

    void DrawLaser()
    {
        // Lazerin çýkýþ noktasýndan ileriye doðru bir ray gönderiyoruz
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxLength, obstacleMask);

        // LineRenderer'ýn baþlangýç noktasýný belirle
        lineRenderer.SetPosition(0, transform.position);

        Vector3 laserEndPoint;

        if (hit.collider != null)
        {
            // Eðer bir engele çarparsa lazerin sonu engele kadar olur
            laserEndPoint = hit.point;

            // Eðer lazer bir Player'a çarparsa yanma efektini tetikle
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<SpringCharacterController>()?.StartCoroutine("BurnEffect");
            }
        }
        else
        {
            // Eðer engel yoksa lazer maksimum uzunluða kadar uzar
            laserEndPoint = transform.position + transform.up * maxLength;
        }

        // LineRenderer'ýn bitiþ noktasýný ayarla
        lineRenderer.SetPosition(1, laserEndPoint);

        // Collider'ý lazerin uzunluðuna ve pozisyonuna göre ayarla
        UpdateCollider(transform.position, laserEndPoint);
    }

    void UpdateCollider(Vector3 startPoint, Vector3 endPoint)
    {
        // Lazerin ortasýný hesapla
        Vector3 midPoint = (startPoint + endPoint) / 2;

        // Lazerin uzunluðunu hesapla
        float laserLength = Vector3.Distance(startPoint, endPoint);

        // Collider'ýn pozisyonunu ve boyutunu güncelle
        laserCollider.size = new Vector2(0.1f, laserLength); // X ekseni geniþliði, Y ekseni uzunluðu
        laserCollider.offset = transform.InverseTransformPoint(midPoint); // Ortasýný ayarla
        laserCollider.transform.rotation = transform.rotation; // Lazerin rotasyonuna uygun hale getir
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer Player lazerle temas ederse yanma efektini tetikle
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SpringCharacterController>()?.StartCoroutine("BurnEffect");
        }
    }
}
